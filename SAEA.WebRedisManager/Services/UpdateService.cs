using System;

using HtmlAgilityPack;

using SAEA.Common;
using SAEA.Redis.WebManager.Models;

namespace SAEA.WebRedisManager.Services
{
    public class UpdateService
    {
        public JsonResult<string> GetLatest()
        {
            JsonResult<string> result = new JsonResult<string>()
            {
                Data = ""
            };

            try
            {
                var url = "https://github.com/yswenli/WebRedisManager/releases";

                HtmlWeb web = new HtmlWeb();

                HtmlDocument doc = web.Load(url);

                var alinks = doc.DocumentNode.SelectNodes("//div[@class='markdown-body']/p/a");

                if (alinks == null)
                {
                    result.Message = "找不到相关连接地址";
                    result.Code = 2;
                    return result;
                }

                foreach (var item in alinks)
                {
                    var str = item.InnerText;
                    var ver = str.Replace("SAEA.WebRedisManager v", "").Replace(".zip", "");
                    if (new Version(ver) > new Version(SAEAVersion.ToString().Replace("v", "")))
                    {
                        result.Data = item.Attributes["href"].Value;
                    }
                }
                result.Code = 1;
            }
            catch (Exception ex)
            {
                result.Message = "";
                result.Code = 2;
                LogHelper.Error("UpdateService.Update", ex);
            }

            return result;
        }
    }
}
