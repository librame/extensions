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

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 只读内存 BASE64 字符串序列化器。
    /// </summary>
    public class ReadOnlyMemoryBase64StringSerializer : IObjectStringSerializer
    {
        internal const string DefaultName
            = nameof(ReadOnlyMemoryBase64StringSerializer);


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name
            => DefaultName;


        /// <summary>
        /// 反序列化为原始对象。
        /// </summary>
        /// <param name="target">给定的字符串。</param>
        /// <returns>返回字符编码。</returns>
        public object Deserialize(string target)
        {
            if (target.IsEmpty())
                return ReadOnlyMemory<byte>.Empty;

            return (ReadOnlyMemory<byte>)target
                .FromBase64String()
                .DeflateDecompress();
            //.RtlDecompress() // RTL 压缩会利用到程序集信息，当版本变化时会抛出异常
        }

        /// <summary>
        /// 序列化为字符串。
        /// </summary>
        /// <param name="source">给定的字符编码。</param>
        /// <returns>返回字符串。</returns>
        public string Serialize(object source)
        {
            if (source is ReadOnlyMemory<byte> readOnlyMemory && !readOnlyMemory.IsEmpty)
            {
                return readOnlyMemory
                    .ToArray()
                    //.RtlCompress() // RTL 压缩会利用到程序集信息，当版本变化时会抛出异常
                    .DeflateCompress()
                    .AsBase64String();
            }

            return string.Empty;
        }
    }
}
