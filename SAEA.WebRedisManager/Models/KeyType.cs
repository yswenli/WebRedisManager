using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.Redis.WebManager.Models
{
    public class KeyType
    {
        public string Key
        {
            get; set;
        }

        public string Type
        {
            get; set;
        }
    }

    public static class Extiontions
    {
        public static List<KeyType> ToKeyTypes(this Dictionary<string, string> dic)
        {
            if (dic == null) return null;

            List<KeyType> result = new List<KeyType>();

            foreach (var item in dic)
            {
                result.Add(new KeyType()
                {
                    Key = item.Key,
                    Type = item.Value
                });
            }

            return result;
        }
    }
}
