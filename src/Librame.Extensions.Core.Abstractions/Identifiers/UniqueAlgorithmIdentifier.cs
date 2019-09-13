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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 唯一算法标识符。
    /// </summary>
    [Serializable]
    public class UniqueAlgorithmIdentifier : AbstractAlgorithmIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="UniqueAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/> 。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>。</param>
        public UniqueAlgorithmIdentifier(Guid guid, IAlgorithmConverter converter)
            : base(guid.ToByteArray(), converter)
        {
            RawGuid = guid;
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定的算法字符串。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>。</param>
        public UniqueAlgorithmIdentifier(string identifier, IAlgorithmConverter converter)
            : base(identifier, converter)
        {
            RawGuid = new Guid(Memory.ToArray());
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="SerializationInfo"/>。</param>
        /// <param name="context">给定的 <see cref="StreamingContext"/>。</param>
        public UniqueAlgorithmIdentifier(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            RawGuid = new Guid(Memory.ToArray());
        }


        /// <summary>
        /// 原始 GUID。
        /// </summary>
        public Guid RawGuid { get; }


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="UniqueAlgorithmIdentifier"/>。</param>
        public static implicit operator string(UniqueAlgorithmIdentifier identifier)
            => identifier?.ToString();


        /// <summary>
        /// 只读空标识符实例（默认使用 <see cref="HexAlgorithmConverter"/> 转换器）。
        /// </summary>
        public static readonly UniqueAlgorithmIdentifier Empty
            = new UniqueAlgorithmIdentifier(Guid.Empty, HexAlgorithmConverter.Default);


        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>（可选；默认使用 <see cref="HexAlgorithmConverter"/> 转换器）。</param>
        /// <returns>返回 <see cref="UniqueAlgorithmIdentifier"/>。</returns>
        public static UniqueAlgorithmIdentifier New(IAlgorithmConverter converter = null)
            => new UniqueAlgorithmIdentifier(Guid.NewGuid(), converter ?? HexAlgorithmConverter.Default);

        /// <summary>
        /// 新建数组实例。
        /// </summary>
        /// <param name="count">给定要生成的实例数量。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>（可选；默认使用 <see cref="HexAlgorithmConverter"/> 转换器）。</param>
        /// <returns>返回 <see cref="UniqueAlgorithmIdentifier"/> 数组。</returns>
        public static UniqueAlgorithmIdentifier[] NewArray(int count, IAlgorithmConverter converter = null)
        {
            var identifiers = new UniqueAlgorithmIdentifier[count];
            for (var i = 0; i < count; i++)
                identifiers[i] = New(converter);

            return identifiers;
        }
    }
}
