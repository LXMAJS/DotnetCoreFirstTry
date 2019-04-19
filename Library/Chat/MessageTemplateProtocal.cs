using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Chat
{
    /// <summary>
    /// 简单通讯协议
    /// </summary>
    public class MessageTemplateProtocal
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string MessageType { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// 公共聊天接收者
        /// </summary>
        public static string PublicReceiverId = "public";

        /// <summary>
        /// 消息类型
        /// </summary>
        public enum EMessateType
        {
            Text,
            Picture
        }
    }
}
