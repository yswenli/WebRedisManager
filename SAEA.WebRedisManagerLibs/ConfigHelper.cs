/****************************************************************************
*项目名称：SAEA.WebRedisManagerLibs
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.WebRedisManagerLibs
*类 名 称：ConfigHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/1/22 10:54:26
*描述：
*=====================================================================
*修改时间：2019/1/22 10:54:26
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using System.IO;
using System.Text;

namespace SAEA.WebRedisManagerLibs
{
    public static class ConfigHelper
    {

        static ConfigHelper()
        {
            Init();
        }

        static void Init()
        {
            var configPath = PathHelper.GetFilePath(PathHelper.Current, "mvcConfig.json");

            if (!File.Exists(configPath))
            {
                var json = SerializeHelper.Serialize(new Config()
                {
                    root = "/html/",
                    port = 39654,
                    bufferSize = 102400,
                    count = 100,
                    defaultPage = "index.html"
                });

                FileHelper.Write(configPath, Encoding.UTF8.GetBytes(json));
            }
           
        }


        public static Config Get()
        {
            var json = string.Empty;

            var configPath = PathHelper.GetFilePath(PathHelper.Current, "mvcConfig.json");

            if (File.Exists(configPath))

                json = Encoding.UTF8.GetString(FileHelper.Read(configPath));

            return SerializeHelper.Deserialize<Config>(json);
        }

        public static void Set(Config config)
        {
            var configPath = PathHelper.GetFilePath(PathHelper.Current, "mvcConfig.json");

            var json = SerializeHelper.Serialize(config);

            FileHelper.Write(configPath, Encoding.UTF8.GetBytes(json));
        }


        public class Config
        {
            public string root
            {
                get; set;
            }

            public int port
            {
                get; set;
            }

            public int bufferSize
            {
                get; set;
            }

            public int count
            {
                get; set;
            }

            public string defaultPage
            {
                get; set;
            }

        }
    }
}
