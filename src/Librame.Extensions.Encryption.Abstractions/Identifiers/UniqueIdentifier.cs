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

namespace Librame.Extensions.Encryption
{
    /// <summary>
    /// 唯一标识符。
    /// </summary>
    public struct UniqueIdentifier : IEquatable<UniqueIdentifier>, IDefaultable
    {
        private static readonly string _base32Chars = "0123456789abcdefghjkmnpqrstvwxyz";

        private string _identifier;


        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/> 实例。
        /// </summary>
        /// <param name="generator">给定的 <see cref="RandomNumberGenerator"/>。</param>
        /// <param name="length">给定要生成的长度（可选；默认为 32）。</param>
        public UniqueIdentifier(RandomNumberGenerator generator, int length = 32)
        {
            var buffer = new byte[length];
            generator.GetBytes(buffer);

            _identifier = buffer.AsHexString();
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/> 实例。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/>。</param>
        /// <param name="base32Chars">给定的 BASE64 字符集合（可选；默认由数字加小写字母共 26 个字符）。</param>
        public UniqueIdentifier(Guid guid, string base32Chars = null)
        {
            var buffer = guid.ToByteArray();

            var hs = BitConverter.ToInt64(buffer, 0);
            var ls = BitConverter.ToInt64(buffer, 8);

            if (base32Chars.IsEmpty())
                base32Chars = _base32Chars;

            _identifier = ToBase32(hs, ls, base32Chars);
        }


        private static string ToBase32(long hs, long ls, string base32Chars)
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


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(UniqueIdentifier other)
        {
            return this == other;
        }

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
        {
            return ToString().GetHashCode();
        }


        /// <summary>
        /// 是否为默认值。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public bool IsDefaulting()
        {
            return _identifier.IsEmpty();
        }


        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return _identifier;
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        public static bool operator ==(UniqueIdentifier a, UniqueIdentifier b)
        {
            return a.ToString() == b.ToString();
        }

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回是否不等的布尔值。</returns>
        public static bool operator !=(UniqueIdentifier a, UniqueIdentifier b)
        {
            return !(a == b);
        }


        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <param name="base32Chars">给定的 BASE64 字符集合（可选；默认由数字加小写字母共 26 个字符）。</param>
        /// <returns>返回 <see cref="UniqueIdentifier"/>。</returns>
        public static UniqueIdentifier NewByGuid(string base32Chars = null)
        {
            return new UniqueIdentifier(Guid.NewGuid(), base32Chars);
        }

        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <param name="length">给定要生成的长度（可选；默认为 32）。</param>
        /// <returns>返回 <see cref="UniqueIdentifier"/>。</returns>
        public static UniqueIdentifier NewByRng(int length = 32)
        {
            return new UniqueIdentifier(RandomNumberGenerator.Create(), length);
        }

    }
}
