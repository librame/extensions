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
    /// 基础分表规则。
    /// </summary>
    public class BaseShardingRule : AbstractShardingRule
    {
        /// <summary>
        /// 转换为表架构。
        /// </summary>
        /// <param name="name">给定的表名。</param>
        /// <param name="schema">给定的架构。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        protected override ITableSchema ToTable(string name, string schema)
        {
            return new TableSchema(name).TryApplySchema(schema);
        }

    }
}
