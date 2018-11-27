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
using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象分表规则。
    /// </summary>
    public abstract class AbstractShardingRule : IShardingRule
    {
        /// <summary>
        /// 转换为表架构。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingSchema"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public virtual ITableSchema ToTable(IShardingSchema sharding, Type entityType)
        {
            AddDefaultKeyValues(sharding, entityType);
            
            var name = FormatKeyValues(sharding);

            return ToTable(name, sharding.Schema);
        }

        /// <summary>
        /// 转换为表架构。
        /// </summary>
        /// <param name="name">给定的表名。</param>
        /// <param name="schema">给定的架构。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        protected abstract ITableSchema ToTable(string name, string schema);


        /// <summary>
        /// 格式化键值对集合为表名。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingSchema"/>。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string FormatKeyValues(IShardingSchema sharding)
        {
            var formatter = sharding.Formatter;

            foreach (var pair in sharding.Formattings)
                formatter = formatter.Replace(pair.Key, pair.Value);

            return formatter;
        }

        /// <summary>
        /// 添加默认键值对。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingSchema"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected virtual void AddDefaultKeyValues(IShardingSchema sharding, Type entityType)
        {
            if (sharding.Formattings.IsDefault())
                sharding.Formattings = new List<FormattingDescriptor>();

            sharding.Formattings.Add(new FormattingDescriptor(Keys.Entity, entityType.Name));
            sharding.Formattings.Add(new FormattingDescriptor(Keys.Entities, entityType.Name.AsPluralize()));
        }


        /// <summary>
        /// 键名集合。
        /// </summary>
        public class Keys
        {
            /// <summary>
            /// 实体键名。
            /// </summary>
            public static readonly string Entity = BuildKey(nameof(Entity));

            /// <summary>
            /// 实体键名复数。
            /// </summary>
            public static readonly string Entities = BuildKey(nameof(Entities));


            /// <summary>
            /// 建立键名。
            /// </summary>
            /// <param name="name">给定的名称。</param>
            /// <returns>返回字符串。</returns>
            public static string BuildKey(string name)
            {
                return "{" + name + "}";
            }
        }

    }
}
