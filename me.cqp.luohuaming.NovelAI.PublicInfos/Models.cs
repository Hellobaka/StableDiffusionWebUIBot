using System.Collections.Generic;
using System.Text;
using me.cqp.luohuaming.NovelAI.Sdk.Cqp.EventArgs;

namespace PublicInfos
{
    public interface IOrderModel
    {
        bool ImplementFlag { get; set; }
        string GetOrderStr();
        bool Judge(string destStr);
        FunctionResult Progress(CQGroupMessageEventArgs e);
        FunctionResult Progress(CQPrivateMessageEventArgs e);
    }
    
    public class APIResult 
    {
        public string Result { get; set; }
        public bool IsSuccess { get; set; }
        public bool R18 { get; set; }
    }
}
