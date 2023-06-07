using System.Text.RegularExpressions;
using TaskSchedulerEngine;
using System.Linq;

public static class Util
{
    private static HttpClient httpClient { get; } = new HttpClient();
    public static async Task<HttpResponseMessage> Log(string message)
    {
        string telegramUrl = Environment.GetEnvironmentVariable("TELEGRAM_URL")!; //Checked during program init
        message = message.Substring(0, message.Length < 4000 ? message.Length : 4000);
        var resp = await httpClient.GetAsync($"{telegramUrl}&text={System.Net.WebUtility.UrlEncode(message)}");
        return resp;
    }

    public static void LoadDotEnv(string filePath)
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