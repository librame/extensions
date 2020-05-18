#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 短信服务描述符。
    /// </summary>
    public class SmsDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="SmsDescriptor"/>。
        /// </summary>
        /// <param name="mobile">给定的手机。</param>
        /// <param name="text">给定的文本。</param>
        public SmsDescriptor(string mobile, string text)
        {
            Mobile = mobile.NotEmpty(nameof(mobile));
            Text = text.NotEmpty(nameof(text));
        }


        /// <summary>
        /// 手机。
        /// </summary>
        public string Mobile { get; }

        /// <summary>
        /// 文本。
        /// </summary>
        public string Text { get; }
    }
}
