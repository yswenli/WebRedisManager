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
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetKeys(string name, int dbIndex, string key = "*")
        {
            List<string> result = new List<string>();

            if (_redisClients.ContainsKey(name))
            {
                var redisClient = _redisClients[name];

                if (redisClient.IsConnected)
                {
                    result = redisClient.GetDataBase(dbIndex).Scan(0, key, 50).Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取keytypes
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbIndex"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetKeyTypes(string name, int dbIndex, string key = "*")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            var keys = GetKeys(name, dbIndex, key).Distinct().ToList();

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
                    redisClient.GetDataBase(dbIndex).ZAdd(hid, double.Parse(key), value);
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


    }
}
