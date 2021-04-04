/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Services
*类 名 称：RedisService
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

using SAEA.Common;
using SAEA.Common.Serialization;
using SAEA.Http;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;

namespace SAEA.WebRedisManager.Services
{
    class RedisService
    {
        /// <summary>
        /// 连接到redis
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult<List<int>> Connect(string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {

                    name = HttpUtility.UrlDecode(name);

                    var config = ConfigHelper.Get(name);

                    var cnnResult = CurrentRedisClient.Connect(config);

                    if (string.IsNullOrEmpty(cnnResult) || cnnResult != "OK")
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

                    return new JsonResult<List<int>>() { Code = 1, Data = data, Message = "OK" };
                }

                return new JsonResult<List<int>>() { Code = 3, Data = null, Message = "找不到配置~" };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.Connect name:{name}", ex);
                return new JsonResult<List<int>>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult<object> GetInfoString(string name)
        {
            try
            {
                var data = CurrentRedisClient.GetInfo(name);

                if (!string.IsNullOrEmpty(data))
                {
                    data = data.Replace("\r\n", "<br/>");

                    var obj = new { Config = ConfigHelper.Get(name), Info = data };

                    return new JsonResult<object>() { Code = 1, Data = obj, Message = "OK" };
                }
                return new JsonResult<object>() { Code = 2, Message = "暂未读取数据" };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetInfoString name:{name}", ex);
                return new JsonResult<object>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取客户端连接信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult<string> GetClients(string name)
        {
            try
            {
                var data = CurrentRedisClient.Clients(name);

                if (!string.IsNullOrEmpty(data))
                {
                    return new JsonResult<string>() { Code = 1, Data = data, Message = "OK" };
                }
                return new JsonResult<string>() { Code = 2, Message = "暂未读取数据" };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetClients name:{name}", ex);
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取db中的元素数量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public JsonResult<long> GetDBSize(string name, int dbIndex)
        {
            try
            {
                var data = CurrentRedisClient.GetDBSize(name, dbIndex);

                return new JsonResult<long>() { Code = 1, Data = data, Message = "OK" };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetDBSize name:{name}", ex);
                return new JsonResult<long>() { Code = 2, Message = ex.Message };
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
        public JsonResult<List<KeyType>> GetKeyTypes(int offset, string name, int dbIndex, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key)) key = "*";

                var data = CurrentRedisClient.GetKeyTypes(offset, name, dbIndex, key);

                if (data != null)
                {
                    var dts = data.ToKeyTypes().Take(50).ToList();

                    return new JsonResult<List<KeyType>>() { Code = 1, Data = dts, Message = "OK" };
                }
                return new JsonResult<List<KeyType>>() { Code = 3, Message = "暂未读取数据" };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.GetKeyTypes name:{name}", ex);
                return new JsonResult<List<KeyType>>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取ttls
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="keys"></param>
        public JsonResult<List<int>> GetTtls(string name, int dbIndex, string keys)
        {
            try
            {
                if (!string.IsNullOrEmpty(keys))
                {
                    var data = CurrentRedisClient.GetTTLs2(name, dbIndex, keys).ToList();

                    return new JsonResult<List<int>>() { Code = 1, Data = data, Message = "OK" };
                }
                return new JsonResult<List<int>>() { Code = 3, Message = "未获取ttl数据" };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.BatchRemove name:{name}", ex);
                return new JsonResult<List<int>>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        public JsonResult<long> BatchRemove(string name, int dbIndex, string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var data = CurrentRedisClient.BatchRemove(name, dbIndex, key);

                    return new JsonResult<long>() { Code = 1, Data = data, Message = "OK" };
                }
                return new JsonResult<long>() { Code = 3, Message = "未删除任何数据" };
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.BatchRemove name:{name}", ex);
                return new JsonResult<long>() { Code = 2, Message = ex.Message };
            }
        }

        /// <summary>
        /// 提交或修改数据
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public JsonResult<string> Set(RedisData redisData)
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
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public JsonResult<int> GetCount(RedisData redisData)
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
                return new JsonResult<int>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="redisData"></param>
        public JsonResult<string> Del(RedisData redisData)
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
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public JsonResult<string> Get(RedisData redisData)
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
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 获取子项
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public JsonResult<object> GetItems(int offset, RedisData redisData)
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
                return new JsonResult<object>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <param name="redisData"></param>
        /// <param name="newID"></param>
        /// <returns></returns>
        public JsonResult<string> Rename(RedisData redisData, string newID)
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
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 修改数据项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public JsonResult<string> Edit(RedisData redisData)
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
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public JsonResult<string> DelItem(RedisData redisData)
        {
            var result = new JsonResult<string>() { Code = 3, Message = "操作失败" };
            try
            {
                object data = string.Empty;
                if (redisData != null)
                {
                    redisData.ID = HttpUtility.UrlDecode(redisData.ID);

                    if (!string.IsNullOrEmpty(redisData.Key))
                    {
                        redisData.Key = HttpUtility.UrlDecode(redisData.Key);
                    }
                    if (!string.IsNullOrEmpty(redisData.Value))
                    {
                        redisData.Value = HttpUtility.UrlDecode(redisData.Value);
                    }
                    CurrentRedisClient.DelItem(redisData);
                }
                result.Code = 1;
                result.Message = "ok";
            }
            catch (Exception ex)
            {
                LogHelper.Error($"RedisController.DelItem redisData:{SerializeHelper.Serialize(redisData)}", ex);
                return new JsonResult<string>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd1"></param>
        /// <param name="pwd2"></param>
        /// <returns></returns>
        public JsonResult<bool> AlterPWD(string name, string pwd1, string pwd2)
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
                return new JsonResult<bool>() { Code = 2, Message = ex.Message };
            }
            return result;
        }

    }
}
