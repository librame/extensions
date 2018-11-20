#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据库提供程序接口。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public interface IDbContext<TBuilderOptions> : IDbContext, IDbProvider<TBuilderOptions>
        where TBuilderOptions : DataBuilderOptions
    {
    }


    /// <summary>
    /// 数据库提供程序接口。
    /// </summary>
    public interface IDbContext : IDbProvider
    {
        /// <summary>
        /// 变化跟踪器上下文。
        /// </summary>
        IChangeTrackerContext TrackerContext { get; }


        /// <summary>
        /// 审计数据集。
        /// </summary>
        DbSet<Audit> Audits { get; set; }

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        DbSet<AuditProperty> AuditProperties { get; set; }

        /// <summary>
        /// 租户数据集。
        /// </summary>
        DbSet<Tenant> Tenants { get; set; }
    }
}
