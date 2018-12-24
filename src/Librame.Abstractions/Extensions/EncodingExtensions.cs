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
            return Encoding.GetEncoding(name.NotEmpty(nameof(name)));
        }

        /// <summary>
        /// 将字符编码转换为可配置的代码页名称。
        /// </summary>
        /// <param name="encoding">给定的字符编码。</param>
        /// <returns>返回代码页名称。</returns>
        public static string AsName(this Encoding encoding)
        {
            // WebName or BodyName
            return encoding.NotDefault(nameof(encoding)).WebName;
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
        /// 转换为字符编码字节数组。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsEncodingBytes(this string str, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 还原字符编码字节数组。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingBytes(this byte[] bytes, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            return encoding.GetString(bytes);
        }

    }
}
