﻿using me.cqp.luohuaming.NovelAI.Tool.Http;
using Newtonsoft.Json.Linq;
using PublicInfos.Config;
using System;

namespace PublicInfos.API;

public class NovelAI
{
    public static APIResult Txt2Img(string prompt)
    {
        return Txt2Img(prompt, AppConfig.NegativePrompt, AppConfig.Steps, AppConfig.Height, AppConfig.Width
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
  ""mask"": ""string"",
  ""mask_blur"": 4,
  ""inpainting_fill"": 0,
  ""inpaint_full_res"": true,
  ""inpaint_full_res_padding"": 0,
  ""inpainting_mask_invert"": 0,
  ""prompt"": ""%prompt%"",
  ""styles"": [
    """"
  ],
  ""seed"": -1,
  ""subseed"": -1,
  ""subseed_strength"": 0,
  ""seed_resize_from_h"": -1,
  ""seed_resize_from_w"": -1,
  ""batch_size"": 1,
  ""n_iter"": 1,
  ""steps"": %steps%,
  ""cfg_scale"": 7,
  ""width"": %width%,
  ""height"": %height%,
  ""restore_faces"": %restore_faces%,
  ""tiling"": false,
  ""negative_prompt"": ""%negative_prompt%"",
  ""eta"": 0,
  ""s_churn"": 0,
  ""s_tmax"": 0,
  ""s_tmin"": 0,
  ""s_noise"": 1,
  ""override_settings"": {},
  ""sampler_index"": ""%engine%"",
  ""include_init_images"": false
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
}