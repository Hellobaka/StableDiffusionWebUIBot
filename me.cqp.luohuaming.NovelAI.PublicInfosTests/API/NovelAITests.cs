using me.cqp.luohuaming.NovelAI.Code;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicInfos.API;
using PublicInfos.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicInfos.API.Tests
{
    [TestClass()]
    public class NovelAITests
    {
        [TestMethod()]
        public void Txt2ImgTest()
        {
            MainSave.AppDirectory = @"E:\酷Q机器人插件开发\AMN\data\app\me.cqp.luohuaming.NovelAI";
            ConfigHelper.ConfigFileName = Path.Combine(MainSave.AppDirectory, "Config.json");
            Event_StartUp s = new Event_StartUp();
            s.InitConfig();
            s.ParseTimeSpan(AppConfig.EnableTimespan);
            Console.WriteLine(Translate.BaiduTextTranslate("金发天使 头上有光环"));
            //var r = NovelAI.Txt2Img(AppConfig.BasePrompt + "blonde hair, loli, red eyes, twintail, angel, messy_hair, masterpiece, halo");
            //File.WriteAllBytes("1.png", Convert.FromBase64String(r.Result));
        }
    }
}