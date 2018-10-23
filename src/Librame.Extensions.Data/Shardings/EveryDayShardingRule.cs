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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 每天分表规则。
    /// </summary>
    public class EveryDayShardingRule : AbstractShardingRule
    {
        /// <summary>
        /// 添加默认键值对。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingOptions"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected override void AddDefaultKeyValues(IShardingOptions sharding, Type entityType)
        {
            base.AddDefaultKeyValues(sharding, entityType);

            sharding.Formattings.Add(new FormattingDescriptor(Keys.Today, DateTime.Now.ToString("yyyyMMdd")));
        }


        /// <summary>
        /// 键名集合。
        /// </summary>
        public new class Keys : AbstractShardingRule.Keys
        {
            /// <summary>
            /// 今天键名。
            /// </summary>
            public static readonly string Today = BuildKey(nameof(Today));

            /// <summary>
            /// 实体键名复数加今天键名。
            /// </summary>
            public static readonly string EntitiesToday = $"{Entities}_{Today}";
        }

    }
}
