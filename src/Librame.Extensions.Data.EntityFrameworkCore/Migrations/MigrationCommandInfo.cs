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

namespace Librame.Extensions.Data.Migrations
{
    /// <summary>
    /// 迁移命令信息。
    /// </summary>
    public class MigrationCommandInfo : IEquatable<MigrationCommandInfo>
    {
        /// <summary>
        /// 文本。
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 连接字符串。
        /// </summary>
        public string ConnectionString { get; set; }


        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="other">给定的 <see cref="MigrationCommandInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(MigrationCommandInfo other)
            => Text == other?.Text && ConnectionString == other.ConnectionString;

        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is MigrationCommandInfo other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode(StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{nameof(Text)}={Text};{nameof(ConnectionString)}={ConnectionString}";
    }
}
