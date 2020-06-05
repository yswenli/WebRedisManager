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
using SAEA.MVC;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SAEA.WebRedisManager.Libs
{
    /// <summary>
    /// redis server info
    /// </summary>
    public static class ServerInfoDataHelper
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isCpu"></param>
        /// <returns></returns>
        public static JsonResult<string> GetInfo(string name, bool isCpu)
        {
            try
            {
                var data = CurrentRedisClient.GetServerInfo(name);

                if (data != null)
                {
                    var result = "0";

                    if (isCpu)
                    {
                        result = CurrentRedisClient.CpuUsed(name).ToString();
                    }
                    else
                    {
                        var totalmem = CurrentRedisClient.GetMaxMem(name);

                        var usemem = double.Parse(data.used_memory);

                        if (totalmem == 0)
                        {
                            result = totalmem.ToString();
                        }
                        else
                        {
                            result = (usemem / totalmem * 100).ToString();
                        }
                    }

                    return new JsonResult<string>() { Code = 1, Data = result, Message = "OK" };
                }
                return new JsonResult<string>() { Code = 2, Message = "暂未读取数据" };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetInfo name:{name}", ex);
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
        }
    }
}
