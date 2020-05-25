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

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// <see cref="SecurityIdentifier"/> 钥匙信息。
    /// </summary>
    public class SecurityIdentifierKeyInfo : IEquatable<SecurityIdentifierKeyInfo>
    {
        /// <summary>
        /// 索引。
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// 标识符。
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="SecurityIdentifierKeyInfo"/>（可选）。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(SecurityIdentifierKeyInfo other)
            => Index == other?.Index;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is SecurityIdentifierKeyInfo other && Equals(other);


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
            => $"{Index}{CoreSettings.Preference.KeySeparator}{Identifier}";


        /// <summary>
        /// 创建安全标识符钥匙信息。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>。</param>
        /// <param name="createdTime">给定的创建时间。</param>
        /// <returns>返回 <see cref="SecurityIdentifierKeyInfo"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static SecurityIdentifierKeyInfo Create(SecurityIdentifier identifier,
            DateTimeOffset? createdTime = null)
        {
            identifier.NotNull(nameof(identifier));

            if (!createdTime.HasValue)
                createdTime = DateTimeOffset.UtcNow;

            return new SecurityIdentifierKeyInfo
            {
                Index = identifier.ToShortString(createdTime.Value),
                Identifier = identifier,
                CreatedTime = createdTime.Value
            };
        }

    }
}
