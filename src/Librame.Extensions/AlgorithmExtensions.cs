#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Librame.Extensions
{
    using Resources;

    /// <summary>
    /// 算法静态扩展。
    /// </summary>
    public static class AlgorithmExtensions
    {
        internal static ConcurrentDictionary<HashAlgorithmName, HashAlgorithm> HashAlgorithms
             = new ConcurrentDictionary<HashAlgorithmName, HashAlgorithm>();

        internal static ConcurrentDictionary<string, HMAC> HmacAlgorithms
             = new ConcurrentDictionary<string, HMAC>();

        internal static ConcurrentDictionary<string, SymmetricAlgorithm> SymmetricAlgorithms
             = new ConcurrentDictionary<string, SymmetricAlgorithm>();

        internal static ConcurrentDictionary<string, AsymmetricAlgorithm> AsymmetricAlgorithms
             = new ConcurrentDictionary<string, AsymmetricAlgorithm>();


        #region Base and Hex

        /// <summary>
        /// 转换 BASE32 字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string AsBase32String(this byte[] bytes)
        {
            bytes.NotEmpty(nameof(bytes));

            var chars = ExtensionSettings.Preference.Base32Chars;

            var sb = new StringBuilder();
            for (var offset = 0; offset < bytes.Length;)
            {
                var numCharsToOutput = GetNextGroup(bytes, ref offset,
                    out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h);

                sb.Append((numCharsToOutput >= 1) ? chars[a] : '=');
                sb.Append((numCharsToOutput >= 2) ? chars[b] : '=');
                sb.Append((numCharsToOutput >= 3) ? chars[c] : '=');
                sb.Append((numCharsToOutput >= 4) ? chars[d] : '=');
                sb.Append((numCharsToOutput >= 5) ? chars[e] : '=');
                sb.Append((numCharsToOutput >= 6) ? chars[f] : '=');
                sb.Append((numCharsToOutput >= 7) ? chars[g] : '=');
                sb.Append((numCharsToOutput >= 8) ? chars[h] : '=');
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
        /// 还原 BASE32 字符串。
        /// </summary>
        /// <param name="base32String">给定的 BASE32 字符串。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static byte[] FromBase32String(this string base32String)
        {
            base32String.NotEmpty(nameof(base32String));

            base32String = base32String.TrimEnd('=');
            if (base32String.Length == 0)
                return Array.Empty<byte>();

            if (base32String.HasLower())
                base32String = base32String.ToUpperInvariant();

            var chars = ExtensionSettings.Preference.Base32Chars;

            var bytes = new byte[base32String.Length * 5 / 8];
            var bitIndex = 0;
            var inputIndex = 0;
            var outputBits = 0;
            var outputIndex = 0;

            while (outputIndex < bytes.Length)
            {
                var byteIndex = chars.CompatibleIndexOf(base32String[inputIndex]);
                if (byteIndex < 0)
                    throw new FormatException(InternalResource.FormatExceptionBase32StringFormat.Format(base32String));

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
        /// 转换 BASE64 字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase64String(this byte[] bytes)
            => Convert.ToBase64String(bytes);

        /// <summary>
        /// 还原 BASE64 字符串。
        /// </summary>
        /// <param name="base64">给定的 BASE64 字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromBase64String(this string base64)
            => Convert.FromBase64String(base64);


        /// <summary>
        /// 转换 16 进制字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsHexString(this byte[] bytes)
            => BitConverter.ToString(bytes).CompatibleReplace("-", string.Empty);

        /// <summary>
        /// 还原 16 进制字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Invalid hex string.
        /// </exception>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static byte[] FromHexString(this string hexString)
        {
            hexString.NotEmpty(nameof(hexString));

            if (!hexString.Length.IsMultiples(2))
                throw new ArgumentException(InternalResource.ArgumentExceptionHexString);

            //var memory = hexString.AsMemory();
            var length = hexString.Length / 2;
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16); // memory.Slice(i * 2, 2)

            return buffer;
        }

        #endregion


        #region Hash Algorithm

        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Md5Base64String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Md5Base64String();

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha1Base64String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Sha1Base64String();

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha256Base64String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Sha256Base64String();

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha384Base64String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Sha384Base64String();

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha512Base64String(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Sha512Base64String();


        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Md5HexString(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Md5HexString();

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha1HexString(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Sha1HexString();

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha256HexString(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Sha256HexString();

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha384HexString(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Sha384HexString();

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="IExtensionPreferenceSetting.DefaultEncoding"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha512HexString(this string str, Encoding encoding = null)
            => str.FromEncodingString(encoding).Sha512HexString();


        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Md5Base64String(this byte[] buffer)
            => buffer.Md5().AsBase64String();

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha1Base64String(this byte[] buffer)
            => buffer.Sha1().AsBase64String();

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha256Base64String(this byte[] buffer)
            => buffer.Sha256().AsBase64String();

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha384Base64String(this byte[] buffer)
            => buffer.Sha384().AsBase64String();

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha512Base64String(this byte[] buffer)
            => buffer.Sha512().AsBase64String();


        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Md5HexString(this byte[] buffer)
            => buffer.Md5().AsHexString();

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha1HexString(this byte[] buffer)
            => buffer.Sha1().AsHexString();

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha256HexString(this byte[] buffer)
            => buffer.Sha256().AsHexString();

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha384HexString(this byte[] buffer)
            => buffer.Sha384().AsHexString();

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha512HexString(this byte[] buffer)
            => buffer.Sha512().AsHexString();


        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（可选；默认不签名）。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Md5(this byte[] buffer, bool isSigned = false,
            RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.ComputeHash(HashAlgorithmName.MD5, isSigned, rsa, padding);

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（可选；默认不签名）。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha1(this byte[] buffer, bool isSigned = false,
            RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.ComputeHash(HashAlgorithmName.SHA1, isSigned, rsa, padding);

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（可选；默认不签名）。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha256(this byte[] buffer, bool isSigned = false,
            RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.ComputeHash(HashAlgorithmName.SHA256, isSigned, rsa, padding);

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（可选；默认不签名）。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha384(this byte[] buffer, bool isSigned = false,
            RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.ComputeHash(HashAlgorithmName.SHA384, isSigned, rsa, padding);

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（可选；默认不签名）。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha512(this byte[] buffer, bool isSigned = false,
            RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.ComputeHash(HashAlgorithmName.SHA512, isSigned, rsa, padding);

        /// <summary>
        /// 计算散列。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="name">给定的散列算法名称。</param>
        /// <param name="isSigned">是否签名（可选；默认不签名）。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名，如果启用签名，则此参数不能为空）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static byte[] ComputeHash(this byte[] buffer, HashAlgorithmName name,
            bool isSigned = false, RSA rsa = null, RSASignaturePadding padding = null)
        {
            var algo = HashAlgorithms.GetOrAdd(name,
                key => HashAlgorithm.Create(key.Name));

            return ExtensionSettings.Preference.RunLockerResult(() =>
            {
                var hash = algo.ComputeHash(buffer);

                // 提供对服务调用的支持
                if (isSigned)
                {
                    rsa.NotNull(nameof(rsa));
                    hash = rsa.SignHash(hash, name, padding ?? RSASignaturePadding.Pkcs1);
                }

                return hash;
            });
        }

        #endregion


        #region HMAC Algorithm

        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.HmacShortKey"/>）。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Microsoft.Cryptography", "CA5351:DoNotUseBrokenCryptographicAlgorithms")]
        public static byte[] HmacMd5(this byte[] buffer, byte[] key = null)
            => buffer.ComputeHmacHash<HMACMD5>(key ?? ExtensionSettings.Preference.HmacShortKey.FromBase64String());

        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.HmacShortKey"/>）。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Microsoft.Cryptography", "CA5350:DoNotUseWeakCryptographicAlgorithms")]
        public static byte[] HmacSha1(this byte[] buffer, byte[] key = null)
            => buffer.ComputeHmacHash<HMACSHA1>(key ?? ExtensionSettings.Preference.HmacShortKey.FromBase64String());

        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.HmacShortKey"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha256(this byte[] buffer, byte[] key = null)
            => buffer.ComputeHmacHash<HMACSHA256>(key ?? ExtensionSettings.Preference.HmacShortKey.FromBase64String());

        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.HmacLongKey"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha384(this byte[] buffer, byte[] key = null)
            => buffer.ComputeHmacHash<HMACSHA384>(key ?? ExtensionSettings.Preference.HmacLongKey.FromBase64String());

        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.HmacLongKey"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha512(this byte[] buffer, byte[] key = null)
            => buffer.ComputeHmacHash<HMACSHA512>(key ?? ExtensionSettings.Preference.HmacLongKey.FromBase64String());

        /// <summary>
        /// 计算 HMAC 散列。
        /// </summary>
        /// <typeparam name="THmac">指定的 HMAC 类型。</typeparam>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] ComputeHmacHash<THmac>(this byte[] buffer, byte[] key)
            where THmac : HMAC
        {
            var algo = HmacAlgorithms.GetOrAdd(typeof(THmac).Name,
                key => HMAC.Create(key));
            
            return ExtensionSettings.Preference.RunLockerResult(() =>
            {
                algo.Key = key.NotEmpty(nameof(key));

                return algo.ComputeHash(buffer);
            });
        }

        #endregion


        #region Symmetric Algorithm

        /// <summary>
        /// 转换为 AES。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is null or empty.
        /// </exception>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.AesKey"/>）。</param>
        /// <param name="iv">给定的向量（可选；默认使用 <see cref="IExtensionPreferenceSetting.AesVector"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsAes(this byte[] buffer, byte[] key = null, byte[] iv = null)
            => buffer.SymmetricTransform<Aes>(isEncrypt: true,
                key ?? ExtensionSettings.Preference.AesKey.FromBase64String(),
                iv ?? ExtensionSettings.Preference.AesVector.FromBase64String());

        /// <summary>
        /// 还原 AES。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is null or empty.
        /// </exception>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.AesKey"/>）。</param>
        /// <param name="iv">给定的向量（可选；默认使用 <see cref="IExtensionPreferenceSetting.AesVector"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromAes(this byte[] buffer, byte[] key = null, byte[] iv = null)
            => buffer.SymmetricTransform<Aes>(isEncrypt: false,
                key ?? ExtensionSettings.Preference.AesKey.FromBase64String(),
                iv ?? ExtensionSettings.Preference.AesVector.FromBase64String());


        /// <summary>
        /// 转换为 DES。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is null or empty.
        /// </exception>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.DesKey"/>）。</param>
        /// <param name="iv">给定的向量（可选；默认使用 <see cref="IExtensionPreferenceSetting.DesVector"/>）。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Security", "CA5351:不要使用损坏的加密算法")]
        public static byte[] AsDes(this byte[] buffer, byte[] key = null, byte[] iv = null)
            => buffer.SymmetricTransform<DES>(isEncrypt: true,
                key ?? ExtensionSettings.Preference.DesKey.FromBase64String(),
                iv ?? ExtensionSettings.Preference.DesVector.FromBase64String());

        /// <summary>
        /// 还原 DES。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is null or empty.
        /// </exception>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.DesKey"/>）。</param>
        /// <param name="iv">给定的向量（可选；默认使用 <see cref="IExtensionPreferenceSetting.DesVector"/>）。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Security", "CA5351:不要使用损坏的加密算法")]
        public static byte[] FromDes(this byte[] buffer, byte[] key = null, byte[] iv = null)
            => buffer.SymmetricTransform<DES>(isEncrypt: false,
                key ?? ExtensionSettings.Preference.DesKey.FromBase64String(),
                iv ?? ExtensionSettings.Preference.DesVector.FromBase64String());


        /// <summary>
        /// 转换为 TripleDES。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is null or empty.
        /// </exception>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.TripleDesKey"/>）。</param>
        /// <param name="iv">给定的向量（可选；默认使用 <see cref="IExtensionPreferenceSetting.TripleDesVector"/>）。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Security", "CA5350:不要使用弱加密算法")]
        public static byte[] AsTripleDes(this byte[] buffer, byte[] key = null, byte[] iv = null)
            => buffer.SymmetricTransform<TripleDES>(isEncrypt: true,
                key ?? ExtensionSettings.Preference.TripleDesKey.FromBase64String(),
                iv ?? ExtensionSettings.Preference.TripleDesVector.FromBase64String());

        /// <summary>
        /// 还原 TripleDES。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is null or empty.
        /// </exception>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥（可选；默认使用 <see cref="IExtensionPreferenceSetting.TripleDesKey"/>）。</param>
        /// <param name="iv">给定的向量（可选；默认使用 <see cref="IExtensionPreferenceSetting.TripleDesVector"/>）。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Security", "CA5350:不要使用弱加密算法")]
        public static byte[] FromTripleDes(this byte[] buffer, byte[] key = null, byte[] iv = null)
            => buffer.SymmetricTransform<TripleDES>(isEncrypt: false,
                key ?? ExtensionSettings.Preference.TripleDesKey.FromBase64String(),
                iv ?? ExtensionSettings.Preference.TripleDesVector.FromBase64String());


        /// <summary>
        /// 对称变换。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is null or empty.
        /// </exception>
        /// <typeparam name="TSym">指定的对称算法类型。</typeparam>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isEncrypt">是加密运算，反之则解密。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="iv">给定的向量。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static byte[] SymmetricTransform<TSym>(this byte[] buffer, bool isEncrypt, byte[] key, byte[] iv)
            where TSym : SymmetricAlgorithm
        {
            buffer.NotEmpty(nameof(buffer));

            var algo = SymmetricAlgorithms.GetOrAdd(typeof(TSym).Name,
                key => SymmetricAlgorithm.Create(key));

            return ExtensionSettings.Preference.RunLockerResult(() =>
            {
                algo.Key = key.NotEmpty(nameof(key));
                algo.IV = iv.NotEmpty(nameof(iv));

                algo.Mode = CipherMode.ECB;
                algo.Padding = PaddingMode.PKCS7;

                if (isEncrypt)
                {
                    var encryptor = algo.CreateEncryptor();
                    return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                }

                var decryptor = algo.CreateDecryptor();
                return decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            });
        }

        #endregion


        #region AsymmetricAlgorithm

        /// <summary>
        /// RSA 非对称加密。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsRsa(this byte[] buffer, RSAParameters parameters, RSAEncryptionPadding padding = null)
            => buffer.RsaEndecrypt(parameters, padding, isEncrypt: true);

        /// <summary>
        /// RSA 非对称解密。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromRsa(this byte[] buffer, RSAParameters parameters, RSAEncryptionPadding padding = null)
            => buffer.RsaEndecrypt(parameters, padding, isEncrypt: false);

        private static byte[] RsaEndecrypt(this byte[] buffer, RSAParameters parameters,
            RSAEncryptionPadding padding = null, bool isEncrypt = default)
        {
            return AsymmetricEndecrypt<RSA>(rsa =>
            {
                rsa.ImportParameters(parameters);

                if (isEncrypt)
                    return rsa.Encrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);

                return rsa.Decrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);
            });
        }


        /// <summary>
        /// 非对称加解密。
        /// </summary>
        /// <typeparam name="TAsym">指定的非对称算法类型。</typeparam>
        /// <param name="endecryptFactory">给定的加解密工厂方法。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static byte[] AsymmetricEndecrypt<TAsym>(Func<TAsym, byte[]> endecryptFactory)
            where TAsym : AsymmetricAlgorithm
        {
            endecryptFactory.NotNull(nameof(endecryptFactory));

            var algo = AsymmetricAlgorithms.GetOrAdd(typeof(TAsym).Name,
                key => AsymmetricAlgorithm.Create(key));

            return ExtensionSettings.Preference.RunLockerResult(() => endecryptFactory.Invoke(algo as TAsym));
        }

        #endregion

    }
}
