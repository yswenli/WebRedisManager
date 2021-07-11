/****************************************************************************
*项目名称：SAEA.WebRedisManager
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager
*类 名 称：AppService
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
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using SAEA.Common;
using SAEA.MVC;
using SAEA.WebRedisManager.Libs;

namespace SAEA.WebRedisManager
{
    public class AppService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            var config = SAEAMvcApplicationConfigBuilder.Read();

            config.Port = 16379;

            config.IsStaticsCached = false;

            SAEAMvcApplicationConfigBuilder.Write(config);

            //启动api

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication(config);

            mvcApplication.Start();

            //启动websocket

            WebSocketsHelper webSocketsHelper = new WebSocketsHelper(port: 16666);

            webSocketsHelper.Start();

            ConsoleHelper.WriteLine("SAEA.WebRedisManager Already started");

            ConsoleHelper.WriteLine($"Please open on Browser：http://127.0.0.1:{config.Port}/");
        }
    }
}
