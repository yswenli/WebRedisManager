using SAEA.MVC;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Attr;
using SAEA.WebRedisManager.Libs;
using SAEA.WebRedisManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAEA.WebRedisManager.Controllers
{
    /// <summary>
    /// 配置处理api
    /// </summary>
    public class ConfigController : Controller
    {
        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost]
        [Auth(false, true)]
        public ActionResult Set(Config config)
        {
            try
            {
                config.Creator = HttpContext.Current.Session["uid"].ToString();

                ConfigHelper.Set(config);

                return Json(new JsonResult<string>() { Code = 1, Data = string.Empty, Message = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Data = string.Empty, Message = ex.Message });
            }
        }

        [Auth(false, true)]
        [HttpPost]
        public ActionResult SetConfigs(string configs)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(configs))
                {
                    return Json(new JsonResult<string>() { Code = 2, Data = string.Empty, Message = "配置不能为空" });
                }

                var confs = Deserialize<List<Config>>(configs);

                var user = UserHelper.Get(HttpContext.Current.Session["uid"].ToString());

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

                return Json(new JsonResult<string>() { Code = 1, Data = string.Empty, Message = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Data = string.Empty, Message = ex.Message });
            }
        }
        /// <summary>
        /// 获取全部配置
        /// </summary>
        /// <returns></returns>
        [Auth(false, true)]
        public ActionResult GetList()
        {
            try
            {
                var user = UserHelper.Get(HttpContext.Current.Session["uid"].ToString());

                if (user != null)
                {
                    if (user.Role == Role.Admin)
                    {
                        return Json(new JsonResult<List<Config>>() { Code = 1, Data = ConfigHelper.ReadList(), Message = "Ok" });
                    }
                    else
                    {
                        var list = ConfigHelper.ReadList();

                        if (list != null && list.Any())
                        {
                            list = list.Where(b => b.Creator == user.ID).ToList();

                            return Json(new JsonResult<List<Config>>() { Code = 1, Data = list, Message = "Ok" });
                        }
                    }
                }
                return Json(new JsonResult<List<Config>>() { Code = 1, Data = new List<Config>(), Message = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<List<Config>>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Get(string name)
        {
            try
            {
                return Json(new JsonResult<Config>() { Code = 1, Data = ConfigHelper.Get(name), Message = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<Config>() { Code = 2, Message = ex.Message });
            }
        }

        [Auth(false, true)]
        [HttpPost]
        public ActionResult Rem(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name)) return Json(new JsonResult<string>() { Code = 2, Message = "传入的配置项名称不能为空！" });

                var user = UserHelper.Get(HttpContext.Current.Session["uid"].ToString());

                if (user.Role == Role.Admin)
                {
                    ConfigHelper.Rem(name);
                    return Json(new JsonResult<bool>() { Code = 1, Data = true, Message = "Ok" });
                }
                else
                {
                    var config = ConfigHelper.Get(name);

                    if (config == null) return Json(new JsonResult<bool>() { Code = 2, Message = "找不到名称为" + name + "的配置！" });

                    if (config.Creator == user.ID)
                    {
                        ConfigHelper.Rem(name);
                        return Json(new JsonResult<bool>() { Code = 1, Data = true, Message = "Ok" });
                    }
                    else
                    {
                        return Json(new JsonResult<bool>() { Code = 4, Message = "权限不足，请联系管理员！" });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new JsonResult<bool>() { Code = 2, Message = ex.Message });
            }
        }

    }
}
