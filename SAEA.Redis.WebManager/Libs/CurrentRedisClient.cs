using SAEA.Redis.WebManager.Models;
using SAEA.RedisSocket;
using SAEA.RedisSocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.Redis.WebManager.Libs
{
    /// <summary>
    /// 当前的redisClient
    /// </summary>
    public static class CurrentRedisClient
    {

        static Dictionary<string, RedisClient> _redisClients = new Dictionary<string, RedisClient>();

        /// <summary>
        /// 连接到redis
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string Connect(Config config)
        {
            if (_redisClients.ContainsKey(config.Name))
            {
                var redisClient = _redisClients[config.Name];

                if (!redisClient.IsConnected)
                {
                    return redisClient.Connect();
                }
                return "ok";
            }
            else
            {
                var redisClient = new RedisClient(config.IP + ":" + config.Port, config.Password);

                var result = redisClient.Connect();

                if (result == "OK")
                {
                    _redisClients.Add(config.Name, redisClient);
                }
                return result;
            }
        }


        public static string GetInfo(string name)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    return redisClient.Info();
                }
            }

            return string.Empty;
        }


        public static ServerInfo GetServerInfo(string name)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    return redisClient.ServerInfo;
                }
            }
            return null;

        }


        public static List<int> GetDBs(string name)
        {
            List<int> result = new List<int>();

            var redisClient = _redisClients[name];

            if (redisClient.IsConnected)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (redisClient.Select(i))
                    {
                        result.Add(i);
                    }
                    else
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取keys
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetKeys(int offset, string name, int dbIndex, string key = "*")
        {
            List<string> result = new List<string>();

            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var count = 50;
                    if (!string.IsNullOrEmpty(key) && key != "*")
                    {
                        count = 10000000;
                    }
                    result = redisClient.GetDataBase(dbIndex).Scan(offset, key, count).Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取keytypes
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetKeyTypes(int offset, string name, int dbIndex, string key = "*")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var keys = GetKeys(offset, name, dbIndex, key).Distinct().Take(50).ToList();

            if (keys.Count > 0)
            {
                foreach (var k in keys)
                {
                    var redisClient = _redisClients[name];
                    var type = redisClient.Type(k);
                    result.Add(k, type);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取db的元素数量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public static int GetDBSize(string name, int dbIndex)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    if (redisClient.Select(dbIndex))
                        return redisClient.DBSize();
                }
            }
            return 0;
        }

        /// <summary>
        /// stringset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void StringSet(string name, int dbIndex, string key, string value)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.GetDataBase(dbIndex).Set(key, value);
                }
            }
        }

        /// <summary>
        /// HashSet
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void HashSet(string name, int dbIndex, string hid, string key, string value)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.GetDataBase(dbIndex).HSet(hid, key, value);
                }
            }

        }
        /// <summary>
        /// SAdd
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SAdd(string name, int dbIndex, string key, string value)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.GetDataBase(dbIndex).SAdd(key, value);
                }
            }
        }

        /// <summary>
        /// ZAdd
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="hid"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void ZAdd(string name, int dbIndex, string hid, string key, string value)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.GetDataBase(dbIndex).ZAdd(hid, value, double.Parse(key));
                }
            }

        }

        /// <summary>
        /// LPush
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void LPush(string name, int dbIndex, string key, string value)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.GetDataBase(dbIndex).LPush(key, value);
                }
            }
        }

        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        public static void Del(string name, int dbIndex, string key)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.GetDataBase(dbIndex).Del(key);
                }
            }
        }

        /// <summary>
        /// 获取string
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        public static string Get(string name, int dbIndex, string key)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    return redisClient.GetDataBase(dbIndex).Get(key);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取子项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public static object GetItems(int offset, RedisData redisData)
        {
            object result = new object();

            if (_redisClients.ContainsKey(redisData.Name))
            {
                var redisClient = _redisClients[redisData.Name];

                if (redisClient.IsConnected)
                {
                    if (redisData.Key == null) redisData.Key = "*";

                    switch (redisData.Type)
                    {
                        case 2:
                            var hresult = redisClient.GetDataBase(redisData.DBIndex).HScan(redisData.ID, offset, redisData.Key, 10);
                            if (hresult.Data == null && hresult.Offset > 0)
                            {
                                hresult = redisClient.GetDataBase(redisData.DBIndex).HScan(redisData.ID, hresult.Offset, redisData.Key, 1000000);
                            }
                            result = hresult.Data;
                            break;
                        case 3:
                            var sresult = redisClient.GetDataBase(redisData.DBIndex).SScan(redisData.ID, offset, redisData.Key, 10);
                            if (sresult.Data == null && sresult.Offset > 0)
                            {
                                sresult = redisClient.GetDataBase(redisData.DBIndex).SScan(redisData.ID, offset, redisData.Key, 1000000);
                            }
                            result = sresult.Data;
                            break;
                        case 4:
                            var zresult = redisClient.GetDataBase(redisData.DBIndex).ZScan(redisData.ID, offset, redisData.Key, 10);
                            if (zresult.Data == null && zresult.Offset > 0)
                            {
                                zresult = redisClient.GetDataBase(redisData.DBIndex).ZScan(redisData.ID, offset, redisData.Key, 1000000);
                            }
                            result = zresult.Data;
                            break;
                        case 5:
                            result = redisClient.GetDataBase(redisData.DBIndex).LRang(redisData.ID, offset, 10);
                            break;
                    }
                }
            }
            return result;
        }

        public static int GetItemsCount(int offset, RedisData redisData)
        {
            var result = 0;

            if (_redisClients.ContainsKey(redisData.Name))
            {
                var redisClient = _redisClients[redisData.Name];

                if (redisClient.IsConnected)
                {
                    if (redisData.Key == null) redisData.Key = "*";

                    switch (redisData.Type)
                    {
                        case 2:
                            var hdata = redisClient.GetDataBase(redisData.DBIndex).HScan(redisData.ID, offset, redisData.Key, 10000).Data;
                            result = hdata != null ? hdata.Count : 0;
                            break;
                        case 3:
                            var sdata = redisClient.GetDataBase(redisData.DBIndex).SScan(redisData.ID, offset, redisData.Key, 10000).Data;
                            result = sdata != null ? sdata.Count : 0;
                            break;
                        case 4:
                            var zdata = redisClient.GetDataBase(redisData.DBIndex).ZScan(redisData.ID, offset, redisData.Key, 10000).Data;
                            result = zdata != null ? zdata.Count : 0;
                            break;
                        case 5:
                            var ldata = redisClient.GetDataBase(redisData.DBIndex).LRang(redisData.ID, offset, 10000);
                            result = ldata != null ? ldata.Count : 0;
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 修改id
        /// </summary>
        /// <param name="redisData"></param>
        /// <param name="newID"></param>
        public static void Rename(RedisData redisData, string newID)
        {
            if (_redisClients.ContainsKey(redisData.Name))
            {
                var redisClient = _redisClients[redisData.Name];

                if (redisClient.IsConnected)
                {
                    redisClient.GetDataBase(redisData.DBIndex).Rename(redisData.ID, newID);
                }
            }
        }


        /// <summary>
        /// 修改数据项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public static void Edit(RedisData redisData)
        {
            if (_redisClients.ContainsKey(redisData.Name))
            {
                var redisClient = _redisClients[redisData.Name];

                if (redisClient.IsConnected)
                {
                    switch (redisData.Type)
                    {
                        case 2:
                            redisClient.GetDataBase(redisData.DBIndex).HSet(redisData.ID, redisData.Key, redisData.Value);
                            break;
                        case 3:
                            redisClient.GetDataBase(redisData.DBIndex).SRemove(redisData.ID, redisData.Key);
                            redisClient.GetDataBase(redisData.DBIndex).SAdd(redisData.ID, redisData.Value);
                            break;
                        case 4:
                            redisClient.GetDataBase(redisData.DBIndex).ZAdd(redisData.ID, redisData.Value,double.Parse(redisData.Key));
                            break;
                        case 5:
                            redisClient.GetDataBase(redisData.DBIndex).LSet(redisData.ID, int.Parse(redisData.Key), redisData.Value);
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="redisData"></param>
        public static void DelItem(RedisData redisData)
        {
            if (_redisClients.ContainsKey(redisData.Name))
            {
                var redisClient = _redisClients[redisData.Name];

                if (redisClient.IsConnected)
                {
                    switch (redisData.Type)
                    {
                        case 2:
                            redisClient.GetDataBase(redisData.DBIndex).HDel(redisData.ID, redisData.Key);
                            break;
                        case 3:
                            redisClient.GetDataBase(redisData.DBIndex).SRemove(redisData.ID, redisData.Key);
                            break;
                        case 4:
                            redisClient.GetDataBase(redisData.DBIndex).ZRemove(redisData.ID,new string[] { redisData.Value });
                            break;
                        case 5:
                            redisClient.GetDataBase(redisData.DBIndex).LSet(redisData.ID, int.Parse(redisData.Key), "---VALUE REMOVED BY WEBREDISMANAGER---");
                            redisClient.GetDataBase(redisData.DBIndex).LRemove(redisData.ID, 0, "---VALUE REMOVED BY WEBREDISMANAGER---");
                            break;
                    }
                }
            }
        }



        #region cluster
        public static List<ClusterNode> GetClusterNodes(string name)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var list= redisClient.ClusterNodes;

                    foreach (var item in list)
                    {
                        if (!item.IsMaster)
                        {
                            var masterNode = list.Where(b => b.NodeID == item.MasterNodeID).FirstOrDefault();
                            if (masterNode != null)
                            {
                                item.MinSlots = masterNode.MinSlots;
                                item.MaxSlots = masterNode.MaxSlots;
                            }
                        }                        
                    }
                    return list;
                }
            }
            return null;
        }
        #endregion
    }
}
