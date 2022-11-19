using System.Collections.Generic;
using System.Linq;
using System.Web;
using me.cqp.luohuaming.NovelAI.Tool.Http;
using Newtonsoft.Json.Linq;
using PublicInfos.Config;

namespace PublicInfos.API;

public class Translate
{
    public static string CallTranslate(string text)
    {
        return AppConfig.TranslateType switch
        {
            "Baidu" => BaiduTextTranslate(text),
            _ => BaiduTextTranslate(text),
        };
    }

    public static string BaiduTextTranslate(string text)
    {
        string url = "https://fanyi-api.baidu.com/api/trans/vip/translate";
        string appid = ConfigHelper.GetConfig<string>("Baidu_AppId");
        string key = ConfigHelper.GetConfig<string>("Baidu_Key");
        long timestamp = CommonHelper.GetTimeStamp();
        using HttpWebClient client = new();
        Dictionary<string, string> dic = new();
        string salt = CommonHelper.MD5Encrypt(key + timestamp);
        dic.Add("q", text);
        dic.Add("from", "zh");
        dic.Add("to", "en");
        dic.Add("appid", appid);
        dic.Add("salt", salt);
        dic.Add("sign", CommonHelper.MD5Encrypt(appid + text + salt + key));
        var formStr = string.Join("&", dic.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));
        var response = client.DownloadString(url + "?" +formStr);
        var json = JObject.Parse(response);
        if (json.ContainsKey("error_code"))
        {
            return "err";
        }
        else
        {
            return json["trans_result"][0]["dst"].ToString();
        }
    }
}