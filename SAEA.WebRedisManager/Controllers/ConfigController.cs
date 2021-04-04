/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Controllers
*类 名 称：ConfigController
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
using SAEA.WebRedisManager.Services;

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
            return Json(new ConfigService().Set(config));
        }

        /// <summary>
        /// 导入配置
        /// </summary>
        /// <param name="configs"></param>
        /// <returns></returns>
        [Auth(false, true)]
        [HttpPost]
        public ActionResult SetConfigs(string configs)
        {
            return Json(new ConfigService().SetConfigs(configs));
        }
        /// <summary>
        /// 获取全部配置
        /// </summary>
        /// <returns></returns>
        [Auth(false, true)]
        public ActionResult GetList()
        {
            return Json(new ConfigService().GetList());
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Get(string name)
        {
            return Json(new ConfigService().Get(name));
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Auth(false, true)]
        [HttpPost]
        public ActionResult Rem(string name)
        {
            return Json(new ConfigService().Rem(name));
        }

    }
}
