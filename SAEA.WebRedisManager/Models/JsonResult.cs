namespace SAEA.WebRedisManager.Models;

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
