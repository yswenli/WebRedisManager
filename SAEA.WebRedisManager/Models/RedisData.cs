using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.Redis.WebManager.Models
{
    /// <summary>
    /// 提交Redis的数据
    /// </summary>
    public class RedisData
    {
        public string Name
        {
            get;set;
        }

        public int DBIndex
        {
            get;set;
        }

        public int Type
        {
            get; set;
        }
        public string ID
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

        public string Value
        {
            get; set;
        }
    }
}
