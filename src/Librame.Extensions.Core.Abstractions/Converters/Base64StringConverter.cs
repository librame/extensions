#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Converters
{
    /// <summary>
    /// BASE64 字符串转换器。
    /// </summary>
    public class Base64StringConverter : AbstractAlgorithmConverter
    {
        /// <summary>
        /// 构造一个 <see cref="Base64StringConverter"/>。
        /// </summary>
        public Base64StringConverter()
            : base(f => f.AsBase64String(), r => r.FromBase64String())
        {
        }

    }
}
