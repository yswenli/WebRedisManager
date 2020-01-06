/****************************************************************************
*项目名称：SAEA.WebRedisManager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Libs
*类 名 称：UsersDataHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/1/6 13:05:09
*描述：
*=====================================================================
*修改时间：2020/1/6 13:05:09
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.WebRedisManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SAEA.WebRedisManager.Libs
{
    static class UsersDataHelper
    {
        static List<UsersData> _list = new List<UsersData>();


        public static string GetCurrentPath(string children)
        {
            var path = Path.Combine(Path.GetDirectoryName(AssemblyHelper.Current.Location), children);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// 添加或更新配置
        /// </summary>
        /// <param name="UsersData"></param>
        public static void Set(UsersData UsersData)
        {
            var old = _list.Where(b => b.RedisConfigName == UsersData.RedisConfigName && b.UID== UsersData.UID).FirstOrDefault();

            if (old == null)
            {
                _list.Add(UsersData);
            }
            else
            {
                _list.Remove(old);
                _list.Add(UsersData);
            }

            Save();
        }

        public static void Set(List<UsersData> UsersDatas)
        {
            _list = UsersDatas;

            Save();
        }

        /// <summary>
        /// 读取配置列表
        /// </summary>
        /// <returns></returns>
        public static List<UsersData> ReadList()
        {
            var filePath = Path.Combine(GetCurrentPath("Config"), "UsersDataConfig.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);

                if (!string.IsNullOrEmpty(json))
                {
                    _list = SerializeHelper.Deserialize<List<UsersData>>(json);
                    if (_list != null && _list.Count > 0)
                        return _list;
                }
            }

            return new List<UsersData>();
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        public static void Save()
        {
            var json = SerializeHelper.Serialize(_list);

            var filePath = Path.Combine(GetCurrentPath("Config"), "UsersDataConfig.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.AppendAllText(filePath, json);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="redisConfigName"></param>
        /// <returns></returns>
        public static UsersData Get(string uid, string redisConfigName)
        {
            if (_list == null || _list.Count < 1) ReadList();

            return _list.Where(b => b.RedisConfigName == redisConfigName && b.UID == uid).FirstOrDefault();
        }

        /// <summary>
        /// 获取配置列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<UsersData> GetList(string uid)
        {
            if (_list == null || _list.Count < 1) ReadList();

            return _list.Where(b => b.UID == uid).ToList();
        }

        /// <summary>
        /// 移除配置
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="redisConfigName"></param>
        public static void Rem(string uid, string redisConfigName)
        {
            if (_list == null || _list.Count < 1) ReadList();

            var UsersData = _list.Where(b =>b.UID== uid && b.RedisConfigName == redisConfigName).FirstOrDefault();

            if (UsersData != null)
            {
                _list.Remove(UsersData);
            }

            Save();
        }

        /// <summary>
        /// 移除配置
        /// </summary>
        /// <param name="uid"></param>
        public static void Rem(string uid)
        {
            if (_list == null || _list.Count < 1) ReadList();

            _list.RemoveAll(b => b.UID == uid);

            Save();
        }
    }
}
