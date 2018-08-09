using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using SAEA.RedisSocket.Model;
using SAEA.WebAPI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.Redis.WebManager.Controllers
{
    /// <summary>
    /// redis相关api
    /// </summary>
    public class RedisController : APIController
    {
        /// <summary>
        /// 连接到redis
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult Connect(string name)
        {
            try
            {
                var config = ConfigHelper.Get(name);

                var cnnResult = CurrentRedisClient.Connect(config);

                if (string.Compare(cnnResult, "OK", true) != 0)
                {
                    throw new Exception(cnnResult);
                }

                var data = CurrentRedisClient.GetDBs(name);

                return Json(new JsonResult<List<int>>() { Code = 1, Data = data, Message = "OK" });
            }
            catch (Exception ex)
            {
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
                    return Json(new JsonResult<string>() { Code = 1, Data = data, Message = "OK" });
                }
                return Json(new JsonResult<string>() { Code = 2, Message = "暂未读取数据" });
            }
            catch (Exception ex)
            {
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
            try
            {
                var data = CurrentRedisClient.GetServerInfo(name);

                if (data != null)
                {
                    var result = "0";
                    if (isCpu)
                        result = data.used_cpu_user.ToString();
                    else
                        result = (int.Parse(data.used_memory) / 1024 / 1024).ToString();

                    return Json(new JsonResult<string>() { Code = 1, Data = result, Message = "OK" });
                }
                return Json(new JsonResult<string>() { Code = 2, Message = "暂未读取数据" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }
        /// <summary>
        /// 获取keys
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult GetKeyTypes(string name, int dbIndex, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key)) key = "*";

                var data = CurrentRedisClient.GetKeyTypes(name, dbIndex, key);

                if (data != null)
                {
                    return Json(new JsonResult<List<KeyType>>() { Code = 1, Data = data.ToKeyTypes(), Message = "OK" });
                }
                return Json(new JsonResult<string>() { Code = 3, Message = "暂未读取数据" });
            }
            catch (Exception ex)
            {
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
        }

        /// <summary>
        /// 提交或修改数据
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
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
                            CurrentRedisClient.StringSet(redisData.Name, redisData.DBIndex, redisData.Key, redisData.Value);
                            break;
                        case 2:
                            CurrentRedisClient.HashSet(redisData.Name, redisData.DBIndex, redisData.ID, redisData.Key, redisData.Value);
                            break;
                        case 3:
                            CurrentRedisClient.SAdd(redisData.Name, redisData.DBIndex, redisData.Key, redisData.Value);
                            break;
                        case 4:
                            CurrentRedisClient.ZAdd(redisData.Name, redisData.DBIndex, redisData.ID, redisData.Key, redisData.Value);
                            break;
                        case 5:
                            CurrentRedisClient.LPush(redisData.Name, redisData.DBIndex, redisData.Key, redisData.Value);
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
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }

        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
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
                return Json(new JsonResult<string>() { Code = 2, Message = ex.Message });
            }
            return Json(result);
        }

    }
}
