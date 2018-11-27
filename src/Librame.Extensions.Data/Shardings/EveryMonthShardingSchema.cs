#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 每月分表架构。
    /// </summary>
    public class EveryMonthShardingSchema : ShardingSchema
    {
        /// <summary>
        /// 构造一个 <see cref="EveryMonthShardingSchema"/> 实例。
        /// </summary>
        public EveryMonthShardingSchema()
            : base(EveryMonthShardingRule.Keys.EntitiesMonth, typeof(EveryMonthShardingRule))
        {
        }

    }
}
