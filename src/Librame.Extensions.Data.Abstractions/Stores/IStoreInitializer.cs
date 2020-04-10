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

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;

    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public interface IStoreInitializer<TGenId> : IStoreInitializer
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 存储标识符。
        /// </summary>
        IStoreIdentifier<TGenId> Identifier { get; }


        /// <summary>
        /// 初始化。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <param name="stores">给定的存储中心。</param>
        void Initialize<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TIncremId>
            (IStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> stores)
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TIncremId : IEquatable<TIncremId>;
    }


    /// <summary>
    /// 存储初始化器接口（主要用作标记）。
    /// </summary>
    public interface IStoreInitializer : IService
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        IClockService Clock { get; }


        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 需要保存变化。
        /// </summary>
        bool RequiredSaveChanges { get; }
    }
}
