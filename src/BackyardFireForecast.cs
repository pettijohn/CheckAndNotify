using System.Reflection;
using System.Text.Json.Nodes;
using UnitsNet;
using UnitsNet.Units;

/// <summary>
/// One of the sixteen compass headings, e.g. N, NNE, NE, ...
/// </summary>
/// <param name="Name">N, NNE, NE, ... </param>
/// <param name="DirDeg">Degree heading, 0, 11.25, 22.5, ...</param>
/// <param name="StartDeg">Beginning of range (halfway from previous compass heading)</param>
/// <param name="EndDir">End of range (halfway to next compass heading)</param>
public record Heading(string Name, double DirDeg, double StartDeg, double EndDir);


public static class BackyardFireForecast
{
    
    public static async Task ExecAsync()
    {
        var coordinates = Environment.GetEnvironmentVariable("COORDINATES");
        if (string.IsNullOrEmpty(coordinates)) throw new ArgumentNullException("COORDINATES");

        
        // Headings collection
        var headings = new Dictionary<string, Heading>() {
            {"N",   new Heading("N", 0, 348.75, 11.25)},
            {"NNE", new Heading("NNE", 22.5, 11.25, 33.75)},
            {"NE",  new Heading("NE", 45, 33.75, 56.25)},
            {"ENE", new Heading("ENE", 67.5, 56.25, 78.75)},
            {"E",   new Heading("E", 90, 78.75, 101.25)},
            {"ESE", new Heading("ESE", 112.5, 101.25, 123.75)},
            {"SE",  new Heading("SE", 135, 123.75, 146.25)},
            {"SSE", new Heading("SSE", 157.5, 146.25, 168.75)},
            {"S",   new Heading("S", 180, 168.75, 191.25)},
            {"SSW", new Heading("SSW", 202.5, 191.25, 213.75)},
            {"SW",  new Heading("SW", 225, 213.75, 236.25)},
            {"WSW", new Heading("WSW", 247.5, 236.25, 258.75)},
            {"W",   new Heading("W", 270, 258.75, 281.25)},
            {"WNW", new Heading("WNW", 292.5, 281.25, 303.75)},
            {"NW",  new Heading("NW", 315, 303.75, 326.25)},
            {"NNW", new Heading("NNW", 337.5, 326.25, 348.75)}
        };

        var http = new HttpClient();
        var version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        http.DefaultRequestHeaders.Add("User-Agent", $"CheckAndNotify/{version}"); // required by NWS
        http.DefaultRequestHeaders.Add("Feature-Flags", "forecast_temperature_qv, forecast_wind_speed_qv"); // required by NWS
        http.DefaultRequestHeaders.Add("Accept", "application/geo+json"); // required by NWS
                                                                          //http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

        var url = $"https://api.weather.gov/points/{coordinates}";
        var rawJson = await http.GetStreamAsync(url);
        var document = JsonNode.Parse(rawJson);
        var hourlyUrl = document!["properties"]!["forecastHourly"]!.GetValue<string>();

        url = hourlyUrl;
        rawJson = await http.GetStreamAsync(url);
        document = JsonNode.Parse(rawJson);
        if(document != null && document["status"] != null && document!["status"]!.GetValue<int>() == 500)
        {
            await Util.Log($"NWS returned HTTP 500 {hourlyUrl}");
            return;
        }

        var now = DateTimeOffset.Now;
        var periods = document!["properties"]!["periods"]!.AsArray();
        string message = "Backyard Fire Forecast: \n";
        foreach (var period in periods)
        {

            var startTime = period!["startTime"]!.GetValue<DateTimeOffset>();
            var windSpeed = Speed.FromKilometersPerHour(period!["windSpeed"]!["value"]!.GetValue<double>());
            var windDirection = headings[period!["windDirection"]!.GetValue<String>()]; // "SW"
            var jWindGust = period!["windGust"];//.GetValue<String>(); // Unknown 
            Speed windGust = windSpeed;
            if (jWindGust != null)
            {
                windGust = Speed.FromKilometersPerHour(jWindGust["value"]!.GetValue<double>());
            }

            var probabilityOfPrecipitation = period!["probabilityOfPrecipitation"]!["value"]!.GetValue<double>(); // %

            if (startTime > now && startTime < now.AddHours(16) && startTime.Hour > 16 && startTime.Hour <= 21)
            {
                var windDirIcon = windDirection.DirDeg > 115 && windDirection.DirDeg < 245 ? "✅" : "❌";
                var windSpeedIcon = windSpeed < Speed.FromMilesPerHour(10) && windGust < Speed.FromMilesPerHour(10) ? "✅" : "❌";
                var precipIcon = probabilityOfPrecipitation < 30 ? "✅" :
                    probabilityOfPrecipitation < 60 ? "⚠️" : "❌";

                message += $"{startTime:HH:mm} - Wind from {windDirection.Name}{windDirIcon} @ "
                    + $"{windSpeed.ToUnit(SpeedUnit.MilePerHour):f1}{windSpeedIcon}"
                    + (windGust > windSpeed ? $"G{windGust.ToUnit(SpeedUnit.MilePerHour):f1}" : "")
                    + $" {probabilityOfPrecipitation}% chance rain {precipIcon}"
                    + "\n";
            }
        }

        await Util.Log(message);
        Console.WriteLine(message);
    }
}