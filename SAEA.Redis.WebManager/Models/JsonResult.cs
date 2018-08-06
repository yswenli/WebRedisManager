using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.Redis.WebManager.Models
{
    public class JsonResult<T>
    {
        public int Code
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }

        public T Data
        {
            get; set;
        }
    }
}
