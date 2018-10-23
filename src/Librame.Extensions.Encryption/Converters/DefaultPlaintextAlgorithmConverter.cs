#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Encryption
{
    using Converters;

    /// <summary>
    /// 默认密文算法转换器。
    /// </summary>
    public class DefaultPlaintextAlgorithmConverter : DefaultEncodingConverter, IPlaintextAlgorithmConverter
    {
        /// <summary>
        /// 构造一个 <see cref="DefaultPlaintextAlgorithmConverter"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{DefaultPlaintextAlgorithmConverter}"/>。</param>
        public DefaultPlaintextAlgorithmConverter(ILogger<DefaultPlaintextAlgorithmConverter> logger)
            : base(logger)
        {
        }

    }
}
