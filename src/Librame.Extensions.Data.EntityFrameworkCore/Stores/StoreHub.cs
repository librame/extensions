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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储中心。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class StoreHub<TAccessor> : StoreHub<TAccessor, DataAudit<string>, DataAuditProperty<int, string>, DataEntity<string>, DataMigration<string>, DataTenant<string>, string, int>
        , IStoreHub<TAccessor>
        where TAccessor : IDbContextAccessor<DataAudit<string>, DataAuditProperty<int, string>, DataEntity<string>, DataMigration<string>, DataTenant<string>>
    {
        /// <summary>
        /// 构造一个存储中心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer"/>。</param>
        public StoreHub(IStoreInitializer initializer, IAccessor accessor)
            : base(initializer, accessor)
        {
        }

        /// <summary>
        /// 构造一个存储中心。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer"/>。</param>
        public StoreHub(IStoreInitializer initializer, TAccessor accessor)
            : base(initializer, accessor)
        {
        }

    }


    /// <summary>
    /// 存储中心。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class StoreHub<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> : AbstractStoreHub
        , IStoreHub<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        where TAccessor : IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty: DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 构造一个存储中心。
        /// </summary>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer"/>。</param>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public StoreHub(IStoreInitializer initializer, IAccessor accessor)
            : base(initializer, accessor)
        {
            Accessor = accessor.CastTo<IAccessor, TAccessor>(nameof(accessor));

            InitializeStoreHub();
        }

        /// <summary>
        /// 构造一个存储中心。
        /// </summary>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer"/>。</param>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        public StoreHub(IStoreInitializer initializer, TAccessor accessor)
            : base(initializer, accessor)
        {
            Accessor = accessor;

            InitializeStoreHub();
        }


        /// <summary>
        /// 初始化存储中心。
        /// </summary>
        protected virtual void InitializeStoreHub()
        {
            if (Accessor.BuilderOptions.Stores.InitializationEnabled
                && Initializer is StoreInitializer storeInitializer)
            {
                storeInitializer.Initialize(this);
            }
        }


        /// <summary>
        /// 数据访问器。
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
        /// 实体查询。
        /// </summary>
        public IQueryable<TEntity> Entities
            => Accessor.Entities;

        /// <summary>
        /// 迁移查询。
        /// </summary>
        public IQueryable<TMigration> Migrations
            => Accessor.Migrations;

        /// <summary>
        /// 租户查询。
        /// </summary>
        public IQueryable<TTenant> Tenants
            => Accessor.Tenants;


        #region IDataAuditStore

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
			var query = queryFactory?.Invoke(Audits) ?? Audits;
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


        #region IDataEntityStore

        /// <summary>
        /// 异步查找实体。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的异步操作。</returns>
        public virtual ValueTask<TEntity> FindEntityAsync(CancellationToken cancellationToken, params object[] keyValues)
            => Accessor.Entities.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 异步获取分页实体集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTable}"/> 的异步操作。</returns>
        public virtual ValueTask<IPageable<TEntity>> GetPagingEntitiesAsync(int index, int size,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFactory = null, CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Entities) ?? Entities;
            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }

        #endregion


        #region IDataMigrationStore

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
        /// <returns>返回一个包含 <see cref="IPageable{TTable}"/> 的异步操作。</returns>
        public virtual ValueTask<IPageable<TMigration>> GetPagingMigrationsAsync(int index, int size,
            Func<IQueryable<TMigration>, IQueryable<TMigration>> queryFactory = null, CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Migrations) ?? Migrations;
            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }

        #endregion


        #region IDataTenantStore

        /// <summary>
        /// 异步包含指定租户。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        public virtual Task<bool> ContainTenantAsync(string name, string host, CancellationToken cancellationToken = default)
        {
            var predicate = StoreExpression.GetTenantUniqueIndexExpression<TTenant, TGenId>(name, host);
            return Accessor.Tenants.ExistsAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 异步获取指定租户。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TTenant"/> 的异步操作。</returns>
        public virtual Task<TTenant> GetTenantAsync(string name, string host, CancellationToken cancellationToken = default)
        {
            var predicate = StoreExpression.GetTenantUniqueIndexExpression<TTenant, TGenId>(name, host);
            return Tenants.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 异步查找指定租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TTenant"/> 的异步操作。</returns>
        public virtual ValueTask<TTenant> FindTenantAsync(CancellationToken cancellationToken, params object[] keyValues)
            => Accessor.Tenants.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 异步获取所有分页租户集合。
        /// </summary>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="List{TTenant}"/> 的异步操作。</returns>
        public virtual Task<List<TTenant>> GetAllTenantsAsync(Func<IQueryable<TTenant>, IQueryable<TTenant>> queryFactory = null,
            CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Tenants) ?? Tenants;
            return query.ToListAsync(cancellationToken);
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
            Func<IQueryable<TTenant>, IQueryable<TTenant>> queryFactory = null, CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Tenants) ?? Tenants;
            return query.AsDescendingPagingByIndexAsync(index, size, cancellationToken);
        }


        /// <summary>
        /// 尝试异步创建租户集合。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        public virtual Task<OperationResult> TryCreateAsync(CancellationToken cancellationToken, params TTenant[] tenants)
            => Accessor.Tenants.TryCreateAsync(cancellationToken, tenants);

        /// <summary>
        /// 尝试创建租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public virtual OperationResult TryCreate(params TTenant[] tenants)
            => Accessor.Tenants.TryCreate(tenants);

        /// <summary>
        /// 尝试更新租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public virtual OperationResult TryUpdate(params TTenant[] tenants)
            => Accessor.Tenants.TryUpdate(tenants);

        /// <summary>
        /// 尝试删除租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public virtual OperationResult TryDelete(params TTenant[] tenants)
            => Accessor.Tenants.TryLogicDelete(tenants);

        #endregion

    }
}
