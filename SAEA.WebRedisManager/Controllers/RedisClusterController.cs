using SAEA.Common;
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
                List<ClusterNode> result = new List<ClusterNode>();

                if (CurrentRedisClient.IsCluster(name))
                {
                    result = CurrentRedisClient.GetClusterNodes(name);
                }               

                return Json(new JsonResult<List<ClusterNode>>() { Code = 1, Data = result, Message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ipPort"></param>
        /// <returns></returns>
        public ActionResult AddMaster(string name, string ipPort)
        {
            try
            {
                var result = false;

                if (CurrentRedisClient.IsCluster(name))
                {
                    var ipp = ipPort.ToIPPort();

                    result = CurrentRedisClient.AddNode(name, ipp.Item1, ipp.Item2);
                }

                return Json(new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }
        /// <summary>
        /// 添加从节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="masterID"></param>
        /// <returns></returns>
        public ActionResult AddSlave(string name, string masterID)
        {
            try
            {
                var result = false;

                if (CurrentRedisClient.IsCluster(name))
                {
                    result = CurrentRedisClient.AddSlave(name, masterID);
                }

                return Json(new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        public ActionResult DeleteNode(string name, string nodeID)
        {
            try
            {
                var result = false;

                if (CurrentRedisClient.IsCluster(name))
                {
                    result = CurrentRedisClient.DeleteNode(name, nodeID);
                }

                return Json(new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        public ActionResult SaveConfig(string name)
        {
            try
            {
                var result = false;

                if (CurrentRedisClient.IsCluster(name))
                {
                    result = CurrentRedisClient.SaveConfig(name);
                }

                return Json(new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }
    }
}
