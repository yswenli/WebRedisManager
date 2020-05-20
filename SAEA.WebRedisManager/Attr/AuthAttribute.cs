/****************************************************************************
*项目名称：SAEA.WebRedisManager.Attr
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Attr
*类 名 称：AuthAttribute
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/1/8 9:29:01
*描述：
*=====================================================================
*修改时间：2020/1/8 9:29:01
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.MVC;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Libs;
using SAEA.WebRedisManager.Models;
using System.Diagnostics;
using System.Text;

namespace SAEA.WebRedisManager.Attr
{
    /// <summary>
    /// 验证并记录日志
    /// </summary>
    public class AuthAttribute : ActionFilterAttribute
    {
        Stopwatch _stopwatch;

        bool _isAdmin = false;

        public AuthAttribute(bool isEnabled) : base(isEnabled)
        {

        }

        public AuthAttribute(bool isAdmin, bool isEnabled) : this(isEnabled)
        {
            _isAdmin = isAdmin;
        }


        public override bool OnActionExecuting()
        {
            _stopwatch = Stopwatch.StartNew();

            if (!HttpContext.Current.Session.Keys.Contains("uid"))
            {
                HttpContext.Current.Response.SetCached(new JsonResult(new JsonResult<string>() { Code = 3, Message = "当前操作需要登录！" }));

                HttpContext.Current.Response.End();

                return false;
            }
            if (_isAdmin)
            {
                var user = UserHelper.Get(HttpContext.Current.Session["uid"].ToString());

                if (user.Role != Role.Admin)
                {
                    HttpContext.Current.Response.SetCached(new JsonResult(new JsonResult<string>() { Code = 4, Message = "当前操作权限不足，请联系管理员！" }));

                    HttpContext.Current.Response.End();

                    return false;
                }
            }

            return true;
        }

        public override void OnActionExecuted(ActionResult result)
        {
            var des = new StringBuilder();

            if (!HttpContext.Current.Session.Keys.Contains("uid"))
            {
                des.Append("user:未登录用户");
            }
            else
            {
                des.Append("user:用户" + HttpContext.Current.Session["uid"]);
            }

            des.Append(",url:" + HttpContext.Current.Request.Url);

            des.Append(",inputs:" + SerializeHelper.Serialize(HttpContext.Current.Request.Parmas));

            des.Append(",outputs:" + SerializeHelper.Serialize(result));

            des.Append(",cost:" + _stopwatch.Elapsed.TotalMilliseconds);

            _stopwatch.Stop();

            LogHelper.Info(HttpContext.Current.Request.Url, result);
        }
    }
}
