using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.NovelAI.Code.OrderFunctions;
using me.cqp.luohuaming.NovelAI.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.NovelAI.Sdk.Cqp.Interface;
using PublicInfos;
using System.Reflection;
using PublicInfos.Config;


namespace me.cqp.luohuaming.NovelAI.Code
{
    public class Event_StartUp : ICQStartup
    {
        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            try
            {
                MainSave.AppDirectory = e.CQApi.AppDirectory;
                MainSave.CQApi = e.CQApi;
                MainSave.CQLog = e.CQLog;
                MainSave.ImageDirectory = CommonHelper.GetAppImageDirectory();
                ConfigHelper.ConfigFileName = Path.Combine(MainSave.AppDirectory, "Config.json");
                //这里写处理逻辑
                //MainSave.Instances.Add(new ExampleFunction());//这里需要将指令实例化填在这里
                foreach (var item in Assembly.GetAssembly(typeof(Event_GroupMessage)).GetTypes())
                {
                    if (item.IsInterface)
                        continue;
                    foreach (var instance in item.GetInterfaces())
                    {
                        if (instance == typeof(IOrderModel))
                        {
                            IOrderModel obj = (IOrderModel) Activator.CreateInstance(item);
                            if (obj.ImplementFlag == false)
                                break;
                            MainSave.Instances.Add(obj);
                        }
                    }
                }
                InitConfig();
                ParseTimeSpan(AppConfig.EnableTimespan);
            }
            catch (Exception exc)
            {
                MainSave.CQLog.Info("初始化失败", exc.Message + exc.StackTrace);
            }
        }

        public void InitConfig()
        {
            AppConfig.R18 = ConfigHelper.GetConfig("R18", false);
            AppConfig.UseTranslate = ConfigHelper.GetConfig("UseTranslate", false);
            AppConfig.WhiteMode = ConfigHelper.GetConfig("WhiteMode", false);
            AppConfig.Admin = ParseConfigList(ConfigHelper.GetConfig("Admin", ""));
            AppConfig.WhiteList = ParseConfigList(ConfigHelper.GetConfig("WhiteList", ""));
            AppConfig.BlackList = ParseConfigList(ConfigHelper.GetConfig("BlackList", ""));
            AppConfig.EnableTimespan = ConfigHelper.GetConfig("EnableTimespan", "");
            AppConfig.MaxPersonQuota = ConfigHelper.GetConfig("MaxPersonQuota", 10);
            AppConfig.MaxGroupQuota = ConfigHelper.GetConfig("MaxGroupQuota", 50);
            AppConfig.R18PunishTime = ConfigHelper.GetConfig("R18PunishTime", 300);
            AppConfig.APIBaseUrl = ConfigHelper.GetConfig("APIBaseUrl", "http://127.0.0.1:7860/");
            AppConfig.Height = ConfigHelper.GetConfig("Height", 936);
            AppConfig.Width = ConfigHelper.GetConfig("Width", 936);
            AppConfig.Steps = ConfigHelper.GetConfig("Steps", 30);
            AppConfig.Timeout = ConfigHelper.GetConfig("Timeout", 300);
            AppConfig.RestoreFaces = ConfigHelper.GetConfig("RestoreFaces", true);
            AppConfig.SamplingMethod = ConfigHelper.GetConfig("SamplingMethod", "Euler a");
            AppConfig.BasePrompt = ConfigHelper.GetConfig("BasePrompt", "{{masterpiece}},{{best quality}}, extremely detailed CG unity 8k wallpaper,");
            AppConfig.NegativePrompt = ConfigHelper.GetConfig("NegativePrompt", "Lowres, bad anatomy, bad hands, text, error, missing, fingers, extra digit, fewer digits, cropped, worst, quality, Low quality, normal quality, jpeg, artifacts, signature, watermark, username, blurry, bad feet");
            AppConfig.TranslateType = ConfigHelper.GetConfig("TranslateType", "Baidu");
            AppConfig.CallResponse = ConfigHelper.GetConfig("CallResponse", "今日可用次数:%count%\\n开始作画，大概需要1分钟...");
            AppConfig.BusyResponse = ConfigHelper.GetConfig("BusyResponse", "当前有任务正在进行，请等待前一任务完成...");
            AppConfig.NoQuotaResponse = ConfigHelper.GetConfig("NoQuotaResponse", "调用额度达到上限");
            AppConfig.R18PunishResponse = ConfigHelper.GetConfig("R18PunishResponse", "触发R18审计配置，禁用%time%秒");
            AppConfig.MaxGroupResponse = ConfigHelper.GetConfig("MaxGroupResponse", "群额度达到上限");
            AppConfig.Baidu_AppId = ConfigHelper.GetConfig("Baidu_AppId", "");
            AppConfig.Baidu_Key = ConfigHelper.GetConfig("Baidu_Key", "");
            
            OrderConfig.Txt2Img = ConfigHelper.GetConfig("Txt2Img", "#作画");
            OrderConfig.Img2Img = ConfigHelper.GetConfig("Img2Img", "#转图");

            QuotaHistory.CreateGroupQuotaDict();
        }

        public List<long> ParseConfigList(string s)
        {
            List<long> result = new();
            foreach (string item in s.Split('|'))
            {
                if (long.TryParse(item, out long admin))
                {
                    result.Add(admin);
                }
            }

            return result;
        }

        public void ParseTimeSpan(string s)
        {
            string[] args = s.Split('-');
            if (args.Length != 2)
            {
                return;
            }

            string[] time = args[0].Split(':');
            if (time.Length != 2)
            {
                return;
            }

            int start, end;
            start = Convert.ToInt32(time[0]);
            end = Convert.ToInt32(time[1]);
            AppConfig.EnableTime = new TimeSpan(start, end, 0);

            time = args[1].Split(':');
            if (time.Length != 2)
            {
                return;
            }

            start = Convert.ToInt32(time[0]);
            end = Convert.ToInt32(time[1]);
            AppConfig.DisableTime = new TimeSpan(start, end, 0);
        }
    }
}