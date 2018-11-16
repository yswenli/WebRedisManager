using SAEA.Common;
using SAEA.MVC;
using System;
using System.Diagnostics;

namespace SAEA.WebRedisManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.Title = "WebRedisManager";

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication();

            mvcApplication.Start();


            ConsoleHelper.WriteLine("WebRedisManager已启动");

            ConsoleHelper.WriteLine("请在浏览器上打开：http://localhost:39654/");

            ConsoleHelper.WriteLine("回车退出服务...");

            ConsoleHelper.ReadLine();
        }
    }
}
