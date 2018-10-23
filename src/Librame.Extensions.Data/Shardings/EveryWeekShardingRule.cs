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
    /// 每周分表规则。
    /// </summary>
    public class EveryWeekShardingRule : AbstractShardingRule
    {
        /// <summary>
        /// 添加默认键值对。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingOptions"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected override void AddDefaultKeyValues(IShardingOptions sharding, Type entityType)
        {
            base.AddDefaultKeyValues(sharding, entityType);

            var now = DateTime.Now;
            sharding.Formattings.Add(new FormattingDescriptor(Keys.Week, now.Year.ToString() + now.AsWeekOfYear()));
        }


        /// <summary>
        /// 键名集合。
        /// </summary>
        public new class Keys : AbstractShardingRule.Keys
        {
            /// <summary>
            /// 本周键名。
            /// </summary>
            public static readonly string Week = BuildKey(nameof(Week));

            /// <summary>
            /// 实体键名复数加本周键名。
            /// </summary>
            public static readonly string EntitiesWeek = $"{Entities}_{Week}";
        }

    }
}
