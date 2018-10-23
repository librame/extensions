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
    /// 每月分表选项。
    /// </summary>
    public class EveryMonthShardingOptions : ShardingOptions
    {
        /// <summary>
        /// 构造一个 <see cref="EveryMonthShardingOptions"/> 实例。
        /// </summary>
        public EveryMonthShardingOptions()
            : base(EveryMonthShardingRule.Keys.EntitiesMonth, typeof(EveryMonthShardingRule))
        {
        }

    }
}
