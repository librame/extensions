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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 字符缓冲区。
    /// </summary>
    public class CharBuffer : Buffer<char>, ICharBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="CharBuffer"/> 实例。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{Char}"/>。</param>
        public CharBuffer(Memory<char> memory)
            : base(memory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="CharBuffer"/> 实例。
        /// </summary>
        /// <param name="array">给定的字符数组。</param>
        public CharBuffer(char[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="ICharBuffer"/>。</returns>
        public virtual new ICharBuffer Copy()
        {
            return new CharBuffer(Memory);
        }

    }
}
