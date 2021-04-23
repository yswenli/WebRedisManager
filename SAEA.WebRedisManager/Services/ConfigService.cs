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
using System.Linq;

using SAEA.Common;
using SAEA.Common.Serialization;
using SAEA.MVC;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Libs;
using SAEA.WebRedisManager.Models;

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
                config.Creator = HttpContext.Current.Request.Cookies["uid"].Value;

                ConfigHelper.Set(config);

                return new JsonResult<string>() { Code = 1, Data = string.Empty, Message = "Ok" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigService.Set", ex, config);
                return new JsonResult<string>() { Code = 2, Data = string.Empty, Message = ex.Message };
            }
        }

        /// <summary>
        /// 导入配置
        /// </summary>
        /// <param name="configs"></param>
        /// <returns></returns>
        public JsonResult<string> SetConfigs(string configs)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(configs))
                {
                    return new JsonResult<string>() { Code = 2, Data = string.Empty, Message = "配置不能为空" };
                }

                var confs = SerializeHelper.Deserialize<List<Config>>(configs);

                var user = UserHelper.Get(HttpContext.Current.Request.Cookies["uid"].Value);

                if (user.Role == Role.User)
                {
                    var old = ConfigHelper.ReadList();

                    if (old != null && old.Any())
                    {
                        old.RemoveAll(b => b.Creator == user.ID);
                    }

                    if (confs != null && confs.Any())
                    {
                        confs = confs.Where(b => b.Creator == user.ID).ToList();

                        if (confs != null || confs.Any())
                        {
                            old.AddRange(confs);
                        }
                    }
                    ConfigHelper.Set(old);
                }
                else
                {
                    ConfigHelper.Set(confs);
                }

                return new JsonResult<string>() { Code = 1, Data = string.Empty, Message = "Ok" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigService.SetConfigs", ex, configs);
                return new JsonResult<string>() { Code = 2, Data = string.Empty, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取全部配置
        /// </summary>
        /// <returns></returns>
        public JsonResult<List<Config>> GetList()
        {
            try
            {
                var user = UserHelper.Get(HttpContext.Current.Request.Cookies["uid"].Value);

                if (user != null)
                {
                    if (user.Role == Role.Admin)
                    {
                        return new JsonResult<List<Config>>() { Code = 1, Data = ConfigHelper.ReadList(), Message = "Ok" };
                    }
                    else
                    {
                        var list = ConfigHelper.ReadList();

                        if (list != null && list.Any())
                        {
                            list = list.Where(b => b.Creator == user.ID).ToList();

                            return new JsonResult<List<Config>>() { Code = 1, Data = list, Message = "Ok" };
                        }
                    }
                }
                return new JsonResult<List<Config>>() { Code = 1, Data = new List<Config>(), Message = "Ok" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigService.GetList", ex);
                return new JsonResult<List<Config>>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name"></param>
        public JsonResult<Config> Get(string name)
        {
            try
            {
                return new JsonResult<Config>() { Code = 1, Data = ConfigHelper.Get(name), Message = "Ok" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigService.Get", ex, name);
                return new JsonResult<Config>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult<bool> Rem(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name)) return new JsonResult<bool>() { Code = 2, Message = "传入的配置项名称不能为空！" };

                var user = UserHelper.Get(HttpContext.Current.Request.Cookies["uid"].Value);

                if (user.Role == Role.Admin)
                {
                    ConfigHelper.Rem(name);
                    return new JsonResult<bool>() { Code = 1, Data = true, Message = "Ok" };
                }
                else
                {
                    var config = ConfigHelper.Get(name);

                    if (config == null) return new JsonResult<bool>() { Code = 2, Message = "找不到名称为" + name + "的配置！" };

                    if (config.Creator == user.ID)
                    {
                        ConfigHelper.Rem(name);
                        return new JsonResult<bool>() { Code = 1, Data = true, Message = "Ok" };
                    }
                    else
                    {
                        return new JsonResult<bool>() { Code = 4, Message = "权限不足，请联系管理员！" };
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("ConfigService.Rem", ex, name);
                return new JsonResult<bool>() { Code = 2, Message = ex.Message };
            }
        }
    }
}
