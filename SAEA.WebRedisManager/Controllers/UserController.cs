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
using SAEA.WebRedisManager.Attr;
using SAEA.WebRedisManager.Models;
using SAEA.WebRedisManager.Services;

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
        public ActionResult Login(string userName, string password, string code)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return Json(new JsonResult<string>() { Code = 2, Message = "用户名或密码不能为空" });

            return Json(new UserService().Login(userName, password, code));
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [Auth(false, true)]
        public ActionResult Logout()
        {
            return Json(new UserService().Logout());
        }

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmPwd"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [Auth(true, true)]
        public ActionResult SetUser(User user, string confirmPwd, int role)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.NickName))

                return Json(new JsonResult<string>() { Code = 2, Message = "用户名、密码或昵称不能为空" });

            if (string.IsNullOrEmpty(confirmPwd) || user.Password != confirmPwd)

                return Json(new JsonResult<string>() { Code = 2, Message = "两次输入的密码不一致" });

            return Json(new UserService().SetUser(user, confirmPwd, role));
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [Auth(true, true)]
        public ActionResult GetUserList()
        {
            return Json(new UserService().GetUserList());
        }

        /// <summary>
        /// 移除用户列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Auth(true, true)]
        public ActionResult Rem(string uid)
        {
            return Json(new UserService().Rem(uid));
        }

        /// <summary>
        /// 是否是空项，是否是首次使用系统
        /// </summary>
        /// <returns></returns>
        public ActionResult IsEmpty()
        {
            return Json(new UserService().IsEmpty());
        }

        /// <summary>
        /// Authenticated
        /// </summary>
        /// <returns></returns>
        public ActionResult Authenticated()
        {
            return Json(new UserService().Authenticated());
        }
    }
}
