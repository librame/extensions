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

namespace Librame.Algorithm
{
    /// <summary>
    /// 散列算法。
    /// </summary>
    public class HashAlgorithm : AbstractByteCodec, IHashAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractByteCodec"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public HashAlgorithm(AlgorithmSettings algoSettings)
            : base(algoSettings)
        {
        }


        /// <summary>
        /// 转换为 MD5。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 MD5 字符串。</returns>
        public virtual string ToMd5(string str)
        {
            var buffer = GetBytes(str);

            var md5 = new MD5CryptoServiceProvider();
            buffer = md5.ComputeHash(buffer);

            return EncodeBit(buffer);
        }


        /// <summary>
        /// 转换为 SHA1。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 SHA1 字符串。</returns>
        public virtual string ToSha1(string str)
        {
            var buffer = GetBytes(str);

            var sha1 = new SHA1CryptoServiceProvider();
            buffer = sha1.ComputeHash(buffer);

            return EncodeBit(buffer);
        }


        /// <summary>
        /// 转换为 SHA256。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 SHA256 字符串。</returns>
        public virtual string ToSha256(string str)
        {
            var buffer = GetBytes(str);

            var sha256 = new SHA256CryptoServiceProvider();
            buffer = sha256.ComputeHash(buffer);

            return EncodeBit(buffer);
        }


        /// <summary>
        /// 转换为 SHA384。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 SHA384 字符串。</returns>
        public virtual string ToSha384(string str)
        {
            var buffer = GetBytes(str);

            var sha384 = new SHA384CryptoServiceProvider();
            buffer = sha384.ComputeHash(buffer);

            return EncodeBit(buffer);
        }


        /// <summary>
        /// 转换为 SHA512。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 SHA512 字符串。</returns>
        public virtual string ToSha512(string str)
        {
            var buffer = GetBytes(str);

            var sha512 = new SHA512CryptoServiceProvider();
            buffer = sha512.ComputeHash(buffer);

            return EncodeBit(buffer);
        }

    }
}
