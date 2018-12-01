﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Converters
{
    /// <summary>
    /// 密文算法转换器。
    /// </summary>
    public class PlaintextAlgorithmConverter : EncodingConverter, IPlaintextAlgorithmConverter
    {
        /// <summary>
        /// 构造一个 <see cref="PlaintextAlgorithmConverter"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{PlaintextAlgorithmConverter}"/>。</param>
        public PlaintextAlgorithmConverter(ILogger<PlaintextAlgorithmConverter> logger)
            : base(logger)
        {
        }

    }
}
