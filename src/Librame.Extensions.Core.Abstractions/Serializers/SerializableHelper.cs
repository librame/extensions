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
using System.Text;

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 可序列化助手。
    /// </summary>
    public static class SerializableHelper
    {
        /// <summary>
        /// 创建只读内存字节的 BASE32 格式可序列化对象。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <returns>返回 <see cref="SerializableObject{ReadOnlyMemory}"/>。</returns>
        public static SerializableObject<ReadOnlyMemory<byte>> CreateReadOnlyMemoryBase32(ReadOnlyMemory<byte> readOnlyMemory)
        {
            if (readOnlyMemory.IsEmpty)
                return new SerializableObject<ReadOnlyMemory<byte>>(string.Empty, ReadOnlyMemoryBase32StringSerializer.DefaultName);

            return new SerializableObject<ReadOnlyMemory<byte>>(readOnlyMemory, ReadOnlyMemoryBase32StringSerializer.DefaultName);
        }

        /// <summary>
        /// 创建只读内存字节的 BASE64 格式可序列化对象。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <returns>返回 <see cref="SerializableObject{ReadOnlyMemory}"/>。</returns>
        public static SerializableObject<ReadOnlyMemory<byte>> CreateReadOnlyMemoryBase64(ReadOnlyMemory<byte> readOnlyMemory)
        {
            if (readOnlyMemory.IsEmpty)
                return new SerializableObject<ReadOnlyMemory<byte>>(string.Empty, ReadOnlyMemoryBase64StringSerializer.DefaultName);

            return new SerializableObject<ReadOnlyMemory<byte>>(readOnlyMemory, ReadOnlyMemoryBase64StringSerializer.DefaultName);
        }

        /// <summary>
        /// 创建只读内存字节的 HEX 格式可序列化对象。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <returns>返回 <see cref="SerializableObject{ReadOnlyMemory}"/>。</returns>
        public static SerializableObject<ReadOnlyMemory<byte>> CreateReadOnlyMemoryHex(ReadOnlyMemory<byte> readOnlyMemory)
        {
            if (readOnlyMemory.IsEmpty)
                return new SerializableObject<ReadOnlyMemory<byte>>(string.Empty, ReadOnlyMemoryHexStringSerializer.DefaultName);

            return new SerializableObject<ReadOnlyMemory<byte>>(readOnlyMemory, ReadOnlyMemoryHexStringSerializer.DefaultName);
        }


        /// <summary>
        /// 创建字符编码可序列化对象。
        /// </summary>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回 <see cref="SerializableObject{Encoding}"/>。</returns>
        public static SerializableObject<Encoding> CreateEncoding(Encoding encoding = null)
            => new SerializableObject<Encoding>(encoding ?? Encoding.UTF8, EncodingStringSerializer.DefaultName);


        /// <summary>
        /// 创建类型可序列化对象。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回 <see cref="SerializableObject{Type}"/>。</returns>
        public static SerializableObject<Type> CreateType<T>()
            => CreateType(typeof(T));

        /// <summary>
        /// 创建类型可序列化对象。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回 <see cref="SerializableObject{Type}"/>。</returns>
        public static SerializableObject<Type> CreateType(Type type)
            => new SerializableObject<Type>(type, TypeStringSerializer.DefaultName);
    }
}
