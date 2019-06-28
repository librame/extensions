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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象基础存储。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public abstract class AbstractBaseStore<TAccessor> : AbstractBaseStore<TAccessor, BaseAudit, BaseTenant, float, DataStatus>,
        IBaseStore<TAccessor>
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBaseStore{TAccessor}"/> 实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public AbstractBaseStore(IAccessor accessor)
            : base(accessor)
        {
        }


        /// <summary>
        /// 异步删除租户。
        /// </summary>
        /// <param name="tenant">给定的 <see cref="BaseTenant"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        public override Task<EntityResult> DeleteAsync(BaseTenant tenant, CancellationToken cancellationToken = default)
        {
            // BaseTenant 支持逻辑删除
            return Accessor.LogicDeleteAsync(cancellationToken, tenant);
        }
    }


    /// <summary>
    /// 抽象基础存储。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractBaseStore<TAccessor, TAudit, TTenant, TRank, TStatus> : AbstractStore<TAccessor>,
        IBaseStore<TAccessor, TAudit, TTenant>
        where TAccessor : IAccessor
        where TAudit : BaseAudit
        where TTenant : BaseTenant<TRank, TStatus>
        where TRank : struct
        where TStatus : struct
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore"/> 实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public AbstractBaseStore(IAccessor accessor)
            : base(accessor)
        {
        }


        /// <summary>
        /// 获取已释放类型。
        /// </summary>
        /// <returns>返回 <see cref="Type"/>。</returns>
        protected override Type GetDisposableType()
        {
            return GetType();
        }


        #region Audits

        /// <summary>
        /// 异步获取分页审计集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="orderedFactory">给定的排序工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TAudit}"/> 的异步操作。</returns>
        public virtual Task<IPageable<TAudit>> GetPagingAuditsAsync(int index, int size,
            Func<IQueryable<TAudit>, IOrderedQueryable<TAudit>> orderedFactory,
            CancellationToken cancellationToken = default)
        {
            return Accessor.QueryPagingAsync(orderedFactory,
                descr => descr.ComputeByIndex(index, size),
                cancellationToken);
        }

        #endregion


        #region Tenants

        /// <summary>
        /// 异步获取分页租户集合。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <param name="orderedFactory">给定的排序工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPageable{TAudit}"/> 的异步操作。</returns>
        public virtual Task<IPageable<TTenant>> GetPagingTenantsAsync(int index, int size,
            Func<IQueryable<TTenant>, IOrderedQueryable<TTenant>> orderedFactory,
            CancellationToken cancellationToken = default)
        {
            return Accessor.QueryPagingAsync(orderedFactory,
                descr => descr.ComputeByIndex(index, size),
                cancellationToken);
        }

        /// <summary>
        /// 异步创建租户。
        /// </summary>
        /// <param name="tenant">给定的 <typeparamref name="TTenant"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        public virtual Task<EntityResult> CreateAsync(TTenant tenant, CancellationToken cancellationToken = default)
        {
            return Accessor.CreateAsync(cancellationToken, tenant);
        }

        /// <summary>
        /// 异步更新租户。
        /// </summary>
        /// <param name="tenant">给定的 <typeparamref name="TTenant"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        public virtual Task<EntityResult> UpdateAsync(TTenant tenant, CancellationToken cancellationToken = default)
        {
            return Accessor.UpdateAsync(cancellationToken, tenant);
        }

        /// <summary>
        /// 异步删除租户。
        /// </summary>
        /// <param name="tenant">给定的 <typeparamref name="TTenant"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        public virtual Task<EntityResult> DeleteAsync(TTenant tenant, CancellationToken cancellationToken = default)
        {
            return Accessor.DeleteAsync(cancellationToken, tenant);
        }

        #endregion

    }
}
