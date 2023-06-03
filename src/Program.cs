
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

        if(args.Length != 0)
        {
            //any argument means run in debug mode & exit

            BackyardFireForecast.Exec();
        }
        else
        {
            var runtime = new TaskEvaluationRuntime();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => runtime.RequestStop();

            //schedule it
            var afternoon = new ScheduleRule()
                // .AtHours(23)
                // .AtMinutes(30)
                .AtHours(23)
                .AtMinutes(30)
                .AtSeconds(0)
                .WithName("BackyardFireForecast") //Optional ID for your reference 
                .Execute((e, token) => 
                {
                    BackyardFireForecast.Exec();
                    return true;
                });

            runtime.AddSchedule(afternoon);

            await runtime.RunAsync();
        }
    }
}