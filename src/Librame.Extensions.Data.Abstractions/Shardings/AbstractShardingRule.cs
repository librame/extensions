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
using System.ComponentModel.DataAnnotations.Schema;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象分表规则。
    /// </summary>
    public abstract class AbstractShardingRule : IShardingRule
    {
        /// <summary>
        /// 转换为表特性。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingOptions"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回 <see cref="TableAttribute"/>。</returns>
        public virtual ITableOptions ToTable(IShardingOptions sharding, Type entityType)
        {
            AddDefaultKeyValues(sharding, entityType);
            
            var name = FormatKeyValues(sharding);

            return new TableOptions(name).TryApplySchema(sharding.Schema);
        }


        /// <summary>
        /// 格式化键值对集合为表名。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingOptions"/>。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string FormatKeyValues(IShardingOptions sharding)
        {
            var formatter = sharding.Formatter;

            foreach (var pair in sharding.Formattings)
                formatter = formatter.Replace(pair.Key, pair.Value);

            return formatter;
        }

        /// <summary>
        /// 添加默认键值对。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingOptions"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected virtual void AddDefaultKeyValues(IShardingOptions sharding, Type entityType)
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
