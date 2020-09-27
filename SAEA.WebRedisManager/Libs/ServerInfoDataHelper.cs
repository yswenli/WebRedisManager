/****************************************************************************
*项目名称：SAEA.WebRedisManager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Libs
*类 名 称：ServerInfoDataHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/6/5 20:09:32
*描述：
*=====================================================================
*修改时间：2020/6/5 20:09:32
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using System;

namespace SAEA.WebRedisManager.Libs
{
    /// <summary>
    /// redis server info
    /// </summary>
    public static class ServerInfoDataHelper
    {
        /// <summary>
        /// GetInfo
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static JsonResult<RedisServerInfo> GetInfo(string name)
        {
            try
            {
                RedisServerInfo redisServerInfo = new RedisServerInfo();

                redisServerInfo.Cpu = CurrentRedisClient.GetCpu(name).ToString();

                redisServerInfo.Memory= CurrentRedisClient.GetUsedMem(name).ToString();

                redisServerInfo.Cmds = CurrentRedisClient.GetOpsCmd(name).ToString();

                redisServerInfo.Input = CurrentRedisClient.GetInput(name).ToString();

                redisServerInfo.Output = CurrentRedisClient.GetOutput(name).ToString();

                return new JsonResult<RedisServerInfo>() { Code = 1, Data = redisServerInfo, Message = "OK" };                
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetInfo name:{name}", ex);
                return new JsonResult<RedisServerInfo>() { Code = 2, Message = ex.Message };
            }
        }
    }
}
