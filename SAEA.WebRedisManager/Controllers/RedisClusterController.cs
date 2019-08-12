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
        /// <param name="slaveNodeID"></param>
        /// <param name="masterID"></param>
        /// <returns></returns>
        public ActionResult AddSlave(string name, string slaveNodeID, string masterID)
        {
            try
            {
                var result = false;

                if (CurrentRedisClient.IsCluster(name))
                {
                    result = CurrentRedisClient.AddSlave(name, slaveNodeID, masterID);
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

        public ActionResult AddSlots(string name, string nodeID, string slotStr)
        {
            try
            {
                var result = false;

                if (CurrentRedisClient.IsCluster(name))
                {
                    var slotList = new List<int>();

                    var index = slotStr.IndexOf("-");

                    if (index > -1)
                    {
                        var begin = int.Parse(slotStr.Substring(0, index));

                        var end = int.Parse(slotStr.Substring(index + 1));

                        for (int i = begin; i <= end; i++)
                        {
                            slotList.Add(i);
                        }
                    }
                    else
                    {
                        var slotArr = slotStr.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        if (slotArr != null && slotArr.Any())
                        {
                            foreach (var item in slotArr)
                            {
                                slotList.Add(int.Parse(item));
                            }
                        }
                    }
                    result = CurrentRedisClient.AddSlots(name, nodeID, slotList.ToArray());
                }

                return Json(new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }


        public ActionResult DelSlots(string name, string nodeID, string slotStr)
        {
            try
            {
                var result = false;

                if (CurrentRedisClient.IsCluster(name))
                {
                    var slotList = new List<int>();

                    var index = slotStr.IndexOf("-");

                    if (index > -1)
                    {
                        var begin = int.Parse(slotStr.Substring(0, index));

                        var end = int.Parse(slotStr.Substring(index + 1));

                        for (int i = begin; i <= end; i++)
                        {
                            slotList.Add(i);
                        }
                    }
                    else
                    {
                        var slotArr = slotStr.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        if (slotArr != null && slotArr.Any())
                        {
                            foreach (var item in slotArr)
                            {
                                slotList.Add(int.Parse(item));
                            }
                        }
                    }
                    result = CurrentRedisClient.DelSlots(name, nodeID, slotList.ToArray());
                }

                return Json(new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        public ActionResult SaveConfig(string name, string nodeID)
        {
            try
            {
                var result = false;

                if (CurrentRedisClient.IsCluster(name))
                {
                    result = CurrentRedisClient.SaveConfig(name, nodeID);
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
