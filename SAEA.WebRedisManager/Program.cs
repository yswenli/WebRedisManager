/****************************************************************************
*项目名称：SAEA.WebRedisManager
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager
*类 名 称：Program
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*修改时间：2021/7/8 18:24:22
*描述：
*=====================================================================
*修改时间：2021/7/8 18:24:22
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using Microsoft.Extensions.Hosting;

using SAEA.Common;
using SAEA.WebRedisManager.Libs;

namespace SAEA.WebRedisManager
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleHelper.Title = "SAEA.WebRedisManager " + SAEAVersion.ToString();
            }
            catch { }            

            WorkerServiceHelper.CreateHostBuilder<AppService>(args).Build().Run();
        }
    }
}
