using me.cqp.luohuaming.NovelAI.Tool.Http;
using Newtonsoft.Json.Linq;
using PublicInfos.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PublicInfos.API;

public class NovelAI
{
    public static APIResult Txt2Img(string prompt)
    {
        return Txt2Img(prompt, AppConfig.NegativePrompt, AppConfig.Steps, AppConfig.Height, AppConfig.Width
            , AppConfig.RestoreFaces, AppConfig.SamplingMethod);
    }

    public static APIResult Img2Img(string img, string prompt)
    {
        return Img2Img(img, prompt, AppConfig.NegativePrompt, AppConfig.Steps, AppConfig.Height, AppConfig.Width
            , AppConfig.RestoreFaces, AppConfig.SamplingMethod);
    }

    public static APIResult Txt2Img(string prompt, string negative_prompt, int steps, int height, int width,
        bool restore_faces, string engine)
    {
        AppConfig.Using = true;
        try
        {
            APIResult result = new();
            string body = @"{  
        ""enable_hr"": true,
        ""denoising_strength"": 0.7,
        ""prompt"": ""%prompt%"",
        ""steps"": %steps%,
        ""cfg_scale"": 7,
        ""width"": %width%,
        ""height"": %height%,
        ""restore_faces"": %restore_faces%,
        ""negative_prompt"": ""%negative_prompt%"",
        ""sampler_index"": ""%engine%""
    }";
            body = body.Replace("%prompt%", prompt)
                .Replace("%negative_prompt%", negative_prompt)
                .Replace("%steps%", steps.ToString())
                .Replace("%height%", height.ToString())
                .Replace("%width%", width.ToString())
                .Replace("%restore_faces%", restore_faces.ToString().ToLower())
                .Replace("%engine%", engine);
            string r = new HttpWebClient(AppConfig.Timeout * 1000).UploadString(AppConfig.APIBaseUrl + "sdapi/v1/txt2img", body);
            JObject json = JObject.Parse(r);
            if (json.ContainsKey("images"))
            {
                result.IsSuccess = true;
                result.Result = (json["images"] as JArray)[0].ToString();
            }
            if (result.Result.Length < 10240)
            {
                result.R18 = true;
            }
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            AppConfig.Using = false;
        }
    }

    public static APIResult Img2Img(string img, string prompt, string negative_prompt, int steps, int height, int width,
        bool restore_faces, string engine)
    {
        AppConfig.Using = true;
        try
        {

            APIResult result = new();
            string body = @"{
  ""init_images"": [
    ""%img%""
  ],
  ""resize_mode"": 0,
  ""denoising_strength"": 0.75,
  ""prompt"": ""%prompt%"",
  ""steps"": %steps%,
  ""cfg_scale"": 7,
  ""width"": %width%,
  ""height"": %height%,
  ""restore_faces"": %restore_faces%,
  ""negative_prompt"": ""%negative_prompt%"",
  ""sampler_index"": ""%engine%""
}";
            body = body.Replace("%prompt%", prompt)
                .Replace("%negative_prompt%", negative_prompt)
                .Replace("%img%", img)
                .Replace("%steps%", steps.ToString())
                .Replace("%height%", height.ToString())
                .Replace("%width%", width.ToString())
                .Replace("%restore_faces%", restore_faces.ToString().ToLower())
                .Replace("%engine%", engine);
            string r = new HttpWebClient(AppConfig.Timeout * 1000).UploadString(AppConfig.APIBaseUrl + "sdapi/v1/img2img", body);
            JObject json = JObject.Parse(r);
            if (json.ContainsKey("images"))
            {
                result.IsSuccess = true;
                result.Result = (json["images"] as JArray)[0].ToString();
            }
            if (result.Result.Length < 10240)
            {
                result.R18 = true;
            }
            return result;
        }
        catch (Exception e) { throw e; }
        finally
        {
            AppConfig.Using = false;
        }

    }

    public static bool ServiceOnline()
    {
        try
        {
            string url = AppConfig.APIBaseUrl + "";
            HttpWebClient.Get(url);
            return true;
        }
        catch 
        {
            return false;
        }
    }

    public static void StartService()
    {
        if(ServiceOnline())
        {
            MainSave.CQLog.Info("WebUI启动", "已经启动");
            return;
        }
        if(File.Exists(AppConfig.WebUIPath))
        {
            Process.Start(AppConfig.WebUIPath);
            Thread.Sleep(2000);

            int timeout = 5 * 60;
            for(int i = 0; i < timeout; i++)
            {
                if(ServiceOnline() is false && GetServiceProcess() != null)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }

            if(GetServiceProcess() == null)
            {
                MainSave.CQLog.Info("WebUI启动", "启动失败，建议手动检查原因");
                return;
            }
            if (ServiceOnline() is false)
            {
                MainSave.CQLog.Info("WebUI启动", "启动超时");
                return;
            }


            MainSave.CQLog.Info("WebUI启动", "启动成功");
        }
    }

    public static void StopService()
    {
        var process = GetServiceProcess();
        if(process != null)
        {
            process.Kill();
            MainSave.CQLog.Info("WebUI停止", "停止成功");
        }
        else
        {
            MainSave.CQLog.Info("WebUI停止", "进程不存在");
        }
    }

    private static Process GetServiceProcess()
    {
        var ls = Process.GetProcessesByName("python");
        string basePath = new DirectoryInfo(AppConfig.WebUIPath).Parent.FullName;
        foreach (var item in ls)
        {
            // 可能会出现32位无法访问64位的情况
            string pyPath = item.MainModule.FileName;
            if (pyPath.Contains(basePath))
            {
               return item;
            }
        }
        return null;
    }

    public static void RestartService()
    {
        StopService();
        StartService();
    }
}