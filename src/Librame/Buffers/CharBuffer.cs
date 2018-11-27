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

namespace Librame.Buffers
{
    /// <summary>
    /// 字符缓冲区。
    /// </summary>
    public class CharBuffer : Buffer<char>, ICharBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="CharBuffer"/> 实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{Char}"/>。</param>
        public CharBuffer(ReadOnlyMemory<char> readOnlyMemory)
            : base(readOnlyMemory)
        {
            Memory = readOnlyMemory.ToArray();
        }

        /// <summary>
        /// 构造一个 <see cref="CharBuffer"/> 实例。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{Char}"/>。</param>
        public CharBuffer(Memory<char> memory)
            : base(memory)
        {
            Memory = memory;
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="ICharBuffer"/>。</returns>
        public virtual new ICharBuffer Copy()
        {
            return new CharBuffer(Memory);
        }

        /// <summary>
        /// 创建只读副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyCharBuffer"/>。</returns>
        public virtual new IReadOnlyCharBuffer CopyReadOnly()
        {
            return new ReadOnlyCharBuffer(ReadOnlyMemory);
        }


        /// <summary>
        /// 获取字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetString()
        {
            return ToString();
        }

        /// <summary>
        /// 获取只读字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetReadOnlyString()
        {
            return new string(ReadOnlyMemory.ToArray());
        }


        /// <summary>
        /// 转换为字符串形式。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return new string(Memory.ToArray());
        }

    }
}
