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
    public class StoreHubBase<TAccessor> : StoreHubBase<TAccessor, DataAudit, DataTenant, float, DataStatus>, IStoreHub<TAccessor>
        where TAccessor : DbContextAccessor
    {
        /// <summary>
        /// 构造一个抽象基础存储实例。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        public StoreHubBase(TAccessor accessor)
            : base(accessor)
        {
        }

        /// <summary>
        /// 构造一个抽象基础存储实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public StoreHubBase(IAccessor accessor)
            : base(accessor)
        {
        }


        /// <summary>
        /// 确保审计数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
        protected override DbSet<DataAudit> EnsureAudits()
        {
            return Accessor.BaseAudits;
        }

        /// <summary>
        /// 确保租户数据集。
        /// </summary>
        /// <returns>返回 <see cref="DbSet{TEntity}"/>。</returns>
        protected override DbSet<DataTenant> EnsureTenants()
        {
            return Accessor.BaseTenants;
        }


        #region Tenants

        /// <summary>
        /// 异步获取分页租户集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTenant}"/> 的异步操作。</returns>
        public override Task<IPageable<DataTenant>> GetPagingTenantsAsync(int index, int size, CancellationToken cancellationToken = default)
        {
            return EnsureTenants().AsDescendingPagingByIndexAsync(index, size, cancellationToken);
        }


        /// <summary>
        /// 尝试逻辑删除租户集合。
        /// </summary>
        /// <param name="tenants">给定的 <see cref="DataTenant"/> 数组。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public override EntityResult TryDelete(params DataTenant[] tenants)
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
        where TAudit : DataAudit
        where TTenant : DataTenant<TRank, TStatus>
        where TRank : struct
        where TStatus : struct
    {
        /// <summary>
        /// 构造一个存储中心基类实例。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        public StoreHubBase(TAccessor accessor)
            : base(accessor)
        {
        }

        /// <summary>
        /// 构造一个存储中心基类实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public StoreHubBase(IAccessor accessor)
            : base(accessor)
        {
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
            return EnsureAudits().FindAsync(keyValues, keyValues);
        }

        /// <summary>
        /// 异步获取分页审计集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TAudit}"/> 的异步操作。</returns>
        public virtual Task<IPageable<TAudit>> GetPagingAuditsAsync(int index, int size, CancellationToken cancellationToken = default)
        {
            return EnsureAudits().AsPagingByIndexAsync(q => q.OrderByDescending(k => k.CreatedTime),
                index, size, cancellationToken);
        }

        #endregion


        #region Tenants

        /// <summary>
        /// 异步查找租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="keyValues">给定的键值对数组或标识。</param>
        /// <returns>返回一个包含 <typeparamref name="TTenant"/> 的异步操作。</returns>
        public virtual Task<TTenant> FindTenantAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return EnsureTenants().FindAsync(keyValues, keyValues);
        }

        /// <summary>
        /// 异步获取租户。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TTenant"/> 的异步操作。</returns>
        public virtual Task<TTenant> GetTenantAsync(string name, string host, CancellationToken cancellationToken = default)
        {
            return EnsureTenants().FirstOrDefaultAsync(p => p.Name == name && p.Host == host, cancellationToken);
        }

        /// <summary>
        /// 异步获取所有分页租户集合。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="List{TTenant}"/> 的异步操作。</returns>
        public virtual Task<List<TTenant>> GetAllTenantsAsync(CancellationToken cancellationToken = default)
        {
            return EnsureTenants().ToListAsync();
        }

        /// <summary>
        /// 异步获取分页租户集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TTenant}"/> 的异步操作。</returns>
        public virtual Task<IPageable<TTenant>> GetPagingTenantsAsync(int index, int size, CancellationToken cancellationToken = default)
        {
            return EnsureTenants().AsPagingByIndexAsync(q => q.OrderByDescending(k => k.Id),
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
