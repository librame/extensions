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
    /// 只读字符缓冲区。
    /// </summary>
    public class ReadOnlyCharBuffer : ReadOnlyBuffer<char>, IReadOnlyCharBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyCharBuffer"/> 实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的只读存储器。</param>
        public ReadOnlyCharBuffer(ReadOnlyMemory<char> readOnlyMemory)
            : base(readOnlyMemory)
        {
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
        /// 获取只读字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetReadOnlyString()
        {
            return ToString();
        }


        /// <summary>
        /// 转换为字符串形式。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return new string(ReadOnlyMemory.ToArray());
        }

    }
}
