using System.Text.RegularExpressions;
using TaskSchedulerEngine;
using System.Linq;
using System.Net;

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

    public static HttpClient? _httpClient = null;
    public static HttpClient HttpClient
    {
        get 
        {
            if(_httpClient == null)
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.All
                };        
                _httpClient = new HttpClient(handler);
                _httpClient.DefaultRequestHeaders.Add("User-Agent", $"Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Mobile Safari/537.36 Edg/114.0.1823.43"); 
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/json"); 
                _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br"); 
            }
            return _httpClient;
        }
    }
}