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
using System.Globalization;

namespace Librame.Extensions.Encryption.KeyGenerators
{
    /// <summary>
    /// 密钥描述符。
    /// </summary>
    public class KeyDescriptor
    {
        private byte[] _keyBytes;
        private string _keyString;


        /// <summary>
        /// 构造一个 <see cref="KeyDescriptor"/>。
        /// </summary>
        /// <param name="keyString">给定的密钥字符串。</param>
        public KeyDescriptor(string keyString)
        {
            _keyBytes = keyString.FromHexString();

            //if (isDecompression)
            //{
            //    _keyBytes = _keyBytes.DeflateDecompress();
            //    //.RtlDecompress() // RTL 解压缩会利用到程序集信息，当版本变化时可能会抛出异常
            //}

            _keyString = keyString;
        }

        /// <summary>
        /// 构造一个 <see cref="KeyDescriptor"/>。
        /// </summary>
        /// <param name="keyBytes">给定的密钥字节数组。</param>
        public KeyDescriptor(byte[] keyBytes)
        {
            _keyBytes = keyBytes.NotEmpty(nameof(keyBytes));

            //if (isCompression)
            //{
            //    _keyBytes = _keyBytes.DeflateCompress();
            //    //.RtlCompress() // RTL 压缩会利用到程序集信息，当版本变化时可能会抛出异常
            //}

            _keyString = _keyBytes.AsHexString();
        }


        /// <summary>
        /// 转换为只读内存字节。
        /// </summary>
        /// <returns>返回 <see cref="ReadOnlyMemory{Byte}"/>。</returns>
        public ReadOnlyMemory<byte> ToReadOnlyMemory()
            => _keyBytes;


        /// <summary>
        /// 转换为长度为 15 的短字符串。
        /// </summary>
        /// <param name="timestamp">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回字符串。</returns>
        public string ToShortString(DateTime timestamp)
            => ToShortString(timestamp.Ticks);

        /// <summary>
        /// 转换为长度为 15 的短字符串。
        /// </summary>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回字符串。</returns>
        public string ToShortString(DateTimeOffset timestamp)
            => ToShortString(timestamp.Ticks);

        private string ToShortString(long ticks)
        {
            var i = 1L;
            foreach (var b in _keyBytes)
                i *= b + 1;

            // Length(15): 8d737ebe809e70e
            return string.Format(CultureInfo.InvariantCulture, "{0:x}", _ = ticks);
        }


        /// <summary>
        /// 尝试转换为全局唯一标识符。
        /// </summary>
        /// <param name="g">输出 <see cref="Guid"/>（如果转换失败，则输出 <see cref="Guid.Empty"/>）。</param>
        /// <returns>返回转换是否成功的布尔值。</returns>
        public bool TryToGuid(out Guid g)
        {
            try
            {
                g = new Guid(_keyBytes);
                return true;
            }
            catch (ArgumentException)
            {
                g = Guid.Empty;
                return false;
            }
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="KeyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(KeyDescriptor other)
            => _keyString == other?._keyString;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is KeyDescriptor other && Equals(other);


        /// <summary>
        /// 定义比较相等静态方法需强制重写此方法。
        /// </summary>
        /// <returns>返回 32 位带符号整数。</returns>
        public override int GetHashCode()
            => _keyString.CompatibleGetHashCode();


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => _keyString;


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="KeyDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="KeyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(KeyDescriptor a, KeyDescriptor b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="KeyDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="KeyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(KeyDescriptor a, KeyDescriptor b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="KeyDescriptor"/>。</param>
        public static implicit operator string(KeyDescriptor descriptor)
            => descriptor?.ToString();


        /// <summary>
        /// 空密钥描述符。
        /// </summary>
        public static readonly KeyDescriptor Empty
            = new KeyDescriptor(Guid.Empty.ToByteArray());


        /// <summary>
        /// 新建密钥描述符。
        /// </summary>
        /// <returns>返回 <see cref="KeyDescriptor"/>。</returns>
        public static KeyDescriptor New()
            => new KeyDescriptor(Guid.NewGuid().ToByteArray());

        /// <summary>
        /// 新建密钥描述符数组。
        /// </summary>
        /// <param name="count">给定要生成的实例数量。</param>
        /// <returns>返回 <see cref="KeyDescriptor"/> 数组。</returns>
        public static KeyDescriptor[] NewArray(int count)
        {
            var descriptors = new KeyDescriptor[count];
            for (var i = 0; i < count; i++)
                descriptors[i] = New();

            return descriptors;
        }

    }
}
