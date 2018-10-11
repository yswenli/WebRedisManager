using SAEA.Common;
using SAEA.Redis.WebManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAEA.Redis.WebManager.Libs
{
    /// <summary>
    /// 配置管理
    /// </summary>
    static class ConfigHelper
    {
        static List<Config> _list = new List<Config>();


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
        /// <param name="config"></param>
        public static void Set(Config config)
        {
            var old = _list.Where(b => b.Name == config.Name).FirstOrDefault();

            if (old == null)
            {
                _list.Add(config);
            }
            else
            {
                _list.Remove(old);
                _list.Add(config);
            }

            Save();
        }

        /// <summary>
        /// 读取配置列表
        /// </summary>
        /// <returns></returns>
        public static List<Config> ReadList()
        {
            var filePath = Path.Combine(GetCurrentPath("Config"), "config.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);

                if (!string.IsNullOrEmpty(json))
                {
                    _list = SerializeHelper.Deserialize<List<Config>>(json);
                    if (_list != null && _list.Count > 0)
                        return _list;
                }
            }

            return new List<Config>();
        }

        /// <summary>
        /// 保存到文件
        /// </summary>
        public static void Save()
        {
            var json = SerializeHelper.Serialize(_list);

            var filePath = Path.Combine(GetCurrentPath("Config"), "config.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.AppendAllText(filePath, json);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Config Get(string name)
        {
            if (_list == null || _list.Count < 1) ReadList();

            return _list.Where(b => b.Name == name).FirstOrDefault();
        }


        /// <summary>
        /// 移除配置
        /// </summary>
        /// <param name="name"></param>
        public static void Rem(string name)
        {
            if (_list == null || _list.Count < 1) ReadList();

            var config = _list.Where(b => b.Name == name).FirstOrDefault();

            if (config != null)
            {
                _list.Remove(config);
            }

            Save();
        }

    }
}
