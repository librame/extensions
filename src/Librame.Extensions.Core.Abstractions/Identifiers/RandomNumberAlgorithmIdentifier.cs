#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 随机数算法标识符。
    /// </summary>
    [Serializable]
    public class RandomNumberAlgorithmIdentifier : AbstractAlgorithmIdentifier
    {
        private static readonly RandomNumberGenerator _generator
            = RandomNumberGenerator.Create();


        /// <summary>
        /// 构造一个 <see cref="RandomNumberAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="length">给定要生成的字节数组长度。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>（可选）。</param>
        public RandomNumberAlgorithmIdentifier(int length, IAlgorithmConverter converter)
            : base(_generator.GenerateByteArray(length), converter)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="RandomNumberAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定标识符的字符串形式。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>。</param>
        public RandomNumberAlgorithmIdentifier(string identifier, IAlgorithmConverter converter)
            : base(identifier, converter)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="RandomNumberAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="SerializationInfo"/>。</param>
        /// <param name="context">给定的 <see cref="StreamingContext"/>。</param>
        public RandomNumberAlgorithmIdentifier(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <param name="length">给定要生成的字节数组长度。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>（可选；默认使用 <see cref="HexAlgorithmConverter"/> 转换器）。</param>
        /// <returns>返回 <see cref="RandomNumberAlgorithmIdentifier"/>。</returns>
        public static RandomNumberAlgorithmIdentifier New(int length, IAlgorithmConverter converter = null)
            => new RandomNumberAlgorithmIdentifier(length, converter ?? HexAlgorithmConverter.Default);
    }
}
