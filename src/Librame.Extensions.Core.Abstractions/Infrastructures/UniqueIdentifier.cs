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
using System.Security.Cryptography;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 唯一标识符。
    /// </summary>
    public class UniqueIdentifier : IEquatable<UniqueIdentifier>
    {
        private static readonly string _base32Chars = "0123456789abcdefghjkmnpqrstvwxyz";

        private string _identifier;


        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/> 实例。
        /// </summary>
        /// <param name="generator">给定的 <see cref="RandomNumberGenerator"/>。</param>
        /// <param name="length">给定要生成的字节长度（可选；默认为 32 位）。</param>
        /// <param name="useHexEncode">使用 16 进制或 BASE64 编码。如果字节长度默认为 32，则生成字符串的长度为 64/44（可选；默认使用 16 进制编码）。</param>
        public UniqueIdentifier(RandomNumberGenerator generator, int length = 32, bool useHexEncode = true)
        {
            generator.NotNull(nameof(generator));

            var buffer = new byte[length];
            generator.GetBytes(buffer);

            _identifier = useHexEncode ? buffer.AsHexString() : buffer.AsBase64String();
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/> 实例。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/>。</param>
        /// <param name="base32Chars">给定的 BASE32 字符集合（可选；默认生成由数字加小写字母构成长度为 26 的字符串）。</param>
        public UniqueIdentifier(Guid guid, string base32Chars = null)
        {
            var buffer = guid.ToByteArray();

            var hs = BitConverter.ToInt64(buffer, 0);
            var ls = BitConverter.ToInt64(buffer, 8);

            if (base32Chars.IsNullOrEmpty())
                base32Chars = _base32Chars;

            _identifier = ToBase32(hs, ls, base32Chars);
        }


        private string ToBase32(long hs, long ls, string base32Chars)
        {
            var buffer = new char[26];

            buffer[0] = base32Chars[(int)(hs >> 60) & 31];
            buffer[1] = base32Chars[(int)(hs >> 55) & 31];
            buffer[2] = base32Chars[(int)(hs >> 50) & 31];
            buffer[3] = base32Chars[(int)(hs >> 45) & 31];
            buffer[4] = base32Chars[(int)(hs >> 40) & 31];
            buffer[5] = base32Chars[(int)(hs >> 35) & 31];
            buffer[6] = base32Chars[(int)(hs >> 30) & 31];
            buffer[7] = base32Chars[(int)(hs >> 25) & 31];
            buffer[8] = base32Chars[(int)(hs >> 20) & 31];
            buffer[9] = base32Chars[(int)(hs >> 15) & 31];
            buffer[10] = base32Chars[(int)(hs >> 10) & 31];
            buffer[11] = base32Chars[(int)(hs >> 5) & 31];
            buffer[12] = base32Chars[(int)hs & 31];

            buffer[13] = base32Chars[(int)(ls >> 60) & 31];
            buffer[14] = base32Chars[(int)(ls >> 55) & 31];
            buffer[15] = base32Chars[(int)(ls >> 50) & 31];
            buffer[16] = base32Chars[(int)(ls >> 45) & 31];
            buffer[17] = base32Chars[(int)(ls >> 40) & 31];
            buffer[18] = base32Chars[(int)(ls >> 35) & 31];
            buffer[19] = base32Chars[(int)(ls >> 30) & 31];
            buffer[20] = base32Chars[(int)(ls >> 25) & 31];
            buffer[21] = base32Chars[(int)(ls >> 20) & 31];
            buffer[22] = base32Chars[(int)(ls >> 15) & 31];
            buffer[23] = base32Chars[(int)(ls >> 10) & 31];
            buffer[24] = base32Chars[(int)(ls >> 5) & 31];
            buffer[25] = base32Chars[(int)ls & 31];

            return new string(buffer);
        }


        #region Overrrides

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is UniqueIdentifier other)
                return Equals(other);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => _identifier;

        #endregion


        #region Compares

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(UniqueIdentifier other)
            => this == other;


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        public static bool operator ==(UniqueIdentifier a, UniqueIdentifier b)
            => a.ToString() == b.ToString();

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回是否不等的布尔值。</returns>
        public static bool operator !=(UniqueIdentifier a, UniqueIdentifier b)
            => !(a == b);

        #endregion


        #region Converts

        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>。</param>
        public static implicit operator string(UniqueIdentifier identifier)
            => identifier?.ToString();

        /// <summary>
        /// 显式转换为 <see cref="UniqueIdentifier"/>（默认由数字加小写字母构成）。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/>。</param>
        public static explicit operator UniqueIdentifier(Guid guid)
            => new UniqueIdentifier(guid);

        /// <summary>
        /// 显式转换为 <see cref="UniqueIdentifier"/>（默认生成 32 位字节长度数组）。
        /// </summary>
        /// <param name="generator">给定的 <see cref="RandomNumberGenerator"/>。</param>
        public static explicit operator UniqueIdentifier(RandomNumberGenerator generator)
            => new UniqueIdentifier(generator);

        #endregion


        #region Instances

        /// <summary>
        /// 空 GUID 只读实例。
        /// </summary>
        /// <value>
        /// 返回 <see cref="UniqueIdentifier"/>。
        /// </value>
        public readonly static UniqueIdentifier EmptyByGuid
            = new UniqueIdentifier(Guid.Empty);


        /// <summary>
        /// 使用 <see cref="Guid.NewGuid()"/> 新建实例。
        /// </summary>
        /// <param name="base32Chars">给定的 BASE32 字符集合（可选；默认由数字加小写字母构成）。</param>
        /// <returns>返回 <see cref="UniqueIdentifier"/>。</returns>
        public static UniqueIdentifier NewByGuid(string base32Chars = null)
            => new UniqueIdentifier(Guid.NewGuid(), base32Chars);

        /// <summary>
        /// 使用 <see cref="RandomNumberGenerator.Create()"/> 新建实例。
        /// </summary>
        /// <param name="length">给定要生成的字节长度（可选；默认为 32 位）。</param>
        /// <returns>返回 <see cref="UniqueIdentifier"/>。</returns>
        public static UniqueIdentifier NewByRng(int length = 32)
            => new UniqueIdentifier(RandomNumberGenerator.Create(), length);

        #endregion

    }
}
