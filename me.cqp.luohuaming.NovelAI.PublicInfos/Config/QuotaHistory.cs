using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PublicInfos.Config;

public class QuotaHistory
{
    
    public static int QueryQuota(long group, long qq)
    {
        string date = DateTime.Now.ToString("d");
        string path = Path.Combine(MainSave.AppDirectory, "Quota.json");
        if (File.Exists(path))
        {
            File.WriteAllText(path, "[]");
        }
        JArray json = JArray.Parse(File.ReadAllText(path));
        foreach (var jToken in json)
        {
            var item = (JObject) jToken;
            if (item["group"].ToObject<long>() == group && item["qq"].ToObject<long>() == qq)
            {
                JObject data = (JObject) item["data"];
                if (data.ContainsKey(date))
                {
                    return AppConfig.MaxPersonQuota - data[date].ToObject<int>();
                }
                else
                {
                    return AppConfig.MaxPersonQuota;
                }
            }
        }

        return AppConfig.MaxPersonQuota;
    }

    public void HandleQuota(long group, long qq, int change)
    {
        string date = DateTime.Now.ToString("d");
        string path = Path.Combine(MainSave.AppDirectory, "Quota.json");
        if (File.Exists(path))
        {
            File.WriteAllText(path, "[]");
        }
        JArray json = JArray.Parse(File.ReadAllText(path));
        bool findFlag = false;
        foreach (var jToken in json)
        {
            var item = (JObject) jToken;
            if (item["group"].ToObject<long>() == group && item["qq"].ToObject<long>() == qq)
            {
                findFlag = true;
                JObject data = (JObject) item["data"];
                if (data.ContainsKey(date))
                {
                    int value = data[date].ToObject<int>() + change;
                    data[date] = value;
                }
                else
                {
                    int value = AppConfig.MaxPersonQuota + change;
                    data.Add(date, value);
                }
            }
        }

        if (!findFlag)
        {
            json.Add(new JObject
            {
                new JProperty("group", group),
                new JProperty("qq", qq),
                new JObject()
                {
                    new JProperty(date, AppConfig.MaxPersonQuota + change)
                }
            });
        }

        File.WriteAllText(path, json.ToString());
    }
}