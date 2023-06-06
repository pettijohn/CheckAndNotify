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

    public static ScheduleRule FromCron(string cronExp)
    {
        /*

        # Example of job definition:
        # .---------------- minute (0 - 59)
        # |  .------------- hour (0 - 23)
        # |  |  .---------- day of month (1 - 31)
        # |  |  |  .------- month (1 - 12) OR jan,feb,mar,apr ...
        # |  |  |  |  .---- day of week (0 - 6) (Sunday=0 or 7) OR sun,mon,tue,wed,thu,fri,sat
        # |  |  |  |  |
        # *  *  *  *  *

        */
        var cronParts = Regex.Split(cronExp.Trim(), @"\s+");
        var rule = new ScheduleRule();
        rule.Seconds     = new int[] { 0 };
        rule.Minutes     = cronParts[0] == "*" ? new int[] { } : cronParts[0].Split(",").Select(s => Int32.Parse(s)).ToArray();
        rule.Hours       = cronParts[1] == "*" ? new int[] { } : cronParts[1].Split(",").Select(s => Int32.Parse(s)).ToArray();
        rule.DaysOfMonth = cronParts[2] == "*" ? new int[] { } : cronParts[2].Split(",").Select(s => Int32.Parse(s)).ToArray();
        rule.Months      = cronParts[3] == "*" ? new int[] { } : cronParts[3].Split(",").Select(s => Int32.Parse(s)).ToArray();
        rule.DaysOfWeek  = cronParts[4] == "*" ? new int[] { } : cronParts[4].Split(",").Select(s => Int32.Parse(s)).ToArray();

        return rule;
    }
}