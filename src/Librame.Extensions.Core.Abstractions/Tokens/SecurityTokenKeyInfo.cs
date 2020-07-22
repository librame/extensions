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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Tokens
{
    /// <summary>
    /// <see cref="SecurityToken"/> 钥匙信息。
    /// </summary>
    public class SecurityTokenKeyInfo : IEquatable<SecurityTokenKeyInfo>
    {
        /// <summary>
        /// 索引。
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// 令牌。
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="SecurityTokenKeyInfo"/>（可选）。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(SecurityTokenKeyInfo other)
            => Index == other?.Index;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is SecurityTokenKeyInfo other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => Index.CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{Index}{CoreSettings.Preference.KeySeparator}{Token}";


        /// <summary>
        /// 创建安全令牌钥匙信息。
        /// </summary>
        /// <param name="token">给定的 <see cref="SecurityToken"/>。</param>
        /// <param name="createdTime">给定的创建时间。</param>
        /// <returns>返回 <see cref="SecurityTokenKeyInfo"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static SecurityTokenKeyInfo Create(SecurityToken token,
            DateTimeOffset? createdTime = null)
        {
            token.NotNull(nameof(token));

            if (!createdTime.HasValue)
                createdTime = DateTimeOffset.UtcNow;

            return new SecurityTokenKeyInfo
            {
                Index = token.ToShortString(createdTime.Value),
                Token = token,
                CreatedTime = createdTime.Value
            };
        }

    }
}
