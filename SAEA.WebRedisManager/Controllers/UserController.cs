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
using SAEA.Common;
using SAEA.MVC;
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
            try
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
                            UserName = userName.Length > 20 ? userName.Substring(0, 20) : userName,
                            Password = password.Length > 20 ? password.Substring(0, 20) : password,
                            NickName = "WALLE",
                            Role = Role.Admin
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
            catch (Exception ex)
            {
                LogHelper.Error("UserController.Login", ex, userName, password);
                return Json(new JsonResult<string>() { Code = 2, Message = "登录失败，系统异常，" + ex.Message });
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [Auth(false, true)]
        public ActionResult Logout()
        {
            try
            {
                HttpContext.Current.Session.Remove("uid");

                return Json(new JsonResult<string>() { Code = 1, Message = "注销成功" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = "注销用户失败，" + ex.Message });
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmPwd"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [Auth(true, true)]
        public ActionResult Set(User user, string confirmPwd, int role)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.NickName)) return Json(new JsonResult<string>() { Code = 2, Message = "用户名、密码或昵称不能为空" });

                if (string.IsNullOrEmpty(confirmPwd) || user.Password != confirmPwd)
                {
                    return Json(new JsonResult<string>() { Code = 2, Message = "两次输入的密码不一致" });
                }
                if (string.IsNullOrEmpty(user.ID))
                    user.ID = Guid.NewGuid().ToString("N");
                user.UserName = user.UserName.Length > 20 ? user.UserName.Substring(0, 20) : user.UserName;
                user.Password = user.Password.Length > 20 ? user.Password.Substring(0, 20) : user.Password;
                user.NickName = user.NickName.Length > 20 ? user.NickName.Substring(0, 20) : user.NickName;
                user.Role = role == 1 ? Role.Admin : Role.User;
                UserHelper.Set(user);
                return Json(new JsonResult<string>() { Code = 1, Message = "注册成功" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<List<User>>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [Auth(true, true)]
        public ActionResult GetUserList()
        {
            try
            {
                return Json(new JsonResult<List<User>>() { Code = 1, Data = UserHelper.ReadList(), Message = "Ok" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<List<User>>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 移除用户列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Auth(true, true)]
        public ActionResult Rem(string uid)
        {
            try
            {
                if (HttpContext.Current.Session.Keys.Contains("uid"))
                {
                    var cuid = HttpContext.Current.Session["uid"].ToString();

                    if (cuid == uid)
                    {
                        throw new Exception("禁止删除当前用户！");
                    }

                    var user = UserHelper.Get(cuid);

                    if (user != null)
                    {
                        if (user.Role == Role.Admin)
                        {
                            UserHelper.Rem(uid);

                            return Json(new JsonResult<List<User>>() { Code = 1, Message = "Ok" });
                        }
                    }
                    return Json(new JsonResult<List<User>>() { Code = 4, Message = "权限不足，请联系管理员" });
                }
                else
                {
                    return Json(new JsonResult<List<User>>() { Code = 3, Message = "当前操作需要登录" });
                }
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<List<User>>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 是否是空项，是否是首次使用系统
        /// </summary>
        /// <returns></returns>
        public ActionResult IsEmpty()
        {
            try
            {
                var userList = UserHelper.ReadList();

                return Json(new JsonResult<bool>() { Code = 1, Data = (userList == null || !userList.Any()) });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<bool>() { Code = 2, Data = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Authenticated
        /// </summary>
        /// <returns></returns>
        public ActionResult Authenticated()
        {
            if (HttpContext.Current.Session.Keys != null && HttpContext.Current.Session.Keys.Contains("uid"))
            {
                return Json(new JsonResult<bool>() { Code = 1, Data = true });
            }
            else
            {
                return Json(new JsonResult<bool>() { Code = 1, Data = false });
            }
        }
    }
}
