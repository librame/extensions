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
    /// 每天分表选项。
    /// </summary>
    public class EveryDayShardingOptions : ShardingOptions
    {
        /// <summary>
        /// 构造一个 <see cref="EveryDayShardingOptions"/> 实例。
        /// </summary>
        public EveryDayShardingOptions()
            : base(EveryDayShardingRule.Keys.EntitiesToday, typeof(EveryDayShardingRule))
        {
        }

    }
}
