#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 分表规则接口。
    /// </summary>
    public interface IShardingRule
    {
        /// <summary>
        /// 转换为表选项。
        /// </summary>
        /// <param name="sharding">给定的 <see cref="IShardingSchema"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回 <see cref="TableAttribute"/>。</returns>
        ITableSchema ToTable(IShardingSchema sharding, Type entityType);
    }
}
