using SAEA.Common;
using SAEA.MVC;
using SAEA.WebRedisManager.Libs;

namespace SAEA.WebRedisManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.Title = "SAEA.WebRedisManager " + SAEAVersion.ToString();

            var config = SAEAMvcApplicationConfigBuilder.Read();

            //config.Port = 16379;

            //config.IsStaticsCached = false;

            SAEAMvcApplicationConfigBuilder.Write(config);

            //启动api

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication(config);

            mvcApplication.Start();

            //启动websocket

            WebSocketsHelper webSocketsHelper = new WebSocketsHelper(port: 16666);

            webSocketsHelper.Start();

            ConsoleHelper.WriteLine("SAEA.WebRedisManager Already started");

            ConsoleHelper.WriteLine($"Please open on Browser：http://127.0.0.1:{config.Port}/");

            ConsoleHelper.WriteLine("Enter to exit service...");

            ConsoleHelper.ReadLine();
        }
    }
}
