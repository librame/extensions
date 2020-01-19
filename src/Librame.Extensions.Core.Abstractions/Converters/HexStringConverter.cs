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

namespace Librame.Extensions.Core.Converters
{
    /// <summary>
    /// HEX 字符串转换器。
    /// </summary>
    public class HexStringConverter : AbstractStringConverter
    {
        /// <summary>
        /// 键名。
        /// </summary>
        public static readonly TypeNamedKey Key
            = new TypeNamedKey<ReadOnlyMemory<byte>>(nameof(HexStringConverter));

        /// <summary>
        /// 默认实例。
        /// </summary>
        public static readonly HexStringConverter Default
            = new HexStringConverter();


        /// <summary>
        /// 默认对象。
        /// </summary>
        protected override object DefaultObject
            => ReadOnlyMemory<byte>.Empty;


        /// <summary>
        /// 自定义还原字符串为对象。
        /// </summary>
        /// <param name="destination">给定的目标字符串。</param>
        /// <returns>返回还原后的对象。</returns>
        protected override object CustomConvertFrom(string destination)
        {
            return (ReadOnlyMemory<byte>)destination
                .FromHexString()
                .DeflateDecompress();
                //.RtlDecompress() // RTL 压缩会利用到程序集信息，当版本变化时会抛出异常
        }

        /// <summary>
        /// 自定义转换对象为字符串。
        /// </summary>
        /// <param name="source">给定的来源对象。</param>
        /// <returns>返回转换后的对象。</returns>
        protected override object CustomConvertTo(object source)
        {
            if (source is ReadOnlyMemory<byte> memory)
            {
                return memory.ToArray()
                    //.RtlCompress() // RTL 压缩会利用到程序集信息，当版本变化时会抛出异常
                    .DeflateCompress()
                    .AsHexString();
            }

            return base.CustomConvertTo(source);
        }

    }
}
