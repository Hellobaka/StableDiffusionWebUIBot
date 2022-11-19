using System;
using System.IO;
using System.Linq;
using System.Threading;
using me.cqp.luohuaming.NovelAI.Sdk.Cqp;
using me.cqp.luohuaming.NovelAI.Sdk.Cqp.EventArgs;
using PublicInfos;
using PublicInfos.API;
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
            FunctionResult result = new()
            {
                Result = true,
                SendFlag = true,
            };
            SendText sendText = new()
            {
                SendID = e.FromGroup,
            };
            result.SendObject.Add(sendText);

            if (AppConfig.R18PunishList.Any(x => x == e.FromQQ))
            {
                result.SendFlag = false;
                return result;
            }

            if (AppConfig.Using)
            {
                sendText.MsgToSend.Add(AppConfig.BusyResponse);
                return result;
            }

            if (QuotaHistory.QueryQuota(e.FromGroup, e.FromQQ) <= 0)
            {
                sendText.MsgToSend.Add(AppConfig.NoQuotaResponse);
                return result;
            }
            else
            {
                int quota = AppConfig.MaxPersonQuota - QuotaHistory.HandleQuota(e.FromGroup, e.FromQQ, -1);
                e.FromGroup.SendGroupMessage(AppConfig.CallResponse.Replace("\\n", "\n").Replace("%count%", quota.ToString()));
            }
            try
            {
                string prompt = e.Message.Text.Replace(GetOrderStr(), "").Replace("，", ",");
                if (AppConfig.UseTranslate)
                {
                    string translateResult = Translate.CallTranslate(prompt);
                    if(translateResult == "err")
                    {
                        // sendText.MsgToSend.Add("翻译API无效...");
                        MainSave.CQLog.Info("翻译API无效", "请打开配置文件填写相关字段");
                    }
                    else
                    {
                        prompt = translateResult;
                    }
                }

                var r = PublicInfos.API.NovelAI.Txt2Img(prompt);
                if (r.IsSuccess)
                {
                    string img = r.Result;
                    string filename = DateTime.Now.ToString("G").Replace("/", "").Replace(":", "").Replace(" ", "");
                    string path = Path.Combine(MainSave.ImageDirectory, "NovelAI");
                    Directory.CreateDirectory(path);
                    path = Path.Combine(path, filename + ".png");
                    File.WriteAllBytes(path, Convert.FromBase64String(img));
                    if (AppConfig.R18 == false && r.R18)
                    {
                        MainSave.CQLog.Info("绘制失败", "R18触发");
                        sendText.MsgToSend.Add(AppConfig.R18PunishResponse.Replace("%time%", AppConfig.R18PunishTime.ToString()));
                        AppConfig.R18PunishList.Add(e.FromQQ);
                        new Thread(() =>
                        {
                            Thread.Sleep(AppConfig.R18PunishTime * 1000);
                            AppConfig.R18PunishList.Remove(e.FromQQ);
                        }).Start();
                        return result;
                    }
                    int msgId = e.FromGroup.SendGroupMessage(CQApi.CQCode_Image($"NovelAI\\{filename}.png").ToSendString()).Id;

                    if (AppConfig.R18)
                    {
                        if(r.R18)
                        {
                            // 自动撤回(暂时无法判断图片是否为R18
                        }
                    }
                }
                else
                {
                    QuotaHistory.HandleQuota(e.FromGroup, e.FromQQ, 1);
                    sendText.MsgToSend.Add("绘制失败...");
                }
            }
            catch (Exception exc)
            {
                QuotaHistory.HandleQuota(e.FromGroup, e.FromQQ, 1);
                sendText.MsgToSend.Add($"绘制失败..., {exc.Message}");
                MainSave.CQLog.Info("绘制失败", exc.Message + exc.StackTrace);
            }

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
