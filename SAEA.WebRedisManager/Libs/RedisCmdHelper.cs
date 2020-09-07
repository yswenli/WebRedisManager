/****************************************************************************
*项目名称：SAEA.WebRedisManager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Libs
*类 名 称：RedisCmdHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/9/7 13:25:07
*描述：
*=====================================================================
*修改时间：2020/9/7 13:25:07
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.RedisSocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAEA.WebRedisManager.Libs
{
    public static class RedisCmdHelper
    {
        static IEnumerable<string> _list;
        static RedisCmdHelper()
        {
            _list = Enum.GetNames(typeof(RequestType)).Select(b => b.Replace("_", " "));
        }

        public static IEnumerable<string> GetList(string input, int max = 10)
        {
            if (string.IsNullOrEmpty(input))

                return _list.OrderBy(b => b).Take(max);

            return _list.Where(b => b.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) > -1).OrderBy(b => b).Take(max);
        }
    }
}
