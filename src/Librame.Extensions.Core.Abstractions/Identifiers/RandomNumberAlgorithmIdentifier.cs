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
using System.Security.Cryptography;

namespace Librame.Extensions.Core.Identifiers
{
    using Serializers;

    /// <summary>
    /// 随机数算法标识符。
    /// </summary>
    public class RandomNumberAlgorithmIdentifier : AbstractAlgorithmIdentifier
    {
        private static readonly RandomNumberGenerator _generator
            = RandomNumberGenerator.Create();


        /// <summary>
        /// 构造一个 <see cref="RandomNumberAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>。</param>
        public RandomNumberAlgorithmIdentifier(SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="RandomNumberAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="length">给定要生成的字节数组长度。</param>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>。</param>
        public RandomNumberAlgorithmIdentifier(int length, SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory)
            : base(readOnlyMemory?.ChangeSource(_generator.GenerateByteArray(length)))
        {
        }

        /// <summary>
        /// 构造一个 <see cref="RandomNumberAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定标识符的字符串形式。</param>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>。</param>
        public RandomNumberAlgorithmIdentifier(string identifier, SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory)
            : base(readOnlyMemory?.ChangeValue(identifier))
        {
        }


        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <param name="length">给定要生成的字节数组长度。</param>
        /// <param name="readOnlyMemory">给定的序列化字节数组（可选；默认使用 HEX 序列化字节数组）。</param>
        /// <returns>返回 <see cref="RandomNumberAlgorithmIdentifier"/>。</returns>
        public static RandomNumberAlgorithmIdentifier New(int length, SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory = null)
        {
            if (readOnlyMemory.IsNull())
                return new RandomNumberAlgorithmIdentifier(SerializableHelper.CreateReadOnlyMemoryHex(_generator.GenerateByteArray(length)));

            return new RandomNumberAlgorithmIdentifier(length, readOnlyMemory);
        }
    }
}
