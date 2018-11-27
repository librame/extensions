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
    /// 每天分表架构。
    /// </summary>
    public class EveryDayShardingSchema : ShardingSchema
    {
        /// <summary>
        /// 构造一个 <see cref="EveryDayShardingSchema"/> 实例。
        /// </summary>
        public EveryDayShardingSchema()
            : base(EveryDayShardingRule.Keys.EntitiesToday, typeof(EveryDayShardingRule))
        {
        }

    }
}
