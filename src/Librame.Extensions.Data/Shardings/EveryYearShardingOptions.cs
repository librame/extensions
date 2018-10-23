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
    /// 每年分表选项。
    /// </summary>
    public class EveryYearShardingOptions : ShardingOptions
    {
        /// <summary>
        /// 构造一个 <see cref="EveryYearShardingOptions"/> 实例。
        /// </summary>
        public EveryYearShardingOptions()
            : base(EveryYearShardingRule.Keys.EntitiesYear, typeof(EveryYearShardingRule))
        {
        }

    }
}
