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
using System.Linq;
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
        public static Encoding FromName(this string name)
            => Encoding.GetEncoding(name);


        /// <summary>
        /// 转换为字符编码的字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingString(this byte[] bytes, Encoding encoding = null)
            => (encoding ?? Encoding.UTF8).GetString(bytes);

        /// <summary>
        /// 还原为字符编码的字节数组。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromEncodingString(this string str, Encoding encoding = null)
            => (encoding ?? Encoding.UTF8).GetBytes(str);


        #region Base32

        private static readonly string _base32Chars = AlgorithmExtensions.UPPER + "234567";

        /// <summary>
        /// 转换为 BASE32 字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase32String(this byte[] bytes)
        {
            bytes.NotNullOrEmpty(nameof(bytes));

            var sb = new StringBuilder();
            for (var offset = 0; offset < bytes.Length;)
            {
                var numCharsToOutput = GetNextGroup(bytes, ref offset,
                    out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h);

                sb.Append((numCharsToOutput >= 1) ? _base32Chars[a] : '=');
                sb.Append((numCharsToOutput >= 2) ? _base32Chars[b] : '=');
                sb.Append((numCharsToOutput >= 3) ? _base32Chars[c] : '=');
                sb.Append((numCharsToOutput >= 4) ? _base32Chars[d] : '=');
                sb.Append((numCharsToOutput >= 5) ? _base32Chars[e] : '=');
                sb.Append((numCharsToOutput >= 6) ? _base32Chars[f] : '=');
                sb.Append((numCharsToOutput >= 7) ? _base32Chars[g] : '=');
                sb.Append((numCharsToOutput >= 8) ? _base32Chars[h] : '=');
            }

            return sb.ToString();

            int GetNextGroup(byte[] buffer, ref int offset,
                out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
            {
                uint b1, b2, b3, b4, b5;

                int retVal;
                switch (offset - buffer.Length)
                {
                    case 1: retVal = 2; break;
                    case 2: retVal = 4; break;
                    case 3: retVal = 5; break;
                    case 4: retVal = 7; break;
                    default: retVal = 8; break;
                }

                b1 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b2 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b3 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b4 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b5 = (offset < buffer.Length) ? buffer[offset++] : 0U;

                a = (byte)(b1 >> 3);
                b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6));
                c = (byte)((b2 >> 1) & 0x1f);
                d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4));
                e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7));
                f = (byte)((b4 >> 2) & 0x1f);
                g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
                h = (byte)(b5 & 0x1f);

                return retVal;
            }
        }

        /// <summary>
        /// 还原为 BASE32 字符串。
        /// </summary>
        /// <param name="base32String">给定的 BASE32 字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromBase32String(this string base32String)
        {
            base32String.NotNullOrEmpty(nameof(base32String));

            base32String = base32String.TrimEnd('=');
            if (base32String.Length == 0)
                return new byte[0];

            if (base32String.HasLower())
                base32String = base32String.ToUpperInvariant();

            var bytes = new byte[base32String.Length * 5 / 8];
            var bitIndex = 0;
            var inputIndex = 0;
            var outputBits = 0;
            var outputIndex = 0;

            while (outputIndex < bytes.Length)
            {
                var byteIndex = _base32Chars.IndexOf(base32String[inputIndex]);
                if (byteIndex < 0)
                    throw new FormatException();

                var bits = Math.Min(5 - bitIndex, 8 - outputBits);
                bytes[outputIndex] <<= bits;
                bytes[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

                bitIndex += bits;
                if (bitIndex >= 5)
                {
                    inputIndex++;
                    bitIndex = 0;
                }

                outputBits += bits;
                if (outputBits >= 8)
                {
                    outputIndex++;
                    outputBits = 0;
                }
            }

            // 因字符串强制以“\0”结尾，故需手动移除数组末尾的“0”字节，才能正确还原源数组
            bytes = bytes.TrimEnd(byte.MinValue).ToArray();

            return bytes;
        }


        /// <summary>
        /// 转换为经过字符编码的 BASE32 字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingBase32String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).AsBase32String();

        /// <summary>
        /// 还原为经过字符编码的 BASE32 字符串。
        /// </summary>
        /// <param name="base32String">给定的 BASE32 字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingBase32String(this string base32String, Encoding encoding = null)
            => base32String.FromBase32String().AsEncodingString(encoding);

        #endregion


        #region Base64

        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase64String(this byte[] bytes)
            => Convert.ToBase64String(bytes);

        /// <summary>
        /// 还原为 BASE64 字符串。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromBase64String(this string base64String)
            => Convert.FromBase64String(base64String);


        /// <summary>
        /// 转换为经过字符编码的 BASE64 字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingBase64String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).AsBase64String();

        /// <summary>
        /// 还原为经过字符编码的 BASE64 字符串。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingBase64String(this string base64String, Encoding encoding = null)
            => base64String.FromBase64String().AsEncodingString(encoding);

        #endregion


        #region Hex

        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsHexString(this byte[] bytes)
            => BitConverter.ToString(bytes).Replace("-", string.Empty);

        /// <summary>
        /// 还原为 16 进制字符串。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromHexString(this string hexString)
        {
            hexString.NotNullOrEmpty(nameof(hexString));

            if (!hexString.Length.IsMultiples(2))
                throw new ArgumentException("Hex length must be in multiples of 2.");

            //var memory = hexString.AsMemory();

            var length = hexString.Length / 2;
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16); // memory.Slice(i * 2, 2)

            return buffer;
        }


        /// <summary>
        /// 转换为经过字符编码的 16 进制字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingHexString(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).AsHexString();

        /// <summary>
        /// 还原为经过字符编码的 16 进制字符串。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromEncodingHexString(this string hexString, Encoding encoding = null)
            => hexString.FromHexString().AsEncodingString(encoding);

        #endregion

    }
}
