/****************************************************************************
*项目名称：SAEA.WebRedisManagerForNet.Models
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManagerForNet.Models
*类 名 称：RedisServerInfo
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/27 11:02:05
*描述：
*=====================================================================
*修改时间：2020/9/27 11:02:05
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/

namespace SAEA.Redis.WebManager.Models
{
    /// <summary>
    /// redis 服务器信息
    /// </summary>
    public class RedisServerInfo
    {
        public string Cpu { get; set; }

        public string Memory { get; set; }

        public string Cmds { get; set; }

        public string Input { get; set; }

        public string Output { get; set; }
    }
}
