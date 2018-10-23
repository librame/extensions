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
    /// 每周分表选项。
    /// </summary>
    public class EveryWeekShardingOptions : ShardingOptions
    {
        /// <summary>
        /// 构造一个 <see cref="EveryWeekShardingOptions"/> 实例。
        /// </summary>
        public EveryWeekShardingOptions()
            : base(EveryWeekShardingRule.Keys.EntitiesWeek, typeof(EveryWeekShardingRule))
        {
        }

    }
}
