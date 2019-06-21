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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象基础存储。
    /// </summary>
    public abstract class AbstractBaseStore : AbstractBaseStore<BaseAudit, BaseAuditProperty, BaseTenant, float, DataStatus>, IBaseStore
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBaseStore"/> 实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public AbstractBaseStore(IAccessor accessor)
            : base(accessor)
        {
        }
    }


    /// <summary>
    /// 抽象基础存储。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractBaseStore<TAudit, TAuditProperty, TTenant, TRank, TStatus> : AbstractStore,
        IBaseStore<TAudit, TAuditProperty, TTenant>
        where TAudit : BaseAudit
        where TAuditProperty : BaseAuditProperty
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

    }
}
