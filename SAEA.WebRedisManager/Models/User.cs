/****************************************************************************
*项目名称：SAEA.WebRedisManager.Models
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Models
*类 名 称：User
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/1/6 11:13:41
*描述：
*=====================================================================
*修改时间：2020/1/6 11:13:41
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/

namespace SAEA.WebRedisManager.Models
{
    public class User
    {
        public string ID { get; set; }

        public string UserName { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }

    public enum Role
    {
        Admin = 1,
        User = 2
    }
}
