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

namespace Librame.Extensions
{
    /// <summary>
    /// 转换静态扩展。
    /// </summary>
    public static class ConversionExtensions
    {

        #region Base64String

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

        #endregion


        #region HexString

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
            if (hexString.Length % 2 != 0)
                throw new ArgumentException("Hex length must be in multiples of 2.");

            var memory = hexString.AsMemory();

            var length = memory.Length / 2;
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
                buffer[i] = Convert.ToByte(memory.Slice(i * 2, 2).ToString(), 16);

            return buffer;
        }

        #endregion

    }
}
