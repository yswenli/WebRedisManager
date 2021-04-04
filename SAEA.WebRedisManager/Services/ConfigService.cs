/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Services
*类 名 称：RedisService
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/1/6 13:39:36
*描述：
*=====================================================================
*修改时间：2020/1/6 13:39:36
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using SAEA.Common;
using SAEA.MVC;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;

namespace SAEA.WebRedisManager.Services
{
    class ConfigService
    {
        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public JsonResult<string> Set(Config config)
        {
            try
            {
                config.Creator = HttpContext.Current.Session["uid"].ToString();

                ConfigHelper.Set(config);

                return new JsonResult<string>() { Code = 1, Data = string.Empty, Message = "Ok" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigService.Set", ex, config);
                return new JsonResult<string>() { Code = 2, Data = string.Empty, Message = ex.Message };
            }
        }
    }
}
