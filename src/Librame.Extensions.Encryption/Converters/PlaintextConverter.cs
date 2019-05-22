#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 密文转换器。
    /// </summary>
    public class PlaintextConverter : EncodingConverter, IPlaintextConverter
    {
        /// <summary>
        /// 构造一个 <see cref="PlaintextConverter"/> 实例。
        /// </summary>
        public PlaintextConverter()
            : base()
        {
        }

    }
}
