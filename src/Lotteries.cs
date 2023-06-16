using System.Net;
using System.Reflection;
using System.Security;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using AngleSharp;

public class Lotteries
{
    public static Decimal NotifyMin
    {
        get => Decimal.Parse(Environment.GetEnvironmentVariable("LOTTO_NOTIFY_MIN")!) * 1_000_000; 
    }
    public async static Task MegaMillionsAsync()
    {
        var url = $"https://www.megamillions.com/cmspages/utilservice.asmx/GetLatestDrawData";
        var response = await Util.HttpClient.PostAsync(url, null);
        var body = await response.Content.ReadAsStringAsync();
        var stripped = Regex.Replace(body, @"\<[^\>]+\>", "");


        var nextDrawing = DateTime.Parse(JsonNode.Parse(stripped)["NextDrawingDate"].GetValue<string>());
        var nextJackpot = JsonNode.Parse(stripped)["Jackpot"]["NextPrizePool"].GetValue<Decimal>();

        var message = $"Next Mega Millions: {nextDrawing.ToString("ddd dd MMM")} ${nextJackpot / 1000000:.} Million";

        Console.WriteLine(message);

        if(nextJackpot > NotifyMin)
            await Util.Log(message);
    }

    public async static Task PowerballAsync()
    {
        var body = await Util.HttpClient.GetStringAsync("https://www.powerball.com");

        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(req => req.Content(body));
        var nextDrawing = DateTime.Parse(document.QuerySelectorAll("div.next-card h5.title-date").First().TextContent);
        var nextJackpotText = document.QuerySelectorAll("div.next-card div.game-detail-group span.game-jackpot-number").First().TextContent;

        int nextJackpot = Int32.MaxValue;
        var match = Regex.Match(nextJackpotText, @"(\d+)");
        if(match.Success)
        {
            nextJackpot = Int32.Parse(match.Groups[1].Value) * 1_000_000;
        }
        var message = $"Next Powerball: {nextDrawing.ToString("ddd dd MMM")} {nextJackpotText}";

        Console.WriteLine(message);
        if(nextJackpot > NotifyMin)
            await Util.Log(message);
    }
}