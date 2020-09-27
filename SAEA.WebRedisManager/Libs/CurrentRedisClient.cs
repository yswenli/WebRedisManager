using SAEA.Common;
using SAEA.Redis.WebManager.Models;
using SAEA.RedisSocket;
using SAEA.RedisSocket.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAEA.Redis.WebManager.Libs
{
    /// <summary>
    /// 当前的redisClient
    /// </summary>
    public static class CurrentRedisClient
    {

        static ConcurrentDictionary<string, RedisClient> _redisClients = new ConcurrentDictionary<string, RedisClient>();


        static object _locker = new object();

        /// <summary>
        /// 连接到redis
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static string Connect(Config config)
        {
            try
            {
                RedisClient redisClient;

                if (_redisClients.TryGetValue(config.Name, out redisClient))
                {
                    if (!redisClient.IsConnected)
                    {
                        return redisClient.Connect();
                    }
                    return "OK";
                }
                else
                {
                    redisClient = new RedisClient(config.IP + ":" + config.Port, config.Password);

                    var result = redisClient.Connect();

                    if (result == "OK")
                    {
                        _redisClients.TryAdd(config.Name, redisClient);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("连接到redis失败", ex, config);
                return ex.Message;
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

        public static string Clients(string name)
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


        static ConcurrentDictionary<string, double> _cpuUsed = new ConcurrentDictionary<string, double>();


        public static double GetCpu(string name)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var str = redisClient.Info("CPU");

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

                        if (sarr[0] == "used_cpu_sys")
                        {
                            if (_cpuUsed.ContainsKey(name))
                            {
                                var result = double.Parse(sarr[1]) - _cpuUsed[name];
                                _cpuUsed[name] = double.Parse(sarr[1]);
                                return result;
                            }
                            else
                            {
                                _cpuUsed[name] = double.Parse(sarr[1]);
                                return 0;
                            }
                        }
                    }

                }
            }
            return 0D;
        }



        public static double GetUsedMem(string name)
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

                        if (sarr[0] == "used_memory")
                        {
                            return double.Parse(sarr[1]) / 1024 / 1024;
                        }
                    }

                }
            }
            return 0D;
        }


        public static long GetOpsCmd(string name)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var str = redisClient.Info("Stats");

                    if (string.IsNullOrEmpty(str))
                    {
                        return 0L;
                    }

                    var arr = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (arr == null || !arr.Any())
                    {
                        return 0L;
                    }

                    foreach (var item in arr)
                    {
                        var sarr = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                        if (sarr[0] == "instantaneous_ops_per_sec")
                        {
                            return long.Parse(sarr[1]);
                        }
                    }

                }
            }
            return 0L;
        }


        public static double GetInput(string name)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var str = redisClient.Info("Stats");

                    if (string.IsNullOrEmpty(str))
                    {
                        return 0L;
                    }

                    var arr = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (arr == null || !arr.Any())
                    {
                        return 0L;
                    }

                    foreach (var item in arr)
                    {
                        var sarr = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                        if (sarr[0] == "instantaneous_input_kbps")
                        {
                            return double.Parse(sarr[1]);
                        }
                    }

                }
            }
            return 0L;
        }
        public static double GetOutput(string name)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var str = redisClient.Info("Stats");

                    if (string.IsNullOrEmpty(str))
                    {
                        return 0L;
                    }

                    var arr = str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (arr == null || !arr.Any())
                    {
                        return 0L;
                    }

                    foreach (var item in arr)
                    {
                        var sarr = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                        if (sarr[0] == "instantaneous_output_kbps")
                        {
                            return double.Parse(sarr[1]);
                        }
                    }

                }
            }
            return 0L;
        }

        public static bool IsCluster(string name)
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


        public static List<int> GetDBs(string name)
        {
            List<int> result = new List<int>();

            var redisClient = _redisClients[name];

            if (redisClient.IsConnected)
            {
                for (int i = 0; i < 16; i++)
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

            try
            {
                if (_redisClients.ContainsKey(name))
                {
                    var redisClient = _redisClients[name];

                    if (redisClient.IsConnected)
                    {
                        var count = 20;

                        if (!string.IsNullOrEmpty(key) && key != "*" && key != "[" && key != "]")
                        {
                            if (key.IndexOf("*") == -1 && key.IndexOf("[") == -1 && key.IndexOf("]") == -1)
                            {
                                if (redisClient.GetDataBase(dbIndex).Exists(key))
                                {
                                    result.Add(key);
                                }
                                return result;
                            }
                            else
                            {
                                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                                {
                                    var token = cts.Token;

                                    Task.Run(() =>
                                    {
                                        var o = 0;
                                        do
                                        {
                                            var scanData = redisClient.GetDataBase(dbIndex).Scan(o, key, 10000);

                                            if (scanData != null)
                                            {
                                                if (scanData.Data != null && scanData.Data.Any())

                                                    result.AddRange(scanData.Data);

                                                o = scanData.Offset;

                                                if (o == 0) break;
                                            }
                                            if (result.Count >= count) break;
                                        }
                                        while (o > 0 && !token.IsCancellationRequested);
                                    }).Wait();
                                }
                            }
                        }
                        else
                            result = redisClient.GetDataBase(dbIndex).Scan(offset, key, count)?.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("CurrentRedisClient.GetKeys", ex, offset, name, dbIndex, key);
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
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<string> GetAllKeys(ref int offset, string name, int dbIndex, string key = "*", int count = 10000)
        {
            List<string> result = new List<string>();

            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    var data = redisClient.GetDataBase(dbIndex).Scan(offset, key, count);

                    offset = data.Offset;

                    result = data.Data;
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

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static void SetTTL(string name, int dbIndex, string key, int seconds)
        {
            var redisClient = _redisClients[name];

            redisClient.GetDataBase(dbIndex).Expire(key, seconds);
        }
        /// <summary>
        /// 获取过期时间
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetTTL(string name, int dbIndex, string key)
        {
            var redisClient = _redisClients[name];

            return redisClient.GetDataBase(dbIndex).Ttl(key);
        }


        /// <summary>
        /// 获取过期时间
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static List<int> GetTTLs(string name, int dbIndex, string keys)
        {
            if (!string.IsNullOrEmpty(keys))
            {
                var arr = keys.Split(",", StringSplitOptions.RemoveEmptyEntries);

                if (arr != null && arr.Any())
                {
                    var redisClient = _redisClients[name];

                    var db = redisClient.GetDataBase(dbIndex);

                    var batch = db.CreatedBatch();

                    foreach (var item in arr)
                    {
                        batch.TtlAsync(item);
                    }

                    List<int> result = new List<int>();
                    foreach (var item in batch.Execute())
                    {
                        if (int.TryParse(item.ToString(), out int data))
                        {
                            result.Add(data);
                        }
                    }
                    return result;
                }
            }
            return null;
        }

        public static IEnumerable<int> GetTTLs2(string name, int dbIndex, string keys)
        {
            if (!string.IsNullOrEmpty(keys))
            {
                var arr = keys.Split(",", StringSplitOptions.RemoveEmptyEntries);

                if (arr != null && arr.Any())
                {
                    var redisClient = _redisClients[name];

                    var db = redisClient.GetDataBase(dbIndex);

                    foreach (var item in arr)
                    {
                        yield return db.Ttl(item);
                    }
                }
            }
            yield break;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long BatchRemove(string name, int dbIndex, string key)
        {
            long len = 0;

            var offset = 0;

            var redisClient = _redisClients[name];

            using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                var token = cts.Token;

                Task.Run(() =>
                {
                    do
                    {
                        var keys = GetAllKeys(ref offset, name, dbIndex, key, 50).Distinct().ToArray();

                        if (keys != null && keys.Any())
                        {
                            var batch = redisClient.GetDataBase(dbIndex).CreatedBatch();

                            foreach (var item in keys)
                            {
                                batch.DelAsync(item);
                            }

                            batch.Execute().ToList();
                        }

                        len += keys.Length;
                    }
                    while (offset > 0 && !token.IsCancellationRequested);

                }, token).Wait();
            }

            return len;
        }


        /// <summary>
        /// 获取db的元素数量
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public static long GetDBSize(string name, int dbIndex)
        {
            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    redisClient.Select(dbIndex);

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


        public static int HashSetCount(string name, int dbIndex, string hid)
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
                            redisClient.GetDataBase(redisData.DBIndex).LSet(redisData.Key, int.Parse(redisData.Key), "---VALUE REMOVED BY WEBREDISMANAGER---");
                            redisClient.GetDataBase(redisData.DBIndex).LRemove(redisData.Key, 0, "---VALUE REMOVED BY WEBREDISMANAGER---");
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
