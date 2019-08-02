#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Options;
using System.Text;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 字符编码转换器。
    /// </summary>
    public class EncodingConverter : IEncodingConverter
    {
        /// <summary>
        /// 构造一个 <see cref="EncodingConverter"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        public EncodingConverter(IOptions<CoreBuilderOptions> options)
            : this(options?.Value?.Encoding)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="EncodingConverter"/>。
        /// </summary>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>。</param>
        protected internal EncodingConverter(Encoding encoding)
        {
            Encoding = encoding.NotNull(nameof(encoding));
        }


        /// <summary>
        /// 字符编码（默认）。
        /// </summary>
        public Encoding Encoding { get; set; }


        /// <summary>
        /// 转换为明文。
        /// </summary>
        /// <param name="output">给定的缓冲区。</param>
        /// <returns>返回字符串。</returns>
        public virtual string From(IByteBuffer output)
        {
            return output.Memory.ToArray().FromEncodingBytes(Encoding);
        }

        /// <summary>
        /// 转换为字节缓冲区。
        /// </summary>
        /// <param name="input">给定的明文字符串。</param>
        /// <returns>返回缓冲区。</returns>
        public virtual IByteBuffer To(string input)
        {
            return input.AsEncodingBytes(Encoding).AsByteBuffer();
        }

    }
}
