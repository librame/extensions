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
    /// 每周分表架构。
    /// </summary>
    public class EveryWeekShardingSchema : ShardingSchema
    {
        /// <summary>
        /// 构造一个 <see cref="EveryWeekShardingSchema"/> 实例。
        /// </summary>
        public EveryWeekShardingSchema()
            : base(EveryWeekShardingRule.Keys.EntitiesWeek, typeof(EveryWeekShardingRule))
        {
        }

    }
}
