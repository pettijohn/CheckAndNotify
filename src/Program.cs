using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;


// Build Environment
var wxStation = Environment.GetEnvironmentVariable("WXSTATION");
if (string.IsNullOrEmpty(wxStation)) throw new ArgumentNullException("WXSTATION");

var telegramUrl = Environment.GetEnvironmentVariable("TELEGRAM_URL");
if (String.IsNullOrEmpty(telegramUrl))
{
    DotEnv.Load(".env");
}
telegramUrl = Environment.GetEnvironmentVariable("TELEGRAM_URL");
if (String.IsNullOrEmpty(telegramUrl)) throw new ArgumentNullException("TELEGRAM_URL");

var http = new HttpClient();
var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
http.DefaultRequestHeaders.Add("User-Agent", $"CheckAndNotify/{version}"); // required by NWS
//http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

var url = $"https://w1.weather.gov/xml/current_obs/{wxStation}.xml";
var rawXml = await http.GetStringAsync(url);

XmlSerializer serializer = new XmlSerializer(typeof(CurrentObservation));
using (StringReader reader = new StringReader(rawXml))
{
    var test = (CurrentObservation)serializer.Deserialize(reader)!;
    var message = test.ObservationTimeRfc822;
    message = message.Substring(0, message.Length < 4000 ? message.Length : 4000);
    await http.GetAsync(telegramUrl + "&text=" + System.Net.WebUtility.UrlEncode(message));

    // create real message
}

public static class DotEnv
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        foreach (var line in File.ReadAllLines(filePath))
        {
            var i = line.IndexOf('=');
            if (i == 0) continue;
            var key = line.Substring(0, i);
            var val = line.Substring(i + 1);

            val = val.TrimStart('"').TrimEnd('"');

            Environment.SetEnvironmentVariable(key, val);
        }
    }
}