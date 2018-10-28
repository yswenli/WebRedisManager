using SAEA.Http.Model;
using SAEA.MVC;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using System;
using System.Collections.Generic;

namespace SAEA.Redis.WebManager.Controllers
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
        public ActionResult Set(Config config)
        {
            try
            {
                ConfigHelper.Set(config);

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
        public ActionResult GetList()
        {
            try
            {
                return Json(new JsonResult<List<Config>>() { Code = 1, Data = ConfigHelper.ReadList(), Message = "Ok" });
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
        [HttpPost]
        public ActionResult Rem(string name)
        {
            try
            {
                ConfigHelper.Rem(name);

                return Json(new JsonResult<string>() { Code = 1, Data = "Ok", Message = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

    }
}
