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

namespace Librame.Extensions.Core.Identifiers
{
    using Serializers;

    /// <summary>
    /// 唯一算法标识符。
    /// </summary>
    public class UniqueAlgorithmIdentifier : AbstractAlgorithmIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="UniqueAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>。</param>
        public UniqueAlgorithmIdentifier(SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory)
            : base(readOnlyMemory)
        {
            RawGuid = new Guid(readOnlyMemory?.Source.ToArray());
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="g">给定的 <see cref="Guid"/>。</param>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>。</param>
        public UniqueAlgorithmIdentifier(Guid g, SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory)
            : base(readOnlyMemory?.ChangeSource(g.ToByteArray()))
        {
            RawGuid = g;
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定的标识符字符串。</param>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>。</param>
        public UniqueAlgorithmIdentifier(string identifier, SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory)
            : base(readOnlyMemory?.ChangeValue(identifier))
        {
            RawGuid = new Guid(readOnlyMemory.Source.ToArray());
        }


        /// <summary>
        /// 原始 GUID。
        /// </summary>
        public Guid RawGuid { get; }


        /// <summary>
        /// 空实例（默认使用 HEX 序列化字节数组）。
        /// </summary>
        public static readonly UniqueAlgorithmIdentifier Empty
            = new UniqueAlgorithmIdentifier(SerializableHelper.CreateReadOnlyMemoryHex(Guid.Empty.ToByteArray()));


        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>（可选；默认使用 HEX 序列化字节数组）。</param>
        /// <returns>返回 <see cref="UniqueAlgorithmIdentifier"/>。</returns>
        public static UniqueAlgorithmIdentifier New(SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory = null)
        {
            if (readOnlyMemory.IsNull())
                return new UniqueAlgorithmIdentifier(SerializableHelper.CreateReadOnlyMemoryHex(Guid.NewGuid().ToByteArray()));

            return new UniqueAlgorithmIdentifier(Guid.NewGuid(), readOnlyMemory);
        }

        /// <summary>
        /// 新建实例数组。
        /// </summary>
        /// <param name="count">给定要生成的实例数量。</param>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>（可选；默认使用 HEX 序列化字节数组）。</param>
        /// <returns>返回 <see cref="UniqueAlgorithmIdentifier"/> 数组。</returns>
        public static UniqueAlgorithmIdentifier[] NewArray(int count, SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory = null)
        {
            var identifiers = new UniqueAlgorithmIdentifier[count];
            for (var i = 0; i < count; i++)
                identifiers[i] = New(readOnlyMemory);

            return identifiers;
        }
    }
}
