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
            var r = NovelAI.Txt2Img(AppConfig.BasePrompt + "white hair，blue eyes，dog ears，standing split，full body，loli".Replace("，", ","));
            File.WriteAllBytes("1.png", Convert.FromBase64String(r.Result));
        }
    }
}