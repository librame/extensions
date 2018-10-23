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
    /// 每月分表规则。
    /// </summary>
    public class EveryMonthShardingRule : AbstractShardingRule
    {
        /// <summary>
        /// 添加默认键值对。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingOptions"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected override void AddDefaultKeyValues(IShardingOptions sharding, Type entityType)
        {
            base.AddDefaultKeyValues(sharding, entityType);

            sharding.Formattings.Add(new FormattingDescriptor(Keys.Month, DateTime.Now.ToString("yyyyMM")));
        }


        /// <summary>
        /// 键名集合。
        /// </summary>
        public new class Keys : AbstractShardingRule.Keys
        {
            /// <summary>
            /// 本月键名。
            /// </summary>
            public static readonly string Month = BuildKey(nameof(Month));

            /// <summary>
            /// 实体键名复数加本月键名。
            /// </summary>
            public static readonly string EntitiesMonth = $"{Entities}_{Month}";
        }

    }
}
