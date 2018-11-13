#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 消息主体。
    /// </summary>
    public class MessageBody
    {
        /// <summary>
        /// 构造一个 <see cref="MessageBody"/> 实例。
        /// </summary>
        /// <param name="message">给定的消息。</param>
        public MessageBody(string message)
        {
            Message = message;
        }


        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; }


        /// <summary>
        /// 转换为 JSON 格式。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string ToJsonFormat()
        {
            return "{" + $"\"{nameof(MessageBody)}\" :" + "{" + $"\"{nameof(Message)}\"" + " :\"" + Message + "\"}" + "}";
        }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return Message;
        }

    }
}
