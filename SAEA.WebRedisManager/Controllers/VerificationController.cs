/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Controllers
*类 名 称：VerificationController
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/5/9 11:38:45
*描述：
*=====================================================================
*修改时间：2020/5/9 11:38:45
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.MVC;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Libs.Verification;
using System;
using System.IO;

namespace SAEA.WebRedisManager.Controllers
{
    public class VerificationController : Controller
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(int id = 0)
        {
            try
            {
                HttpContext.Current.Response.ContentType = "image/Gif";

                using (MemoryStream m = new MemoryStream())
                {
                    VerificationCode va = new VerificationCode(105, 30, 4, id);
                    var s = va.Create(m);
                    string code = va.IdentifyingCode;
                    HttpContext.Current.Session["code"] = code;
                    HttpContext.Current.Response.BinaryWrite(m.ToArray());
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<bool>() { Code = 999, Data = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Check(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.ToLower();

                    var rcode = HttpContext.Current.Session["code"];

                    if (rcode != null && rcode.ToString().ToLower() == code)
                    {
                        return Json(new JsonResult<bool>() { Code = 1, Data = true });
                    }
                }

                return Json(new JsonResult<bool>() { Code = 1, Data = false, Message = "验证码不正确！" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<bool>() { Code = 1, Data = false, Message = "验证码不正确！" });
            }
        }
    }
}
