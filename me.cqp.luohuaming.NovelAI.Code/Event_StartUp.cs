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
                MainSave.CQLog.Info("初始化失败", exc.Message);
            }
        }

        private void InitConfig()
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
            AppConfig.R18PunishTime = ConfigHelper.GetConfig("R18PunishTime", 60 * 1000);
            AppConfig.APIBaseUrl = ConfigHelper.GetConfig("APIBaseUrl", "http://127.0.0.1:7860/");
            AppConfig.Height = ConfigHelper.GetConfig("Height", 936);
            AppConfig.Width = ConfigHelper.GetConfig("Width", 936);
            AppConfig.SamplingMethod = ConfigHelper.GetConfig("SamplingMethod", "Euler a");
            AppConfig.BasePrompt = ConfigHelper.GetConfig("SamplingMethod", "{{masterpiece}},{{best quality}}, extremely detailed CG unity 8k wallpaper,");
            AppConfig.NegativePrompt = ConfigHelper.GetConfig("NegativePrompt", "Lowres, bad anatomy, bad hands, text, error, missing, fingers, extra digit, fewer digits, cropped, worst, quality, Low quality, normal quality, jpeg, artifacts, signature, watermark, username, blurry, bad feet");
            AppConfig.TranslateType = ConfigHelper.GetConfig("TranslateType", "Baidu");
            
            OrderConfig.Txt2Img = ConfigHelper.GetConfig("Txt2Img", "#作画");
            OrderConfig.Img2Img = ConfigHelper.GetConfig("Img2Img", "#转图");
        }

        private List<long> ParseConfigList(string s)
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

        private void ParseTimeSpan(string s)
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