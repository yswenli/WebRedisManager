using SAEA.Common;
using SAEA.MVC;
using SAEA.WebRedisManager.Libs;

namespace SAEA.WebRedisManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.Title = "SAEA.WebRedisManager v5.3.3.1";

            var config = SAEAMvcApplicationConfigBuilder.Read();

            config.Port = 16379;

            config.Count = 100;

            SAEAMvcApplicationConfigBuilder.Write(config);

            //启动api

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication(config);

            mvcApplication.Start();

            //启动websocket

            WebSocketsHelper webSocketsHelper = new WebSocketsHelper(port: 16666);

            webSocketsHelper.Start();

            ConsoleHelper.WriteLine("SAEA.WebRedisManager Already started");

            ConsoleHelper.WriteLine("Please open on Browser：http://127.0.0.1:16379/");

            ConsoleHelper.WriteLine("Enter to exit service...");

            ConsoleHelper.ReadLine();
        }
    }
}
