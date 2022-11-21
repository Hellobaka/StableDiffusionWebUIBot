using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.NovelAI.Sdk.Cqp.EventArgs;
using PublicInfos;
using PublicInfos.Config;

namespace me.cqp.luohuaming.NovelAI.Code
{
    public class Event_GroupMessage
    {
        public static FunctionResult GroupMessage(CQGroupMessageEventArgs e)
        {
            FunctionResult result = new FunctionResult()
            {
                SendFlag = false
            };
            try
            {
                if(AppConfig.WhiteMode)
                {
                    if(!AppConfig.WhiteList.Contains(e.FromGroup))
                    {
                        return result;
                    }
                }
                else
                {
                    if(AppConfig.BlackList.Contains(e.FromGroup))
                    {
                        return result;
                    }
                }
                foreach (var item in MainSave.Instances.Where(item => item.Judge(e.Message.Text)))
                {
                    return item.Progress(e);
                }
                return result;
            }
            catch (Exception exc)
            {
                MainSave.CQLog.Info("异常抛出", exc.Message + exc.StackTrace);
                return result;
            }
        }
    }
}
