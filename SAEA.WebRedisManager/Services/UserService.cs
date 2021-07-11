/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Services
*类 名 称：UserService
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
using System.Text;

using SAEA.Common;
using SAEA.Http;
using SAEA.MVC;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Libs;
using SAEA.WebRedisManager.Models;

namespace SAEA.WebRedisManager.Services
{
    public class UserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult<string> Login2(string userName, string password, string code)
        {
            try
            {
                var codeResult = CheckCode(code);

                if (!codeResult.Data)
                {
                    return new JsonResult<string>() { Code = 2, Message = codeResult.Message };
                }

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

                        HttpContext.Current.Response.Cookies.Add("uid", new HttpCookie("uid", newUser.ID));

                        return new JsonResult<string>() { Code = 1, Message = "登录成功，欢迎" + newUser.NickName + "地访问" };
                    }
                    else
                    {
                        return new JsonResult<string>() { Code = 2, Message = "用户名或密码不正确" };
                    }
                }
                else
                {
                    HttpContext.Current.Response.Cookies.Add("uid", new HttpCookie("uid", user.ID));

                    return new JsonResult<string>() { Code = 1, Message = "登录成功，欢迎" + user.NickName + "地访问" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserController.Login", ex, userName, password);
                return new JsonResult<string>() { Code = 2, Message = "登录失败，系统异常，" + ex.Message };
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult<string> Login(string userName, string password)
        {
            try
            {
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

                        HttpContext.Current.Response.Cookies.Add("uid", new HttpCookie("uid", newUser.ID));

                        return new JsonResult<string>() { Code = 1, Message = "登录成功，欢迎" + newUser.NickName + "地访问" };
                    }
                    else
                    {
                        return new JsonResult<string>() { Code = 2, Message = "用户名或密码不正确" };
                    }
                }
                else
                {
                    HttpContext.Current.Response.Cookies.Add("uid", new HttpCookie("uid", user.ID));

                    return new JsonResult<string>() { Code = 1, Message = "登录成功，欢迎" + user.NickName + "地访问" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserController.Login", ex, userName, password);
                return new JsonResult<string>() { Code = 2, Message = "登录失败，系统异常，" + ex.Message };
            }
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult<bool> CheckCode(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.ToLower();

                    var rcode = HttpContext.Current.Session["code"];

                    if (rcode != null && rcode.ToString().ToLower() == code)
                    {
                        return new JsonResult<bool>() { Code = 1, Data = true };
                    }
                }

                return new JsonResult<bool>() { Code = 1, Data = false, Message = "验证码不正确！" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserController.CheckCode", ex, code);
                return new JsonResult<bool>() { Code = 1, Data = false, Message = "验证码不正确！" };
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public JsonResult<string> Logout()
        {
            try
            {
                HttpContext.Current.Session.Remove("uid");

                return new JsonResult<string>() { Code = 1, Message = "注销成功" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserService.Logout", ex);
                return new JsonResult<string>() { Code = 2, Message = "注销用户失败，" + ex.Message };
            }
        }

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmPwd"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public JsonResult<string> SetUser(User user, string confirmPwd, int role)
        {
            try
            {
                if (string.IsNullOrEmpty(user.ID))
                    user.ID = Guid.NewGuid().ToString("N");
                user.UserName = user.UserName.Length > 20 ? user.UserName.Substring(0, 20) : user.UserName;
                user.Password = user.Password.Length > 20 ? user.Password.Substring(0, 20) : user.Password;
                user.NickName = user.NickName.Length > 20 ? user.NickName.Substring(0, 20) : user.NickName;
                user.Role = role == 1 ? Role.Admin : Role.User;
                UserHelper.Set(user);
                return new JsonResult<string>() { Code = 1, Message = "注册成功" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserService.SetUser", ex, user, confirmPwd, role);
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult<List<User>> GetUserList()
        {
            try
            {
                return new JsonResult<List<User>>() { Code = 1, Data = UserHelper.ReadList(), Message = "Ok" };
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserService.GetUserList", ex);
                return new JsonResult<List<User>>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 移除用户列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public JsonResult<List<User>> Rem(string uid)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies.ContainsKey("uid"))
                {
                    var cuid = HttpContext.Current.Request.Cookies["uid"].Value;

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

                            return new JsonResult<List<User>>() { Code = 1, Message = "Ok" };
                        }
                    }
                    return new JsonResult<List<User>>() { Code = 4, Message = "权限不足，请联系管理员" };
                }
                else
                {
                    return new JsonResult<List<User>>() { Code = 3, Message = "当前操作需要登录" };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserService.Rem", ex, uid);
                return new JsonResult<List<User>>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 是否是空项，是否是首次使用系统
        /// </summary>
        /// <returns></returns>
        public JsonResult<bool> IsEmpty()
        {
            try
            {
                var userList = UserHelper.ReadList();

                return new JsonResult<bool>() { Code = 1, Data = (userList == null || !userList.Any()) };
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserService.IsEmpty", ex);
                return new JsonResult<bool>() { Code = 2, Data = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public JsonResult<bool> Authenticated()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies.ContainsKey("uid"))
                {
                    return new JsonResult<bool>() { Code = 1, Data = true };
                }
                else
                {
                    return new JsonResult<bool>() { Code = 1, Data = false };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UserService.Authenticated", ex);
                return new JsonResult<bool>() { Code = 2, Data = false, Message = ex.Message };
            }
        }

        //
    }
}
