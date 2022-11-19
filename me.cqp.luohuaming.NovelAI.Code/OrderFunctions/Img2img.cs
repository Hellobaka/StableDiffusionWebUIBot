using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using me.cqp.luohuaming.NovelAI.Sdk.Cqp.EventArgs;
using PublicInfos;
using PublicInfos.Config;

namespace me.cqp.luohuaming.NovelAI.Code.OrderFunctions
{
    public class Img2Img : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;
        
        public string GetOrderStr() => OrderConfig.Img2Img;

        public bool Judge(string destStr) => destStr.Replace("＃", "#").StartsWith(GetOrderStr());

        public FunctionResult Progress(CQGroupMessageEventArgs e)//群聊处理
        {
            FunctionResult result = new FunctionResult
            {
                Result = true,
                SendFlag = true,
            };
            SendText sendText = new SendText
            {
                SendID = e.FromGroup,
            };
            result.SendObject.Add(sendText);
            if (AppConfig.Using)
            {
                sendText.MsgToSend.Add("当前有任务正在进行，请等待前一任务完成...");
                return result;
            }

            sendText.MsgToSend.Add("这里输入需要发送的文本");
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)//私聊处理
        {
            FunctionResult result = new FunctionResult
            {
                Result = false,
                SendFlag = false,
            };
            SendText sendText = new SendText
            {
                SendID = e.FromQQ,
            };

            sendText.MsgToSend.Add("这里输入需要发送的文本");
            result.SendObject.Add(sendText);
            return result;
        }
    }
}
