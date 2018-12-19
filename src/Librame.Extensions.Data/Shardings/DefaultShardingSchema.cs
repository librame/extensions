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
    /// 默认分表架构。
    /// </summary>
    public class DefaultShardingSchema : ShardingSchema
    {
        /// <summary>
        /// 构造一个表名格式为“Entities_Value”的 <see cref="DefaultShardingSchema"/> 实例。
        /// </summary>
        /// <param name="value">给定的值。</param>
        public DefaultShardingSchema(string value)
            : base(DefaultShardingRule.Keys.EntitiesDefault, typeof(BaseShardingRule),
                  new FormattingDescriptor(DefaultShardingRule.Keys.Default, value))
        {
        }

    }
}
