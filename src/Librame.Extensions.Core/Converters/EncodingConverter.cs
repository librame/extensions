#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 字符编码转换器。
    /// </summary>
    public class EncodingConverter : IEncodingConverter
    {
        /// <summary>
        /// 构造一个 <see cref="EncodingConverter"/> 实例。
        /// </summary>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        public EncodingConverter(Encoding encoding = null)
        {
            Encoding = encoding ?? Encoding.UTF8;
        }


        /// <summary>
        /// 字符编码。
        /// </summary>
        /// <value>返回 <see cref="System.Text.Encoding"/>。</value>
        public Encoding Encoding { get; set; }


        /// <summary>
        /// 转换为字节缓冲区。
        /// </summary>
        /// <param name="source">给定的明文字符串。</param>
        /// <returns>返回缓冲区。</returns>
        public virtual IByteBuffer ToResult(string source)
        {
            return source.AsEncodingBytes(Encoding).AsByteBuffer();
        }

        /// <summary>
        /// 转换为明文。
        /// </summary>
        /// <param name="result">给定的缓冲区。</param>
        /// <returns>返回字符串。</returns>
        public virtual string ToSource(IByteBuffer result)
        {
            return result.Memory.ToArray().FromEncodingBytes(Encoding);
        }

    }
}
