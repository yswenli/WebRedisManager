using SAEA.Common;
using SAEA.MVC;

namespace SAEA.WebRedisManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.Title = "WebRedisManager v5.3.2.1";

            var config = SAEAMvcApplicationConfigBuilder.Read();

            config.Port = 16379;

            config.Count = 100;

            SAEAMvcApplicationConfigBuilder.Write(config);

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication(config);

            mvcApplication.Start();

            ConsoleHelper.WriteLine("WebRedisManager Already started");

            ConsoleHelper.WriteLine("Please open on Browser：http://localhost:16379/");

            ConsoleHelper.WriteLine("Enter to exit service...");

            ConsoleHelper.ReadLine();
        }
    }
}
