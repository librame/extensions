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

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Data.Stores;

    /// <summary>
    /// 数据构建器接口。
    /// </summary>
    public interface IDataBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 数据库设计时类型。
        /// </summary>
        Type DatabaseDesignTimeType { get; }


        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="THub">指定实现 <see cref="IStoreHub{TGenId, TIncremId}"/> 或 <see cref="IStoreHub{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/> 接口的存储中心类型。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddStoreHub<THub>()
            where THub : class, IStoreHub;


        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IStoreIdentifierGenerator{TGenId}"/> 接口的存储标识符类型，推荐从 <see cref="AbstractStoreIdentifierGenerator{TGenId}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddStoreIdentifierGenerator<TGenerator>()
            where TGenerator : class, IStoreIdentifierGenerator;

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定实现 <see cref="IStoreInitializer{TGenId}"/> 接口的存储初始化器类型，推荐从 <see cref="AbstractStoreInitializer{TGenId}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddStoreInitializer<TInitializer>()
            where TInitializer : class, IStoreInitializer;
    }
}
