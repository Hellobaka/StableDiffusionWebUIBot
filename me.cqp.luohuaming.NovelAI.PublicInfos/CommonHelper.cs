using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using me.cqp.luohuaming.NovelAI.Sdk.Cqp.Model;
using me.cqp.luohuaming.NovelAI.Tool.IniConfig;

namespace PublicInfos
{
    public static class CommonHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        /// <summary>
        /// 获取CQ码中的图片网址
        /// </summary>
        /// <param name="imageCQCode">需要解析的图片CQ码</param>
        /// <returns></returns>
        public static string GetImageURL(string imageCQCode)
        {
            string path = MainSave.ImageDirectory + CQCode.Parse(imageCQCode)[0].Items["file"] + ".cqimg";
            IniConfig image = new IniConfig(path);
            image.Load();
            return image.Object["image"]["url"].ToString();
        }
        public static string GetAppImageDirectory()
        {
            var ImageDirectory = Path.Combine(Environment.CurrentDirectory, "data", "image\\");
            return ImageDirectory;
        }

        public static string Join<T>(this List<T> s, string pattern)
        {
            string r = "";
            for (int i = 0; i < s.Count; i++)
            {
                r += s[i].ToString();
                if (i != s.Count - 1)
                {
                    r += pattern;
                }
            }

            return r;
        }
        public static string MD5Encrypt(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using var md5 = MD5.Create();
            byte[] hashmessage = md5.ComputeHash(messageBytes);
            return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
        } }
}
