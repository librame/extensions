#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Security.Cryptography;

namespace Librame.Extensions
{
    /// <summary>
    /// 算法静态扩展。
    /// </summary>
    public static class AlgorithmExtensions
    {

        #region Hash Algorithm

        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Md5Base64String(this string str)
        {
            return str.AsEncodingBytes().Md5().AsBase64String();
        }

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha1Base64String(this string str)
        {
            return str.AsEncodingBytes().Sha1().AsBase64String();
        }

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha256Base64String(this string str)
        {
            return str.AsEncodingBytes().Sha256().AsBase64String();
        }

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha384Base64String(this string str)
        {
            return str.AsEncodingBytes().Sha384().AsBase64String();
        }

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha512Base64String(this string str)
        {
            return str.AsEncodingBytes().Sha512().AsBase64String();
        }


        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Md5(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
        {
            return buffer.Hash(HashAlgorithmName.MD5, rsa, padding);
        }

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha1(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
        {
            return buffer.Hash(HashAlgorithmName.SHA1, rsa, padding);
        }

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha256(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
        {
            return buffer.Hash(HashAlgorithmName.SHA256, rsa, padding);
        }

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha384(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
        {
            return buffer.Hash(HashAlgorithmName.SHA384, rsa, padding);
        }

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha512(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
        {
            return buffer.Hash(HashAlgorithmName.SHA512, rsa, padding);
        }

        private static byte[] Hash(this byte[] buffer, HashAlgorithmName algorithmName, RSA rsa = null, RSASignaturePadding padding = null)
        {
            var algorithm = HashAlgorithm.Create(algorithmName.Name);
            var hash = algorithm.ComputeHash(buffer);

            if (rsa.IsNotNull())
                hash = rsa.SignHash(hash, algorithmName, padding ?? RSASignaturePadding.Pkcs1);

            return hash;
        }

        #endregion


        #region HMAC Algorithm

        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacMd5Base64String(this string str, byte[] key)
        {
            return str.AsEncodingBytes().HmacMd5(key).AsBase64String();
        }
        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacSha1Base64String(this string str, byte[] key)
        {
            return str.AsEncodingBytes().HmacSha1(key).AsBase64String();
        }

        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacSha256Base64String(this string str, byte[] key)
        {
            return str.AsEncodingBytes().HmacSha256(key).AsBase64String();
        }

        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacSha384Base64String(this string str, byte[] key)
        {
            return str.AsEncodingBytes().HmacSha384(key).AsBase64String();
        }

        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacSha512Base64String(this string str, byte[] key)
        {
            return str.AsEncodingBytes().HmacSha512(key).AsBase64String();
        }


        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacMd5(this byte[] buffer, byte[] key)
        {
            return new HMACMD5(key).ComputeHash(buffer);
        }

        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha1(this byte[] buffer, byte[] key)
        {
            return new HMACSHA1(key).ComputeHash(buffer);
        }

        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha256(this byte[] buffer, byte[] key)
        {
            return new HMACSHA256(key).ComputeHash(buffer);
        }

        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha384(this byte[] buffer, byte[] key)
        {
            return new HMACSHA384(key).ComputeHash(buffer);
        }

        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha512(this byte[] buffer, byte[] key)
        {
            return new HMACSHA512(key).ComputeHash(buffer);
        }

        #endregion


        #region Symmetric Algorithm

        /// <summary>
        /// 转换为 AES。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string AsAesBase64String(this string str, byte[] key)
        {
            return str.AsEncodingBytes().AsAes(key).AsBase64String();
        }

        /// <summary>
        /// 还原 AES。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string FromAesBase64String(this string base64String, byte[] key)
        {
            return base64String.FromBase64String().FromAes(key).FromEncodingBytes();
        }


        /// <summary>
        /// 转换为 DES。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string AsDesBase64String(this string str, byte[] key)
        {
            return str.AsEncodingBytes().AsDes(key).AsBase64String();
        }

        /// <summary>
        /// 还原 DES。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string FromDesBase64String(this string base64String, byte[] key)
        {
            return base64String.FromBase64String().FromDes(key).FromEncodingBytes();
        }


        /// <summary>
        /// 转换为 TripleDES。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string AsTripleDesBase64String(this string str, byte[] key)
        {
            return str.AsEncodingBytes().AsTripleDes(key).AsBase64String();
        }

        /// <summary>
        /// 还原 TripleDES。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string FromTripleDesBase64String(this string base64String, byte[] key)
        {
            return base64String.FromBase64String().FromTripleDes(key).FromEncodingBytes();
        }


        /// <summary>
        /// 转换为 AES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsAes(this byte[] buffer, byte[] key)
        {
            return buffer.AsSymmetric(Aes.Create(), key);
        }

        /// <summary>
        /// 还原 AES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromAes(this byte[] buffer, byte[] key)
        {
            return buffer.FromSymmetric(Aes.Create(), key);
        }


        /// <summary>
        /// 转换为 DES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsDes(this byte[] buffer, byte[] key)
        {
            return buffer.AsSymmetric(DES.Create(), key);
        }

        /// <summary>
        /// 还原 DES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromDes(this byte[] buffer, byte[] key)
        {
            return buffer.FromSymmetric(DES.Create(), key);
        }


        /// <summary>
        /// 转换为 TripleDES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsTripleDes(this byte[] buffer, byte[] key)
        {
            return buffer.AsSymmetric(TripleDES.Create(), key);
        }

        /// <summary>
        /// 还原 TripleDES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromTripleDes(this byte[] buffer, byte[] key)
        {
            return buffer.FromSymmetric(TripleDES.Create(), key);
        }


        private static byte[] AsSymmetric(this byte[] buffer, SymmetricAlgorithm algorithm, byte[] key)
        {
            algorithm.Key = key;
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            var encryptor = algorithm.CreateEncryptor();
            return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }

        private static byte[] FromSymmetric(this byte[] buffer, SymmetricAlgorithm algorithm, byte[] key)
        {
            algorithm.Key = key;
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            var encryptor = algorithm.CreateDecryptor();
            return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }

        #endregion


        #region Asymmetric Algorithm : RSA

        /// <summary>
        /// 转换为 RSA。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsRsaBase64String(this string str, RSAParameters parameters, RSAEncryptionPadding padding = null)
        {
            return str.AsEncodingBytes().AsRsa(parameters, padding).AsBase64String();
        }

        /// <summary>
        /// 还原 RSA。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromRsaBase64String(this string base64String, RSAParameters parameters, RSAEncryptionPadding padding = null)
        {
            return base64String.FromBase64String().FromRsa(parameters, padding).FromEncodingBytes();
        }


        /// <summary>
        /// 转换为 RSA。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsRsa(this byte[] buffer, RSAParameters parameters, RSAEncryptionPadding padding = null)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(parameters);

            return rsa.Encrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);
        }

        /// <summary>
        /// 还原 RSA。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromRsa(this byte[] buffer, RSAParameters parameters, RSAEncryptionPadding padding = null)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(parameters);

            return rsa.Decrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);
        }

        #endregion

    }
}
