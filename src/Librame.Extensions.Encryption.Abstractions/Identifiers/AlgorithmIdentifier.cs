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

namespace Librame.Extensions.Encryption
{
    /// <summary>
    /// 算法标识符。
    /// </summary>
    public class AlgorithmIdentifier : IEquatable<AlgorithmIdentifier>
    {
        /// <summary>
        /// 构造一个 <see cref="AlgorithmIdentifier"/> 实例。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/>。</param>
        public AlgorithmIdentifier(Guid guid)
        {
            Memory = guid.ToByteArray();
        }

        /// <summary>
        /// 构造一个 <see cref="AlgorithmIdentifier"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// bytes is null or empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// bytes.Length is not equals 16.
        /// </exception>
        /// <param name="bytes">给定的字节数组。</param>
        public AlgorithmIdentifier(byte[] bytes)
        {
            bytes.NotNullOrEmpty(nameof(bytes));

            if (bytes.Length != 16)
                throw new ArgumentOutOfRangeException(nameof(bytes));

            Memory = bytes;
        }


        /// <summary>
        /// 字节存储器。
        /// </summary>
        public Memory<byte> Memory { get; }


        /// <summary>
        /// 转换为全局唯一标识符。
        /// </summary>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public Guid ToGuid()
            => new Guid(Memory.ToArray());


        /// <summary>
        /// 转换为只读内存。
        /// </summary>
        /// <returns>返回 <see cref="ReadOnlyMemory{T}"/>。</returns>
        public ReadOnlyMemory<byte> ToReadOnlyMemory()
            => Memory;


        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Memory.ToArray().AsHexString();


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="AlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(AlgorithmIdentifier other)
            => ToString() == other?.ToString();


        #region Converts

        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="AlgorithmIdentifier"/>。</param>
        public static implicit operator string(AlgorithmIdentifier identifier)
            => identifier?.ToString();

        /// <summary>
        /// 显式转换为算法标识符。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/>。</param>
        public static explicit operator AlgorithmIdentifier(Guid guid)
            => new AlgorithmIdentifier(guid);

        /// <summary>
        /// 显式转换为算法标识符。
        /// </summary>
        /// <param name="identifier">给定的标识符字符串（通常为 16 进制格式）。</param>
        public static explicit operator AlgorithmIdentifier(string identifier)
            => new AlgorithmIdentifier(identifier.FromHexString());

        #endregion


        #region Instances

        /// <summary>
        /// 空 GUID 只读实例。
        /// </summary>
        /// <value>
        /// 返回 <see cref="AlgorithmIdentifier"/>。
        /// </value>
        public readonly static AlgorithmIdentifier Empty
            = new AlgorithmIdentifier(Guid.Empty);

        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <returns>返回 <see cref="AlgorithmIdentifier"/>。</returns>
        public static AlgorithmIdentifier New()
            => new AlgorithmIdentifier(Guid.NewGuid());

        #endregion

    }
}
