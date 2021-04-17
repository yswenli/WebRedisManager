using SAEA.MVC;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Attr;
using SAEA.WebRedisManager.Services;

namespace SAEA.WebRedisManager.Controllers
{
    /// <summary>
    /// redis相关api
    /// </summary>
    [Auth(true)]
    public class RedisController : Controller
    {
        /// <summary>
        /// 连接到redis
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Connect(string name)
        {
            return Json(new RedisService().Connect(name));
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult GetInfoString(string name)
        {
            return Json(new RedisService().GetInfoString(name));
        }

        /// <summary>
        /// 获取客户端连接信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult GetClients(string name)
        {
            return Json(new RedisService().GetClients(name));
        }




        /// <summary>
        /// 获取db中的元素数量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public ActionResult GetDBSize(string name, int dbIndex)
        {
            return Json(new RedisService().GetDBSize(name, dbIndex));
        }

        /// <summary>
        /// 获取keys
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult GetKeyTypes(int offset, string name, int dbIndex, string key)
        {
            return Json(new RedisService().GetKeyTypes(offset, name, dbIndex, key));
        }

        /// <summary>
        /// 获取ttls
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public ActionResult GetTtls(string name, int dbIndex, string keys)
        {
            return Json(new RedisService().GetTtls(name, dbIndex, keys));
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult BatchRemove(string name, int dbIndex, string key)
        {
            return Json(new RedisService().BatchRemove(name, dbIndex, key));
        }

        /// <summary>
        /// 提交或修改数据
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Set(RedisData redisData)
        {
            return Json(new RedisService().Set(redisData));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public ActionResult GetCount(RedisData redisData)
        {
            return Json(new RedisService().GetCount(redisData));
        }

        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Del(RedisData redisData)
        {
            return Json(new RedisService().Del(redisData));
        }

        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public ActionResult Get(RedisData redisData)
        {
            return Json(new RedisService().Get(redisData));
        }

        /// <summary>
        /// 获取子项
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public ActionResult GetItems(int offset, RedisData redisData)
        {
            return Json(new RedisService().GetItems(offset, redisData));
        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="redisData"></param>
        /// <param name="newID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Rename(RedisData redisData, string newID)
        {
            return Json(new RedisService().Rename(redisData, newID));
        }
        /// <summary>
        /// 修改数据项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(RedisData redisData)
        {
            return Json(new RedisService().Edit(redisData));
        }

        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelItem(RedisData redisData)
        {
            return Json(new RedisService().DelItem(redisData));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd1"></param>
        /// <param name="pwd2"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterPWD(string name, string pwd1, string pwd2)
        {
            return Json(new RedisService().AlterPWD(name, pwd1, pwd2));
        }
    }
}
