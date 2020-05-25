#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Converters
{
    /// <summary>
    /// 抽象算法转换器。
    /// </summary>
    public abstract class AbstractAlgorithmConverter
        : AbstractConverter<byte[], string>, IAlgorithmConverter
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractAlgorithmConverter"/>。
        /// </summary>
        /// <param name="forward">给定的正向转换工厂方法。</param>
        /// <param name="reverse">给定的反向转换工厂方法。</param>
        protected AbstractAlgorithmConverter(Func<byte[], string> forward,
            Func<string, byte[]> reverse)
            : base(forward, reverse)
        {
        }

    }
}
