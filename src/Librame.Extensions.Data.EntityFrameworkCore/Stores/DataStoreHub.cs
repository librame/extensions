﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Accessors;
    using Collections;

    /// <summary>
    /// 数据存储中心。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class DataStoreHub<TAccessor> : DataStoreHub<TAccessor, Guid, int, Guid>
        where TAccessor : class, IDataAccessor
    {
        /// <summary>
        /// 构造一个数据存储中心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected DataStoreHub(IAccessor accessor)
            : base(accessor)
        {
        }

    }


    /// <summary>
    /// 数据存储中心。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataStoreHub<TAccessor, TGenId, TIncremId, TCreatedBy> : DataStoreHub<TAccessor,
        DataAudit<TGenId, TCreatedBy>,
        DataAuditProperty<TIncremId, TGenId>,
        DataMigration<TGenId, TCreatedBy>,
        DataTabulation<TGenId, TCreatedBy>,
        DataTenant<TGenId, TCreatedBy>,
        TGenId, TIncremId, TCreatedBy>
        where TAccessor : class, IDataAccessor<TGenId, TIncremId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个数据存储中心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected DataStoreHub(IAccessor accessor)
            : base(accessor)
        {
        }

    }


    /// <summary>
    /// 数据存储中心。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataStoreHub<TAccessor, TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy>
        : AbstractStoreHub, IDataStoreHub<TAccessor, TAudit, TAuditProperty, TMigration, TTabulation, TTenant>
        where TAccessor : class, IDataAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant>
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty: DataAuditProperty<TIncremId, TGenId>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTabulation : DataTabulation<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个数据存储中心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected DataStoreHub(IAccessor accessor)
            : base(accessor)
        {
            Accessor = accessor.CastTo<IAccessor, TAccessor>(nameof(accessor));
        }


        /// <summary>
        /// 访问器。
        /// </summary>
        /// <value>返回 <typeparamref name="TAccessor"/>。</value>
        public new TAccessor Accessor { get; }


        /// <summary>
        /// 审计查询。
        /// </summary>
        public IQueryable<TAudit> Audits
            => Accessor.Audits;

        /// <summary>
        /// 审计属性查询。
        /// </summary>
        public IQueryable<TAuditProperty> AuditProperties
            => Accessor.AuditProperties;

        /// <summary>
        /// 迁移查询。
        /// </summary>
        public IQueryable<TMigration> Migrations
            => Accessor.Migrations;

        /// <summary>
        /// 实体查询。
        /// </summary>
        public IQueryable<TTabulation> Tabulations
            => Accessor.Tabulations;

        /// <summary>
        /// 租户查询。
        /// </summary>
        public IQueryable<TTenant> Tenants
            => Accessor.Tenants;


        #region IAuditStore

        /// <summary>
        /// 异步查找审计。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TAudit"/> 的异步操作。</returns>
        public virtual ValueTask<TAudit> FindAuditAsync(CancellationToken cancellationToken, params object[] keyValues)
            => Accessor.Audits.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 异步获取分页审计集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TAudit}"/> 的异步操作。</returns>
        public virtual ValueTask<IPageable<TAudit>> GetPagingAuditsAsync(int index, int size,
            Func<IQueryable<TAudit>, IQueryable<TAudit>> queryFactory = null, CancellationToken cancellationToken = default)
        {
			var query = queryFactory?.Invoke(Audits.AsNoTracking()) ?? Audits.AsNoTracking();
            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }


        /// <summary>
        /// 异步查找审计属性。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TAuditProperty"/> 的异步操作。</returns>
        public virtual ValueTask<TAuditProperty> FindAuditPropertyAsync(CancellationToken cancellationToken, params object[] keyValues)
            => Accessor.AuditProperties.FindAsync(keyValues, cancellationToken);

        #endregion


        #region IMigrationStore

        /// <summary>
        /// 异步查找迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TMigration"/> 的异步操作。</returns>
        public virtual ValueTask<TMigration> FindMigrationAsync(CancellationToken cancellationToken, params object[] keyValues)
            => Accessor.Migrations.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 异步获取分页迁移集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TMigration}"/> 的异步操作。</returns>
        public virtual ValueTask<IPageable<TMigration>> GetPagingMigrationsAsync(int index, int size,
            Func<IQueryable<TMigration>, IQueryable<TMigration>> queryFactory = null, CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Migrations.AsNoTracking()) ?? Migrations.AsNoTracking();
            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }

        #endregion


        #region ITabulationStore

        /// <summary>
        /// 异步查找表格。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TTabulation"/> 的异步操作。</returns>
        public virtual ValueTask<TTabulation> FindTabulationAsync(CancellationToken cancellationToken, params object[] keyValues)
            => Accessor.Tabulations.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 异步获取分页表格集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TEntity}"/> 的异步操作。</returns>
        public virtual ValueTask<IPageable<TTabulation>> GetPagingTabulationsAsync(int index, int size,
            Func<IQueryable<TTabulation>, IQueryable<TTabulation>> queryFactory = null, CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Tabulations.AsNoTracking()) ?? Tabulations.AsNoTracking();
            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }

        #endregion


        #region ITenantStore

        /// <summary>
        /// 异步查找指定租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TTenant"/> 的异步操作。</returns>
        public virtual ValueTask<TTenant> FindTenantAsync(CancellationToken cancellationToken,
            params object[] keyValues)
            => Accessor.Tenants.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 异步获取所有分页租户集合。
        /// </summary>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IReadOnlyList{TTenant}"/> 的异步操作。</returns>
        public virtual ValueTask<IReadOnlyList<TTenant>> GetAllTenantsAsync(
            Func<IQueryable<TTenant>, IQueryable<TTenant>> queryFactory = null,
            CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Tenants.AsNoTracking()) ?? Tenants.AsNoTracking();
            return cancellationToken.RunOrCancelValueAsync(() => (IReadOnlyList<TTenant>)query.ToList());
        }

        /// <summary>
        /// 异步获取分页租户集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTenant}"/> 的异步操作。</returns>
        public virtual ValueTask<IPageable<TTenant>> GetPagingTenantsAsync(int index, int size,
            Func<IQueryable<TTenant>, IQueryable<TTenant>> queryFactory = null,
            CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Tenants.AsNoTracking()) ?? Tenants.AsNoTracking();
            return query.AsDescendingPagingByIndexAsync(index, size, cancellationToken);
        }

        #endregion

    }
}
