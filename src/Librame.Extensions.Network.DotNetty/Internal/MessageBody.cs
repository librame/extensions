#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.DotNetty.Internal
{
    internal class MessageBody
    {
        public MessageBody(string message)
        {
            Message = message;
        }

        
        public string Message { get; }

        
        public virtual string ToJsonFormat()
        {
            return "{" + $"\"{nameof(MessageBody)}\" :" + "{" + $"\"{nameof(Message)}\"" + " :\"" + Message + "\"}" + "}";
        }

        
        public override string ToString()
        {
            return Message;
        }

    }
}
