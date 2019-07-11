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
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 存储中心基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class StoreHubBase<TAccessor> : StoreHubBase<TAccessor, Audit, Tenant, float, DataStatus>, IStoreHub<TAccessor>
        where TAccessor : DbContextAccessor
    {
        /// <summary>
        /// 构造一个存储中心基类实例（可用于容器构造）。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public StoreHubBase(IAccessor accessor)
            : base(accessor)
        {
            Initialize();
        }

        /// <summary>
        /// 构造一个存储中心基类实例（可用于手动构造）。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        protected StoreHubBase(TAccessor accessor)
            : base(accessor)
        {
            Initialize();
        }


        /// <summary>
        /// 初始化。
        /// </summary>
        protected virtual void Initialize()
        {
            // 绑定审计通知动作
            Accessor.AuditNotificationAction = notification =>
            {
                var mediator = GetRequiredService<IMediator>();
                mediator.Publish(notification);
            };
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


        #region Tenants

        /// <summary>
        /// 异步获取分页租户集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="queryFactory">给定的查询工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTenant}"/> 的异步操作。</returns>
        public override Task<IPageable<Tenant>> GetPagingTenantsAsync(int index, int size,
            Func<IQueryable<Tenant>, IQueryable<Tenant>> queryFactory = null, CancellationToken cancellationToken = default)
        {
            var query = queryFactory?.Invoke(EnsureTenants()) ?? EnsureTenants();

            return query.AsDescendingPagingByIndexAsync(index, size, cancellationToken);
        }


        /// <summary>
        /// 尝试逻辑删除租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <see cref="Tenant"/> 数组。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public override EntityResult TryDelete(params Tenant[] tenants)
            => EnsureTenants().TryLogicDelete(tenants);

        #endregion

    }


    /// <summary>
    /// 存储中心基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public class StoreHubBase<TAccessor, TAudit, TTenant, TRank, TStatus> : AbstractStore<TAccessor>, IStoreHub<TAccessor, TAudit, TTenant>
        where TAccessor : DbContext, IAccessor
        where TAudit : Audit
        where TTenant : Tenant<TRank, TStatus>
        where TRank : struct
        where TStatus : struct
    {
        /// <summary>
        /// 构造一个存储中心基类实例（可用于容器构造）。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public StoreHubBase(IAccessor accessor)
            : base(accessor)
        {
        }

        /// <summary>
        /// 构造一个存储中心基类实例（可用于手动构造）。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        protected StoreHubBase(TAccessor accessor)
            : base(accessor)
        {
        }


        /// <summary>
        /// 获取当前服务提供程序中的指定服务实例。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <returns>返回 <typeparamref name="TService"/>。</returns>
        public virtual TService GetRequiredService<TService>()
        {
            return Accessor.GetService<TService>();
        }

        /// <summary>
        /// 获取当前服务提供程序。
        /// </summary>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        public virtual IServiceProvider GetServiceProvider()
        {
            return Accessor.GetInfrastructure();
        }


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
			var query = queryFactory?.Invoke(EnsureAudits()) ?? EnsureAudits();
			
            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }

        #endregion


        #region Tenants

        /// <summary>
        /// 建立唯一租户表达式。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回查询表达式。</returns>
        protected virtual Expression<Func<TTenant, bool>> BuildUniqueTenantExpression(string name, string host)
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
            var predicate = BuildUniqueTenantExpression(name, host);

            return EnsureTenants().AnyAsync(predicate, cancellationToken);
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
            var predicate = BuildUniqueTenantExpression(name, host);

            return EnsureTenants().SingleOrDefaultAsync(predicate, cancellationToken);
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
            var query = queryFactory?.Invoke(EnsureTenants()) ?? EnsureTenants();

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
            var query = queryFactory?.Invoke(EnsureTenants()) ?? EnsureTenants();

            return query.AsPagingByIndexAsync(q => q.OrderByDescending(k => k.Id),
                index, size, cancellationToken);
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
            => EnsureTenants().TryDelete(tenants);

        #endregion

    }
}
