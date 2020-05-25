#region License

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
    /// 存储中心。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class StoreHub<TGenId, TIncremId> : StoreHub<DataAudit<TGenId>, DataAuditProperty<TIncremId, TGenId>, DataEntity<TGenId>, DataMigration<TGenId>, DataTenant<TGenId>, TGenId, TIncremId>, IStoreHub<TGenId, TIncremId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 构造一个存储中心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{Guid}"/>。</param>
        public StoreHub(IStoreInitializer<TGenId> initializer, IAccessor accessor)
            : base(initializer, accessor)
        {
        }
    }


    /// <summary>
    /// 存储中心。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class StoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        : AbstractStore, IStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty: DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        private readonly IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> _dbContextAccessor;


        /// <summary>
        /// 构造一个存储中心。
        /// </summary>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{TGenId}"/>。</param>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public StoreHub(IStoreInitializer<TGenId> initializer, IAccessor accessor)
            : base(accessor)
        {
            Initializer = initializer.NotNull(nameof(initializer));
            _dbContextAccessor = accessor.CastTo<IAccessor,
                IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant>>(nameof(accessor));

            InitializeInitializer();
        }


        /// <summary>
        /// 初始化初始化器。
        /// </summary>
        private void InitializeInitializer()
        {
            if (_dbContextAccessor.Dependency.Options.Stores.UseInitializer
                && !Initializer.IsInitialized)
            {
                Initializer.Initialize(this);
            }
        }


        /// <summary>
        /// 存储初始化器。
        /// </summary>
        /// <value>返回 <see cref="IStoreInitializer{TGenId}"/>。</value>
        public IStoreInitializer<TGenId> Initializer { get; }


        /// <summary>
        /// 审计查询。
        /// </summary>
        public IQueryable<TAudit> Audits
            => _dbContextAccessor.Audits;

        /// <summary>
        /// 审计属性查询。
        /// </summary>
        public IQueryable<TAuditProperty> AuditProperties
            => _dbContextAccessor.AuditProperties;

        /// <summary>
        /// 实体查询。
        /// </summary>
        public IQueryable<TEntity> Entities
            => _dbContextAccessor.Entities;

        /// <summary>
        /// 迁移查询。
        /// </summary>
        public IQueryable<TMigration> Migrations
            => _dbContextAccessor.Migrations;

        /// <summary>
        /// 租户查询。
        /// </summary>
        public IQueryable<TTenant> Tenants
            => _dbContextAccessor.Tenants;


        #region IAuditStore

        /// <summary>
        /// 异步查找审计。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TAudit"/> 的异步操作。</returns>
        public virtual ValueTask<TAudit> FindAuditAsync(CancellationToken cancellationToken, params object[] keyValues)
            => _dbContextAccessor.Audits.FindAsync(keyValues, cancellationToken);

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
            => _dbContextAccessor.AuditProperties.FindAsync(keyValues, cancellationToken);

        #endregion


        #region IEntityStore

        /// <summary>
        /// 异步查找实体。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的异步操作。</returns>
        public virtual ValueTask<TEntity> FindEntityAsync(CancellationToken cancellationToken, params object[] keyValues)
            => _dbContextAccessor.Entities.FindAsync(keyValues, cancellationToken);

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


        #region IMigrationStore

        /// <summary>
        /// 异步查找迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TMigration"/> 的异步操作。</returns>
        public virtual ValueTask<TMigration> FindMigrationAsync(CancellationToken cancellationToken, params object[] keyValues)
            => _dbContextAccessor.Migrations.FindAsync(keyValues, cancellationToken);

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


        #region ITenantStore

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
            return _dbContextAccessor.Tenants.ExistsAsync(predicate, lookupLocal: true, cancellationToken);
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
            => _dbContextAccessor.Tenants.FindAsync(keyValues, cancellationToken);

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
            => _dbContextAccessor.Tenants.TryCreateAsync(cancellationToken, tenants);

        /// <summary>
        /// 尝试创建租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public virtual OperationResult TryCreate(params TTenant[] tenants)
            => _dbContextAccessor.Tenants.TryCreate(tenants);

        /// <summary>
        /// 尝试更新租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public virtual OperationResult TryUpdate(params TTenant[] tenants)
            => _dbContextAccessor.Tenants.TryUpdate(tenants);

        /// <summary>
        /// 尝试删除租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public virtual OperationResult TryDelete(params TTenant[] tenants)
            => _dbContextAccessor.Tenants.TryLogicDelete(tenants);

        #endregion

    }
}
