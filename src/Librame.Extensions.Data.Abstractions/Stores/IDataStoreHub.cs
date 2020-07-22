#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    using Accessors;

    /// <summary>
    /// 数据存储中心接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface IDataStoreHub<TAccessor, TAudit, TAuditProperty, TMigration, TTabulation, TTenant> : IStoreHub,
        IAuditStore<TAudit, TAuditProperty>,
        IMigrationStore<TMigration>,
        ITabulationStore<TTabulation>,
        ITenantStore<TTenant>
        where TAccessor : class, IAccessor
        where TAudit : class
        where TAuditProperty : class
        where TMigration : class
        where TTabulation : class
        where TTenant : class
    {
        /// <summary>
        /// 访问器。
        /// </summary>
        /// <value>返回 <typeparamref name="TAccessor"/>。</value>
        new TAccessor Accessor { get; }
    }
}
