using SAEA.Common;
using SAEA.WebAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.Redis.WebManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.Title = "WebRedisManagerService";

            MvcApplication mvcApplication = new MvcApplication(10240, 3000);

            mvcApplication.Start();

            ConsoleHelper.WriteLine("WebRedisManager服务已启动，按回车结束...");

            ConsoleHelper.WriteLine("http://localhost:39654/html/index.html，按回车结束...");

            Process.Start("http://localhost:39654/html/index.html");

            ConsoleHelper.ReadLine();
        }
    }
}
