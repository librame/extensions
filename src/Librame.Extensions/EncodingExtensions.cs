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

namespace Librame.Extensions
{
    /// <summary>
    /// 字符编码静态扩展。
    /// </summary>
    public static class EncodingExtensions
    {
        /// <summary>
        /// 将代码页名称转换为关联的字符编码。
        /// </summary>
        /// <param name="name">首选编码的代码页名称。</param>
        /// <returns>返回与指定代码页关联的编码。</returns>
        public static Encoding AsEncoding(this string name)
        {
            return Encoding.GetEncoding(name);
        }
        /// <summary>
        /// 将字符编码转换为可配置的代码页名称。
        /// </summary>
        /// <param name="encoding">给定的字符编码。</param>
        /// <returns>返回代码页名称。</returns>
        public static string AsName(this Encoding encoding)
        {
            // WebName or BodyName
            return encoding?.WebName;
        }

        /// <summary>
        /// 转换为字符编码字节数组。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsEncodingBytes(this string str, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetBytes(str);
        }
        /// <summary>
        /// 还原字符编码字节数组。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingBytes(this byte[] bytes, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }

        /// <summary>
        /// 转换为经过字符编码的 16 进制字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingHexString(this string str, Encoding encoding = null)
        {
            return str.AsEncodingBytes(encoding).AsHexString();
        }
        /// <summary>
        /// 还原经过字符编码的 16 进制字符串。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingHexString(this string hexString, Encoding encoding = null)
        {
            return hexString.FromHexString().FromEncodingBytes(encoding);
        }

        /// <summary>
        /// 转换为经过字符编码的 BASE64 字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingBase64String(this string str, Encoding encoding = null)
        {
            return str.AsEncodingBytes(encoding).AsBase64String();
        }
        /// <summary>
        /// 还原经过字符编码的 BASE64 字符串。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingBase64String(this string base64String, Encoding encoding = null)
        {
            return base64String.FromBase64String().FromEncodingBytes(encoding);
        }

        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 还原 BASE64 字符串。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromBase64String(this string base64String)
        {
            return Convert.FromBase64String(base64String);
        }

        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
        /// <summary>
        /// 还原 16 进制字符串。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromHexString(this string hexString)
        {
            if (!hexString.Length.IsMultiples(2))
                throw new ArgumentException("Hex length must be in multiples of 2.");

            //var memory = hexString.AsMemory();

            var length = hexString.Length / 2;
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16); // memory.Slice(i * 2, 2)

            return buffer;
        }

    }
}
