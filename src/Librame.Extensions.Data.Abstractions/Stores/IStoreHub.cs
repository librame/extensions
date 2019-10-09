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
    /// 存储中心接口（主要用于依赖注入的服务注册）。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IStoreHub<TAccessor> : IStoreHub<TAccessor, DataAudit<string>, DataAuditProperty<int, string>, DataEntity<string>, DataMigration<string>, DataTenant<string>, string, int>
        where TAccessor : IAccessor
    {
    }


    /// <summary>
    /// 存储中心接口（主要用于依赖注入的服务注册）。
    /// </summary>
    /// <typeparam name="TAccessor"></typeparam>
    /// <typeparam name="TAudit"></typeparam>
    /// <typeparam name="TAuditProperty"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TMigration"></typeparam>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TGenId"></typeparam>
    /// <typeparam name="TIncremId"></typeparam>
    public interface IStoreHub<TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> : IStore
        where TAccessor : IAccessor
        where TAudit : class
        where TAuditProperty : class
        where TEntity : class
        where TMigration : class
        where TTenant : class
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 数据访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        new TAccessor Accessor { get; }
    }


    /// <summary>
    /// 存储中心接口。
    /// </summary>
    public interface IStoreHub : IStore
    {
        /// <summary>
        /// 初始化器。
        /// </summary>
        IStoreInitializer Initializer { get; }
    }
}
