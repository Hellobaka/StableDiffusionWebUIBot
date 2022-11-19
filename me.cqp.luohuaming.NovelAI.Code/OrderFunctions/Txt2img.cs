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
    public class Txt2Img : IOrderModel
    {
        public bool ImplementFlag { get; set; } = true;
        
        public string GetOrderStr() => OrderConfig.Txt2Img;

        public bool Judge(string destStr) => destStr.Replace("＃", "#").StartsWith(GetOrderStr());

        public FunctionResult Progress(CQGroupMessageEventArgs e)
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
            
            sendText.MsgToSend.Add("这里输入需要发送的文本");
            result.SendObject.Add(sendText);
            return result;
        }

        public FunctionResult Progress(CQPrivateMessageEventArgs e)
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
