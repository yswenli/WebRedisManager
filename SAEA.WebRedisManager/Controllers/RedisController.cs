using SAEA.Common;
using SAEA.Http.Model;
using SAEA.MVC;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using SAEA.RedisSocket.Model;
using SAEA.WebRedisManager.Libs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAEA.WebRedisManager.Controllers
{
    /// <summary>
    /// redis相关api
    /// </summary>
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
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    name = SAEA.Http.HttpUtility.UrlDecode(name);

                    var config = ConfigHelper.Get(name);

                    var cnnResult = CurrentRedisClient.Connect(config);

                    if (string.Compare(cnnResult, "OK", true) != 0)
                    {
                        throw new Exception(cnnResult);
                    }

                    var isCluster = CurrentRedisClient.IsCluster(config.Name);

                    var data = new List<int>();

                    if (!isCluster)
                    {
                        data = CurrentRedisClient.GetDBs(name);
                    }
                    else
                    {
                        data.Add(0);
                    }
                    return Json(new JsonResult<List<int>>() { Code = 1, Data = data, Message = "OK" });
                }

                return Json(new JsonResult<List<int>>() { Code = 3, Data = null, Message = "找不到配置~" });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.Connect name:{name}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult GetInfoString(string name)
        {
            try
            {
                var data = CurrentRedisClient.GetInfo(name);

                if (!string.IsNullOrEmpty(data))
                {
                    data = data.Replace("\r\n", "<br/>");

                    var obj = new { Config = ConfigHelper.Get(name), Info = data };

                    return Json(new JsonResult<object>() { Code = 1, Data = obj, Message = "OK" });
                }
                return Json(new JsonResult<string>() { Code = 2, Message = "暂未读取数据" });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetInfoString name:{name}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }


        public ActionResult GetClients(string name)
        {
            try
            {
                var data = CurrentRedisClient.Clients(name);

                if (!string.IsNullOrEmpty(data))
                {
                    return Json(new JsonResult<object>() { Code = 1, Data = data, Message = "OK" });
                }
                return Json(new JsonResult<string>() { Code = 2, Message = "暂未读取数据" });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetClients name:{name}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isCpu"></param>
        /// <returns></returns>
        public ActionResult GetInfo(string name, bool isCpu)
        {
            return Json(ServerInfoDataHelper.GetInfo(name, isCpu));
        }

        

        /// <summary>
        /// 获取db中的元素数量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public ActionResult GetDBSize(string name, int dbIndex)
        {
            try
            {
                var data = CurrentRedisClient.GetDBSize(name, dbIndex);

                return Json(new JsonResult<long>() { Code = 1, Data = data, Message = "OK" });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetDBSize name:{name}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
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
            try
            {
                if (string.IsNullOrEmpty(key)) key = "*";

                var data = CurrentRedisClient.GetKeyTypes(offset, name, dbIndex, key);

                if (data != null)
                {
                    var dts = data.ToKeyTypes().Take(50).ToList();

                    return Json(new JsonResult<List<KeyType>>() { Code = 1, Data = dts, Message = "OK" });
                }
                return Json(new JsonResult<string>() { Code = 3, Message = "暂未读取数据" });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetKeyTypes name:{name}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 获取ttl
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult GetTtl(string name, int dbIndex, string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var data = CurrentRedisClient.GetTTL(name, dbIndex, key);

                    return Json(new JsonResult<int>() { Code = 1, Data = data, Message = "OK" });
                }
                return Json(new JsonResult<int>() { Code = 3, Message = "未获取ttl数据" });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.BatchRemove name:{name}", ex);
                return Json(new JsonResult<int>() { Code = 2, Message = ex.Message });
            }
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
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var data = CurrentRedisClient.BatchRemove(name, dbIndex, key);

                    return Json(new JsonResult<long>() { Code = 1, Data = data, Message = "OK" });
                }
                return Json(new JsonResult<string>() { Code = 3, Message = "未删除任何数据" });
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.BatchRemove name:{name}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 提交或修改数据
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Set(RedisData redisData)
        {
            var result = new JsonResult<string>() { Code = 3, Message = "操作失败" };
            try
            {
                if (redisData != null)
                {
                    switch (redisData.Type)
                    {
                        case 1:
                            CurrentRedisClient.StringSet(redisData.Name, redisData.DBIndex, redisData.ID, redisData.Value);
                            break;
                        case 2:
                            CurrentRedisClient.HashSet(redisData.Name, redisData.DBIndex, redisData.ID, redisData.Key, redisData.Value);
                            break;
                        case 3:
                            CurrentRedisClient.SAdd(redisData.Name, redisData.DBIndex, redisData.ID, redisData.Value);
                            break;
                        case 4:
                            CurrentRedisClient.ZAdd(redisData.Name, redisData.DBIndex, redisData.ID, redisData.Key, redisData.Value);
                            break;
                        case 5:
                            CurrentRedisClient.LPush(redisData.Name, redisData.DBIndex, redisData.ID, redisData.Value);
                            break;
                        default:
                            throw new Exception("不支持的类型，redisData.Type：" + redisData.Type);
                    }

                    if (redisData.TTL > 0) CurrentRedisClient.SetTTL(redisData.Name, redisData.DBIndex, redisData.ID, redisData.TTL);
                }
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.Set redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }

        public ActionResult GetCount(RedisData redisData)
        {
            var result = new JsonResult<int>() { Code = 3, Message = "操作失败" };
            try
            {
                if (redisData != null)
                {
                    switch (redisData.Type)
                    {
                        case 1:

                            break;
                        case 2:
                            result.Data = CurrentRedisClient.HashSetCount(redisData.Name, redisData.DBIndex, redisData.ID);
                            break;
                        case 3:
                            result.Data = CurrentRedisClient.SAddCount(redisData.Name, redisData.DBIndex, redisData.ID);
                            break;
                        case 4:
                            result.Data = CurrentRedisClient.ZAddCount(redisData.Name, redisData.DBIndex, redisData.ID);
                            break;
                        case 5:
                            result.Data = CurrentRedisClient.LLen(redisData.Name, redisData.DBIndex, redisData.ID);
                            break;
                        default:

                            break;
                    }
                }
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetCount redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }

        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Del(RedisData redisData)
        {
            var result = new JsonResult<string>() { Code = 3, Message = "操作失败" };
            try
            {
                if (redisData != null)
                {
                    CurrentRedisClient.Del(redisData.Name, redisData.DBIndex, redisData.Key);
                }
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.Del redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }

        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public ActionResult Get(RedisData redisData)
        {
            var result = new JsonResult<string>() { Code = 3, Message = "操作失败" };
            try
            {
                var data = string.Empty;
                if (redisData != null)
                {
                    data = CurrentRedisClient.Get(redisData.Name, redisData.DBIndex, redisData.Key);
                }
                result.Data = data;
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.Get redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }

        /// <summary>
        /// 获取子项
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public ActionResult GetItems(int offset, RedisData redisData)
        {
            var result = new JsonResult<object>() { Code = 3, Message = "操作失败" };
            try
            {
                object data = string.Empty;
                if (redisData != null)
                {
                    data = CurrentRedisClient.GetItems(offset, redisData);
                }
                result.Data = data;
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetItems redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
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
            var result = new JsonResult<string>() { Code = 3, Message = "操作失败" };
            try
            {
                object data = string.Empty;
                if (redisData != null)
                {
                    CurrentRedisClient.Rename(redisData, newID);
                }
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.Rename redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }
        /// <summary>
        /// 修改数据项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(RedisData redisData)
        {
            var result = new JsonResult<string>() { Code = 3, Message = "操作失败" };
            try
            {
                object data = string.Empty;
                if (redisData != null)
                {
                    CurrentRedisClient.Edit(redisData);
                }
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.Edit redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }

        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelItem(RedisData redisData)
        {
            var result = new JsonResult<string>() { Code = 3, Message = "操作失败" };
            try
            {
                object data = string.Empty;
                if (redisData != null)
                {
                    redisData.ID = SAEA.Http.HttpUtility.UrlDecode(redisData.ID);

                    if (!string.IsNullOrEmpty(redisData.Key))
                    {
                        redisData.Key = SAEA.Http.HttpUtility.UrlDecode(redisData.Key);
                    }
                    if (!string.IsNullOrEmpty(redisData.Value))
                    {
                        redisData.Value = SAEA.Http.HttpUtility.UrlDecode(redisData.Value);
                    }
                    CurrentRedisClient.DelItem(redisData);
                }
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.DelItem redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult AlterPWD(string name, string pwd1, string pwd2)
        {
            var result = new JsonResult<bool>() { Code = 3, Message = "操作失败" };
            try
            {
                object data = string.Empty;
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd1) && !string.IsNullOrEmpty(pwd2) && pwd1 == pwd2)
                {
                    if (CurrentRedisClient.AlterPWD(name, pwd2))
                    {
                        result.Code = 1;
                        result.Data = true;
                        result.Message = "ok";
                    }
                }
                else
                {
                    result.Code = 2;
                    result.Message = "密码不一致！";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.AlterPWD name:{name}", ex);
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }
    }
}
