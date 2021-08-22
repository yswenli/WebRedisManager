/****************************************************************************
*项目名称：SAEA.WebRedisManager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Libs
*类 名 称：WorkerServiceHelper
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
using System.Runtime.InteropServices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SAEA.WebRedisManager.Libs
{
    public static class WorkerServiceHelper
    {
        /// <summary>
        /// 创建传统类型的服务容器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder<T>(string[] args) where T : class, IHostedService
        {
            bool isWinPlantform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWinPlantform)
            {
                return Host.CreateDefaultBuilder(args)
                    .UseWindowsService()
                    .ConfigureServices((hostContext, services) =>
                       {
                           services.AddHostedService<T>();
                       });
            }
            else
            {
                return Host.CreateDefaultBuilder(args)
                    .UseSystemd()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<T>();
                    });

            }
        }
    }
}
