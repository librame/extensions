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
    /// 存储中心基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class StoreHubBase<TAccessor> : StoreHubBase<TAccessor, DataAudit, DataEntity, DataTenant>, IStoreHub<TAccessor>
        where TAccessor : DbContextAccessor
    {
        /// <summary>
        /// 构造一个 <see cref="StoreHubBase{TAccessor}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{TAccessor}"/>。</param>
        public StoreHubBase(IAccessor accessor, IStoreInitializer<TAccessor> initializer)
            : base(accessor, initializer)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="StoreHubBase{TAccessor}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{TAccessor}"/>。</param>
        public StoreHubBase(TAccessor accessor, IStoreInitializer<TAccessor> initializer)
            : base(accessor, initializer)
        {
        }


        /// <summary>
        /// 初始化存储中心。
        /// </summary>
        protected override void InitializeStoreHub()
        {
            if (Accessor.BuilderOptions.Stores.InitializationEnabled)
                Initializer.Initialize(this);
        }


        /// <summary>
        /// 确保审计数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{Audit}"/>。</returns>
        protected override DbSet<DataAudit> EnsureAudits()
            => Accessor.Audits;

        /// <summary>
        /// 确保实体表数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{Table}"/>。</returns>
        protected override DbSet<DataEntity> EnsureEntities()
            => Accessor.Entities;

        /// <summary>
        /// 确保租户数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{Tenant}"/>。</returns>
        protected override DbSet<DataTenant> EnsureTenants()
            => Accessor.Tenants;
    }


    /// <summary>
    /// 存储中心基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public class StoreHubBase<TAccessor, TAudit, TEntity, TTenant> : AbstractStore<TAccessor>, IStoreHub<TAccessor, TAudit, TEntity, TTenant>
        where TAccessor : DbContextAccessor
        where TAudit : DataAudit
        where TEntity : DataEntity
        where TTenant : DataTenant
    {
        /// <summary>
        /// 构造一个 <see cref="StoreHubBase{TAccessor, TAudit, TEntity, TTenant}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{TAccessor}"/>。</param>
        public StoreHubBase(IAccessor accessor, IStoreInitializer<TAccessor> initializer)
            : base(accessor)
        {
            Initializer = initializer.NotNull(nameof(initializer));

            InitializeStoreHub();
        }

        /// <summary>
        /// 构造一个 <see cref="StoreHubBase{TAccessor, TAudit, TEntity, TTenant}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{TAccessor}"/>。</param>
        public StoreHubBase(TAccessor accessor, IStoreInitializer<TAccessor> initializer)
            : base(accessor)
        {
            Initializer = initializer.NotNull(nameof(initializer));

            InitializeStoreHub();
        }


        /// <summary>
        /// 初始化存储中心。
        /// </summary>
        protected virtual void InitializeStoreHub()
        {
            if (Accessor.BuilderOptions.Stores.InitializationEnabled)
                Initializer.Initialize(this);
        }


        /// <summary>
        /// 初始化器。
        /// </summary>
        public IStoreInitializer<TAccessor> Initializer { get; }


        /// <summary>
        /// 审核查询。
        /// </summary>
        public IQueryable<TAudit> Audits
            => EnsureAudits();

        /// <summary>
        /// 实体表查询。
        /// </summary>
        public IQueryable<TEntity> Entities
            => EnsureEntities();

        /// <summary>
        /// 租户查询。
        /// </summary>
        public IQueryable<TTenant> Tenants
            => EnsureTenants();


        /// <summary>
        /// 确保审计数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
        protected virtual DbSet<TAudit> EnsureAudits()
            => Accessor.Set<TAudit>();

        /// <summary>
        /// 确保实体数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TTable}"/>。</returns>
        protected virtual DbSet<TEntity> EnsureEntities()
            => Accessor.Set<TEntity>();

        /// <summary>
        /// 确保租户数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
        protected virtual DbSet<TTenant> EnsureTenants()
            => Accessor.Set<TTenant>();


        #region Audits

        /// <summary>
        /// 异步查找审计。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TAudit"/> 的异步操作。</returns>
        public virtual Task<TAudit> FindAuditAsync(CancellationToken cancellationToken, params object[] keyValues)
            => EnsureAudits().FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 异步获取分页审计集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TAudit}"/> 的异步操作。</returns>
        public virtual Task<IPageable<TAudit>> GetPagingAuditsAsync(int index, int size,
            Func<IQueryable<TAudit>, IQueryable<TAudit>> queryFactory = null, CancellationToken cancellationToken = default)
        {
			var query = queryFactory?.Invoke(Audits) ?? Audits;
			
            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }

        #endregion


        #region Entities

        /// <summary>
        /// 异步查找实体。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的异步操作。</returns>
        public virtual Task<TEntity> FindEntityAsync(CancellationToken cancellationToken, params object[] keyValues)
            => EnsureEntities().FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// 异步获取分页实体集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTable}"/> 的异步操作。</returns>
        public virtual Task<IPageable<TEntity>> GetPagingEntitiesAsync(int index, int size,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFactory = null, CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(Entities) ?? Entities;
            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }

        #endregion


        #region Tenants

        /// <summary>
        /// 异步包含指定租户。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        public virtual Task<bool> ContainTenantAsync(string name, string host, CancellationToken cancellationToken = default)
        {
            var predicate = DataTenant.GetUniqueIndexExpression<TTenant>(name, host);
            return Tenants.AnyAsync(predicate, cancellationToken);
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
            var predicate = DataTenant.GetUniqueIndexExpression<TTenant>(name, host);
            return Tenants.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 异步查找指定租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TTenant"/> 的异步操作。</returns>
        public virtual Task<TTenant> FindTenantAsync(CancellationToken cancellationToken, params object[] keyValues)
            => EnsureTenants().FindAsync(keyValues, cancellationToken);

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
        public virtual Task<IPageable<TTenant>> GetPagingTenantsAsync(int index, int size,
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
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        public virtual Task<EntityResult> TryCreateAsync(CancellationToken cancellationToken, params TTenant[] tenants)
            => EnsureTenants().TryCreateAsync(cancellationToken, tenants);

        /// <summary>
        /// 尝试更新租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public virtual EntityResult TryUpdate(params TTenant[] tenants)
            => EnsureTenants().TryUpdate(tenants);

        /// <summary>
        /// 尝试删除租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <typeparamref name="TTenant"/> 数组。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public virtual EntityResult TryDelete(params TTenant[] tenants)
            => EnsureTenants().TryLogicDelete(tenants);

        #endregion

    }
}
