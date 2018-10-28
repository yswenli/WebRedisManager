using SAEA.Http.Model;
using SAEA.MVC;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using SAEA.RedisSocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SAEA.WebRedisManager.Controllers
{
    /// <summary>
    /// Redis cluster controller
    /// </summary>
    public class RedisClusterController : Controller
    {
        /// <summary>
        /// 获取cluster 节点信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult GetClusterNodes(string name)
        {
            try
            {
                var cnnResult = CurrentRedisClient.GetClusterNodes(name);

                return Json(new JsonResult<List<ClusterNode>>() { Code = 1, Data = cnnResult, Message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }
    }
}
