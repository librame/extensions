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
using System.Text;

namespace Librame.Extensions.Data.Migrations
{
    using Accessors;
    using Stores;

    /// <summary>
    /// 迁移命令信息。
    /// </summary>
    public class MigrationCommandInfo : IEquatable<MigrationCommandInfo>
    {
        /// <summary>
        /// 构造一个 <see cref="MigrationCommandInfo"/>。
        /// </summary>
        public MigrationCommandInfo()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="MigrationCommandInfo"/>。
        /// </summary>
        /// <param name="text">给定的文本。</param>
        /// <param name="connectionString">给定的连接字符串（可选）。</param>
        public MigrationCommandInfo(string text, string connectionString = null)
        {
            Text = text.NotEmpty(nameof(text));
            ConnectionString = connectionString;
        }


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
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{nameof(Text)}={Text};{nameof(ConnectionString)}={ConnectionString}";


        /// <summary>
        /// 设置连接字符串。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <returns>返回 <see cref="MigrationCommandInfo"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual MigrationCommandInfo SetConnectionString(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            var connector = DataSettings.Preference
                .MigrationCommandInfoConnectionStringConnector;

            var sb = new StringBuilder();

            sb.Append(nameof(ITenant.Name));
            sb.Append(connector);
            sb.Append(accessor.CurrentTenant.Name);
            sb.Append(';');

            sb.Append(nameof(ITenant.Host));
            sb.Append(connector);
            sb.Append(accessor.CurrentTenant.Host);
            sb.Append(';');

            sb.Append(nameof(ConnectionString));
            sb.Append(connector);
            sb.Append(accessor.GetCurrentConnectionDescription());

            ConnectionString = sb.ToString();

            return this;
        }

    }
}
