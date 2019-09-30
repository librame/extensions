#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
    public interface IStoreInitializer<TAccessor, TIdentifier> : IStoreInitializer<TAccessor>
        where TAccessor : IAccessor
        where TIdentifier : IStoreIdentifier
    {
        /// <summary>
        /// 标识符。
        /// </summary>
        new TIdentifier Identifier { get; }
    }


    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IStoreInitializer<TAccessor> : IStoreInitializer
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor, TAudit, TTable, TMigration, TTenant}"/>。</param>
        void Initialize<TAudit, TEntity, TMigration, TTenant>(IStoreHub<TAccessor, TAudit, TEntity, TMigration, TTenant> stores)
            where TAudit : DataAudit
            where TEntity : DataEntity
            where TMigration : DataMigration
            where TTenant : DataTenant;
    }


    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    public interface IStoreInitializer
    {
        /// <summary>
        /// 时钟。
        /// </summary>
        IClockService Clock { get; }

        /// <summary>
        /// 标识符。
        /// </summary>
        IStoreIdentifier Identifier { get; }

        /// <summary>
        /// 日志工厂。
        /// </summary>
        ILoggerFactory LoggerFactory { get; }


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
