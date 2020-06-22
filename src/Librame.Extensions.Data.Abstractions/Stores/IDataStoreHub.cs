﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 数据存储中心接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public interface IDataStoreHub<TGenId, TIncremId, TCreatedBy> : IDataStoreHub<DataAudit<TGenId, TCreatedBy>,
        DataAuditProperty<TIncremId, TGenId>, DataEntity<TGenId, TCreatedBy>,
        DataMigration<TGenId, TCreatedBy>, DataTenant<TGenId, TCreatedBy>, TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
    }


    /// <summary>
    /// 数据存储中心接口。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的数据实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public interface IDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId> : IStoreHub<TGenId>,
        IAuditStore<TAudit, TAuditProperty>, IEntityStore<TEntity>, IMigrationStore<TMigration>, ITenantStore<TTenant>
        where TAudit : class
        where TAuditProperty : class
        where TEntity : class
        where TMigration : class
        where TTenant : class
        where TGenId : IEquatable<TGenId>
    {
    }
}