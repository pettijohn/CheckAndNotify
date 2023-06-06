
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

        if(args.Length != 0)
        {
            //any argument means run in debug mode & exit

            BackyardFireForecast.Exec();
        }
        else
        {
            var runtime = new TaskEvaluationRuntime();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => runtime.RequestStop();

            var backyardSchedule = Util.FromCron(backyardCron)
                .WithLocalTime()
                .Execute((e, token) => 
                {
                    BackyardFireForecast.Exec();
                    return true;
                });
            runtime.AddSchedule(backyardSchedule);

            await runtime.RunAsync();
        }
    }
}