
using TaskSchedulerEngine;

internal class Program
{
    public static async Task Main(string[] args)
    {
        // Build Environment
        var telegramUrl = Environment.GetEnvironmentVariable("TELEGRAM_URL");
        if (string.IsNullOrEmpty(telegramUrl))
        {
            Util.LoadDotEnv(".env");
        }
        telegramUrl = Environment.GetEnvironmentVariable("TELEGRAM_URL");
        if (string.IsNullOrEmpty(telegramUrl)) throw new ArgumentNullException("TELEGRAM_URL");

        var backyardCron = Environment.GetEnvironmentVariable("BACKYARDFIRE_CRON");
        if (string.IsNullOrEmpty(backyardCron)) throw new ArgumentNullException("BACKYARDFIRE_CRON");

        // Run once at startup
        BackyardFireForecast.Exec();

        var runtime = new TaskEvaluationRuntime();
        AppDomain.CurrentDomain.ProcessExit += (s, e) => runtime.RequestStop();

        var heartbeat = runtime.CreateSchedule()
            .WithName("Heartbeat")
            .AtSeconds(0)
            .AtMinutes(0)
            .Execute((e, token) => 
            { 
                Console.WriteLine($"Alive at {e.TimeScheduledUtc}"); return true; 
            });

        var backyardSchedule = runtime.CreateSchedule()
            .FromCron(backyardCron)
            .WithName("BackyardFireForecast")
            .WithLocalTime()
            .Execute((e, token) => 
            {
                BackyardFireForecast.Exec();
                return true;
            });

        Console.WriteLine(heartbeat.ToString());
        Console.WriteLine(backyardSchedule.ToString());

        await runtime.RunAsync();
        
    }
}