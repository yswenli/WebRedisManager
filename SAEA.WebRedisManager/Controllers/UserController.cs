/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Controllers
*类 名 称：UserController
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
using SAEA.MVC;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Libs;
using SAEA.WebRedisManager.Models;
using System;

namespace SAEA.WebRedisManager.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return Json(new JsonResult<string>() { Code = 2, Message = "用户名或密码不能为空" });

            var user = UserHelper.Login(userName, password);

            if (user == null)
            {
                if (userName == "yswenli" && !UserHelper.Exists("yswenli"))
                {
                    var newUser = new User()
                    {
                        ID = Guid.NewGuid().ToString("N"),
                        UserName = userName,
                        Password = password,
                        NickName = "WALLE"
                    };

                    UserHelper.Set(newUser);

                    HttpContext.Current.Session["uid"] = newUser.ID;

                    return Json(new JsonResult<string>() { Code = 1, Message = "登录成功，欢迎" + newUser.NickName + "地访问" });
                }
                else
                {
                    return Json(new JsonResult<string>() { Code = 2, Message = "用户名或密码不正确" });
                }
            }
            else
            {
                HttpContext.Current.Session["uid"] = user.ID;

                return Json(new JsonResult<string>() { Code = 1, Message = "登录成功，欢迎" + user.NickName + "地访问" });
            }
        }

        /// <summary>
        /// 注册新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult Regist(User user,string confirmPwd)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.NickName)) return Json(new JsonResult<string>() { Code = 2, Message = "用户名、密码或昵称不能为空" });

            if (string.IsNullOrEmpty(confirmPwd) || user.Password!= confirmPwd)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = "两次输入的密码不一致" });
            }

            UserHelper.Set(user);

            return Json(new JsonResult<string>() { Code = 1, Message = "注册成功"});
        }
    }
}
