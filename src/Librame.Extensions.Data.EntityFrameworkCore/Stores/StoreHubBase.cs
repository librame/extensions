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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储中心基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class StoreHubBase<TAccessor> : StoreHubBase<TAccessor, Audit, Tenant>, IStoreHub<TAccessor>
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
        /// 确保审计数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
        protected override DbSet<Audit> EnsureAudits()
        {
            return Accessor.Audits;
        }

        /// <summary>
        /// 确保租户数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
        protected override DbSet<Tenant> EnsureTenants()
        {
            return Accessor.Tenants;
        }
    }


    /// <summary>
    /// 存储中心基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public class StoreHubBase<TAccessor, TAudit, TTenant> : AbstractStore<TAccessor>, IStoreHub<TAccessor, TAudit, TTenant>
        where TAccessor : DbContextAccessor
        where TAudit : Audit
        where TTenant : Tenant
    {
        /// <summary>
        /// 构造一个 <see cref="StoreHubBase{TAccessor, TAudit, TTenant}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{TAccessor}"/>。</param>
        public StoreHubBase(IAccessor accessor, IStoreInitializer<TAccessor> initializer)
            : base(accessor)
        {
            Initializer = initializer.NotNull(nameof(initializer));
        }

        /// <summary>
        /// 构造一个 <see cref="StoreHubBase{TAccessor, TAudit, TTenant}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{TAccessor}"/>。</param>
        public StoreHubBase(TAccessor accessor, IStoreInitializer<TAccessor> initializer)
            : base(accessor)
        {
            Initializer = initializer.NotNull(nameof(initializer));
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
        /// 租户查询。
        /// </summary>
        public IQueryable<TTenant> Tenants
            => EnsureTenants();


        /// <summary>
        /// 确保审计数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
        protected virtual DbSet<TAudit> EnsureAudits()
        {
            return Accessor.Set<TAudit>();
        }

        /// <summary>
        /// 确保租户数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
        protected virtual DbSet<TTenant> EnsureTenants()
        {
            return Accessor.Set<TTenant>();
        }


        #region Audits

        /// <summary>
        /// 异步查找审计。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TAudit"/> 的异步操作。</returns>
        public virtual Task<TAudit> FindAuditAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return EnsureAudits().FindAsync(keyValues, cancellationToken);
        }

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


        #region Tenants

        /// <summary>
        /// 建立租户唯一表达式。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回查询表达式。</returns>
        protected Expression<Func<TTenant, bool>> BuildTenantUniqueExpression(string name, string host)
        {
            name.NotNullOrEmpty(nameof(name));
            host.NotNullOrEmpty(nameof(host));

            // Name and Host is unique index
            return p => p.Name == name && p.Host == host;
        }

        /// <summary>
        /// 异步包含指定租户。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        public virtual Task<bool> ContainTenantAsync(string name, string host, CancellationToken cancellationToken = default)
        {
            var predicate = BuildTenantUniqueExpression(name, host);

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
            var predicate = BuildTenantUniqueExpression(name, host);

            return Tenants.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        /// <summary>
        /// 异步查找指定租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TTenant"/> 的异步操作。</returns>
        public virtual Task<TTenant> FindTenantAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return EnsureTenants().FindAsync(keyValues, cancellationToken);
        }

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
