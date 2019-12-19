using SAEA.Common;
using SAEA.MVC;

namespace SAEA.WebRedisManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.Title = "WebRedisManager v5.3.1.4";

            var config = SAEAMvcApplicationConfigBuilder.Read();

            config.Port = 16379;
            
            config.Count = 100;

            SAEAMvcApplicationConfigBuilder.Write(config);

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication(config);

            mvcApplication.Start();

            ConsoleHelper.WriteLine("WebRedisManager已启动");

            ConsoleHelper.WriteLine("请在浏览器上打开：http://localhost:16379/");

            ConsoleHelper.WriteLine("回车退出服务...");

            ConsoleHelper.ReadLine();
        }
    }
}
