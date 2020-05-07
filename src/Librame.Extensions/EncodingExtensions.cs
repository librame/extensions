#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// 字符编码静态扩展。
    /// </summary>
    public static class EncodingExtensions
    {
        /// <summary>
        /// 将字符编码转换为名称的字符串形式。
        /// </summary>
        /// <param name="encoding">给定的字符编码。</param>
        /// <returns>返回代码页名称。</returns>
        public static string AsName(this Encoding encoding)
            => encoding?.WebName;

        /// <summary>
        /// 从名称的字符串形式还原字符编码。
        /// </summary>
        /// <param name="name">首选编码的代码页名称。</param>
        /// <returns>返回与指定代码页关联的编码。</returns>
        public static Encoding FromEncodingName(this string name)
            => Encoding.GetEncoding(name);


        /// <summary>
        /// 转换为字符编码的字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionContext.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string AsEncodingString(this byte[] bytes, Encoding encoding = null)
            => (encoding ?? ExtensionSettings.Current.DefaultEncoding).GetString(bytes);

        /// <summary>
        /// 还原为字符编码的字节数组。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionContext.DefaultEncoding"/>）。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static byte[] FromEncodingString(this string str, Encoding encoding = null)
            => (encoding ?? ExtensionSettings.Current.DefaultEncoding).GetBytes(str);


        #region Encoding Base and Hex

        /// <summary>
        /// 转换为经过字符编码的 BASE32 字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionContext.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingBase32String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).AsBase32String();

        /// <summary>
        /// 还原为经过字符编码的 BASE32 字符串。
        /// </summary>
        /// <param name="base32String">给定的 BASE32 字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionContext.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingBase32String(this string base32String, Encoding encoding = null)
            => base32String.FromBase32String().AsEncodingString(encoding);


        /// <summary>
        /// 转换为经过字符编码的 BASE64 字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionContext.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingBase64String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).AsBase64String();

        /// <summary>
        /// 还原为经过字符编码的 BASE64 字符串。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionContext.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingBase64String(this string base64String, Encoding encoding = null)
            => base64String.FromBase64String().AsEncodingString(encoding);


        /// <summary>
        /// 转换为经过字符编码的 16 进制字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionContext.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingHexString(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).AsHexString();

        /// <summary>
        /// 还原为经过字符编码的 16 进制字符串。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionContext.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingHexString(this string hexString, Encoding encoding = null)
            => hexString.FromHexString().AsEncodingString(encoding);

        #endregion

    }
}
