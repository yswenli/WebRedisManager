namespace SAEA.Redis.WebManager.Models
{
    /// <summary>
    /// SAEA.Redis.WebManager配置
    /// </summary>
    public class Config
    {
        public string Name
        {
            get; set;
        }

        public string IP
        {
            get; set;
        }

        public int Port
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }

        public string Creator
        {
            get;
            set;
        }

    }
}
