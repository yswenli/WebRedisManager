using SAEA.Common;
using SAEA.Redis.WebManager.Models;
using SAEA.RedisSocket;
using SAEA.RedisSocket.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAEA.Redis.WebManager.Libs
{
    /// <summary>
    /// 当前的redisClient
    /// </summary>
    public static class CurrentRedisClient
    {

        static Dictionary<string, RedisClient> _redisClients = new Dictionary<string, RedisClient>();


        static object _locker = new object();

        /// <summary>
        /// 连接到redis
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string Connect(Config config)
        {
            lock (_locker)
            {
                try
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
                        var redisClient = new RedisClient(config.IP + ":" + config.Port, config.Password, 10);

                        var result = redisClient.Connect();

                        if (result == "OK")
                        {
                            _redisClients[config.Name] = redisClient;
                        }
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }            
        }


        public static string GetInfo(string name)
        {
            lock (_locker)
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
        }

        public static string Clients(string name)
        {
            lock (_locker)
            {
                if (_redisClients.ContainsKey(name))
                {
                    var redisClient = _redisClients[name];

                    if (redisClient.IsConnected)
                    {
                        var list = redisClient.ClientList();

                        if (list != null && list.Any())
                        {
                            StringBuilder sb = new StringBuilder();

                            foreach (var item in list)
                            {
                                var arr = item.Split("id=", StringSplitOptions.RemoveEmptyEntries);

                                if (arr != null && arr.Any())
                                {
                                    foreach (var sitem in arr)
                                    {
                                        sb.Append($"<p class='redisclient_p'>id={sitem}<p>");
                                    }
                                }
                            }
                            return sb.ToString();
                        }
                    }
                }

                return string.Empty;
            }
        }




        public static ServerInfo GetServerInfo(string name)
        {
            lock (_locker)
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
        }


        static ConcurrentDictionary<string, double> _cpuUsed = new ConcurrentDictionary<string, double>();

        public static double CpuUsed(string name)
        {
            var data = CurrentRedisClient.GetServerInfo(name);

            var before = 0D;

            var now = double.Parse(data.used_cpu_sys);

            if (_cpuUsed.ContainsKey(name))
            {
                before = _cpuUsed[name];
            }

            _cpuUsed[name] = now;

            return (now - before) * 100;
        }


        public static double GetMaxMem(string name)
        {
            lock (_locker)
            {
                if (_redisClients.ContainsKey(name))
                {
                    var redisClient = _redisClients[name];

                    if (redisClient.IsConnected)
                    {
                        var str = redisClient.Info("Memory");

                        if (string.IsNullOrEmpty(str))
                        {
                            return 0D;
                        }

                        var arr = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                        if (arr == null || !arr.Any())
                        {
                            return 0D;
                        }

                        foreach (var item in arr)
                        {
                            var sarr = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                            if (sarr[0] == "maxmemory")
                            {
                                return double.Parse(sarr[1]);
                            }
                        }

                    }
                }
                return 0D;
            }
        }


        public static bool IsCluster(string name)
        {
            lock (_locker)
            {
                if (_redisClients.ContainsKey(name))
                {
                    var redisClient = _redisClients[name];

                    if (redisClient.IsConnected)
                    {
                        return redisClient.IsCluster;
                    }
                }
                return false;
            }
        }


        public static List<int> GetDBs(string name)
        {
            lock (_locker)
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
            lock (_locker)
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

                            if (key.IndexOf("*") == -1)
                            {
                                result.Add(redisClient.GetDataBase(dbIndex).Get(key));
                                return result;
                            }
                        }
                        result = redisClient.GetDataBase(dbIndex).Scan(offset, key, count).Data;
                    }
                }
                return result;
            }
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
            lock (_locker)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                var keys = GetKeys(offset, name, dbIndex, key).Distinct().Take(20).ToList();

                if (keys.Count > 0)
                {
                    var redisClient = _redisClients[name];

                    foreach (var k in keys)
                    {
                        var type = redisClient.Type(k);

                        result.Add(k, type);
                    }
                }

                return result;
            }
        }

        public static long BatchRemove(string name, int dbIndex, string key)
        {
            lock (_locker)
            {
                var keys = GetKeys(0, name, dbIndex, key).Distinct().ToArray();

                if (keys.Length > 0)
                {
                    var redisClient = _redisClients[name];

                    foreach (var item in keys)
                    {
                        redisClient.GetDataBase(dbIndex).Del(item);
                    }
                }

                return keys.Length;
            }
        }


        /// <summary>
        /// 获取db的元素数量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public static long GetDBSize(string name, int dbIndex)
        {
            lock (_locker)
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
            lock (_locker)
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
            lock (_locker)
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
        }


        public static int HashSetCount(string name, int dbIndex, string hid)
        {
            lock (_locker)
            {
                if (_redisClients.ContainsKey(name))
                {
                    var redisClient = _redisClients[name];

                    if (redisClient.IsConnected)
                    {
                        return redisClient.GetDataBase(dbIndex).HLen(hid);
                    }
                }
                return 0;
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

        public static int SAddCount(string name, int dbIndex, string key)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    return redisClient.GetDataBase(dbIndex).SLen(key);
                }
            }
            return 0;
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

        public static int ZAddCount(string name, int dbIndex, string zid)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    return redisClient.GetDataBase(dbIndex).ZLen(zid);
                }
            }
            return 0;
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

        public static int LLen(string name, int dbIndex, string key)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    return redisClient.GetDataBase(dbIndex).LLen(key);
                }
            }
            return 0;
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
            lock (_locker)
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
        }

        /// <summary>
        /// 获取子项
        /// </summary>
        /// <param name="redisData"></param>
        /// <returns></returns>
        public static object GetItems(int offset, RedisData redisData)
        {
            lock (_locker)
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
        }

        public static int GetItemsCount(int offset, RedisData redisData)
        {
            lock (_locker)
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
                            redisClient.GetDataBase(redisData.DBIndex).SAdd(redisData.ID, redisData.Value);
                            break;
                        case 4:
                            redisClient.GetDataBase(redisData.DBIndex).ZAdd(redisData.ID, redisData.Value, double.Parse(redisData.Key));
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
                            redisClient.GetDataBase(redisData.DBIndex).SRemove(redisData.Key, redisData.Value);
                            break;
                        case 4:
                            redisClient.GetDataBase(redisData.DBIndex).ZRemove(redisData.ID, new string[] { redisData.Value });
                            break;
                        case 5:
                            redisClient.GetDataBase(redisData.DBIndex).LSet(redisData.ID, int.Parse(redisData.Key), "---VALUE REMOVED BY WEBREDISMANAGER---");
                            redisClient.GetDataBase(redisData.DBIndex).LRemove(redisData.ID, 0, "---VALUE REMOVED BY WEBREDISMANAGER---");
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd1"></param>
        /// <param name="pwd2"></param>
        /// <returns></returns>
        public static bool AlterPWD(string name, string pwd)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];
                var result = redisClient.SetConfig("requirepass", pwd);
                if (result)
                {
                    var config = ConfigHelper.Get(name);
                    config.Password = pwd;
                    ConfigHelper.Set(config);
                }
                return result;
            }
            return false;
        }

        #region cluster

        static RedisClient GetRedisClientByNodeID(string name, string nodeID)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                var slaveNode = redisClient.ClusterNodes.Where(b => b.NodeID == nodeID).FirstOrDefault();

                if (slaveNode == null) return null;

                if (!_redisClients.ContainsKey(nodeID))
                {
                    var rc = new RedisClient(slaveNode.IPPort, redisClient.RedisConfig.Passwords);
                    rc.Connect();
                    _redisClients[nodeID] = rc;
                }

                if (_redisClients[nodeID].IsConnected)
                {
                    return _redisClients[nodeID];
                }
            }
            return null;
        }

        public static List<ClusterNode> GetClusterNodes(string name)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var list = redisClient.ClusterNodes;

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

        public static bool AddNode(string name, string ip, int port)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    return redisClient.AddNode(ip, port);
                }
            }
            return false;
        }


        public static bool AddSlave(string name, string slaveNodeID, string masterID)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                var slaveNode = redisClient.ClusterNodes.Where(b => b.NodeID == slaveNodeID).FirstOrDefault();

                if (slaveNode == null) return false;

                if (!_redisClients.ContainsKey(slaveNodeID))
                {
                    var rc = new RedisClient(slaveNode.IPPort, redisClient.RedisConfig.Passwords);
                    rc.Connect();
                    _redisClients[slaveNodeID] = rc;
                }

                if (_redisClients[slaveNodeID].IsConnected)
                {
                    return _redisClients[slaveNodeID].Replicate(masterID);
                }
            }
            return false;
        }

        public static bool DeleteNode(string name, string nodeID)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    return redisClient.DeleteNode(nodeID);
                }
            }
            return false;
        }

        public static bool MigratingSlots(string name, int[] slots, string nodeID)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.Migratings(slots, nodeID);
                    return true;
                }
            }
            return false;
        }

        public static bool ImportingSlots(string name, int[] slots, string nodeID)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.Importings(slots, nodeID);
                    return true;
                }
            }
            return false;
        }


        public static bool AddSlots(string name, string nodeID, int[] slots)
        {
            var rc = GetRedisClientByNodeID(name, nodeID);

            if (rc != null)
            {
                return rc.AddSlots(slots);
            }

            return false;
        }

        public static bool DelSlots(string name, string nodeID, int[] slots)
        {
            var rc = GetRedisClientByNodeID(name, nodeID);

            if (rc != null)
            {
                return rc.DelSlots(slots);
            }

            return false;
        }

        public static bool SaveConfig(string name, string nodeID)
        {
            var rc = GetRedisClientByNodeID(name, nodeID);

            if (rc != null)
            {
                return rc.SaveClusterConfig();
            }
            return false;
        }

        #endregion

        #region Console

        public static string Send(string name, string cmd)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var @params = cmd.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);

                    if (@params.Length > 0)
                    {
                        var cmdType = @params[0].ToUpper();

                        cmd = cmdType + cmd.Substring(cmdType.Length);

                        return redisClient.Console(cmd).Data;
                    }
                }
            }
            return $"操作失败，cmd:{cmd}";
        }

        #endregion
    }
}
