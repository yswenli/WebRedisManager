/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.WebRedisManager.Controllers
*类 名 称：ConsoleController
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/26 11:05:29
*描述：
*=====================================================================
*修改时间：2019/3/26 11:05:29
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.MVC;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Libs;
using System;
using System.Collections.Generic;

namespace SAEA.WebRedisManager.Controllers
{
    public class ConsoleController : Controller
    {
        public ActionResult SendCmd(string name, string cmd)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(cmd))
            {
                return Content(CurrentRedisClient.Send(name, cmd));
            }
            return Content("输入的命令不能为空~");
        }

        /// <summary>
        /// 获取输入的命令
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult GetCMD(string input)
        {
            try
            {
                return Json(new JsonResult<IEnumerable<string>>() { Code = 1,Data= RedisCmdHelper.GetList(input) });
            }
            catch(Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }
    }
}
