using System;
using System.Collections.Generic;

namespace PublicInfos.Config;

public static class AppConfig
{
    public static bool R18 { get; set; }

    public static bool UseTranslate { get; set; }
    
    public static string TranslateType { get; set; }

    public static List<long> Admin { get; set; }

    public static bool WhiteMode { get; set; }

    public static List<long> WhiteList { get; set; }

    public static List<long> BlackList { get; set; }

    public static string EnableTimespan { get; set; }

    public static TimeSpan EnableTime { get; set; } = new(0,0,0);
    
    public static TimeSpan DisableTime { get; set; } = new(0,0,0);
    
    public static int MaxPersonQuota { get; set; }
    
    public static int MaxGroupQuota { get; set; } 

    public static int R18PunishTime { get; set; }

    public static string APIBaseUrl { get; set; }
    
    public static int Height { get; set; }
   
    public static int Width { get; set; }
    
    public static string SamplingMethod { get; set; }
    
    public static string NegativePrompt { get; set; }
    
    public static string BasePrompt { get; set; }
}