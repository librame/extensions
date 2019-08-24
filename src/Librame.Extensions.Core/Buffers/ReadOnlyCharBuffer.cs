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
    /// 只读字符缓冲区。
    /// </summary>
    public class ReadOnlyCharBuffer : ReadOnlyBuffer<char>, IReadOnlyCharBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyCharBuffer"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的只读存储器。</param>
        public ReadOnlyCharBuffer(ReadOnlyMemory<char> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ReadOnlyCharBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字符数组。</param>
        public ReadOnlyCharBuffer(char[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyCharBuffer"/>。</returns>
        public virtual new IReadOnlyCharBuffer Copy()
        {
            return new ReadOnlyCharBuffer(Memory);
        }

    }
}
