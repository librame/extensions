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
    /// 每年分表架构。
    /// </summary>
    public class EveryYearShardingSchema : ShardingSchema
    {
        /// <summary>
        /// 构造一个 <see cref="EveryYearShardingSchema"/> 实例。
        /// </summary>
        public EveryYearShardingSchema()
            : base(EveryYearShardingRule.Keys.EntitiesYear, typeof(EveryYearShardingRule))
        {
        }

    }
}
