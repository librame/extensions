#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Core.Services;
    using Data.Accessors;
    using Data.Stores;
    using Data.ValueGenerators;

    /// <summary>
    /// 数据库上下文访问器截面接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public interface IAccessorAspect<TGenId, TCreatedBy> : ISortableService
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock { get; }

        /// <summary>
        /// 存储标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IDataStoreIdentifierGenerator{TGenId}"/>。</value>
        public IDataStoreIdentifierGenerator<TGenId> IdentifierGenerator { get; }

        /// <summary>
        /// 创建者默认值生成器。
        /// </summary>
        public IDefaultValueGenerator<TCreatedBy> CreatedByGenerator { get; }

        /// <summary>
        /// 启用截面。
        /// </summary>
        bool Enabled { get; }


        /// <summary>
        /// 前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        void PreProcess(IAccessor accessor);

        /// <summary>
        /// 异步前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task PreProcessAsync(IAccessor accessor, CancellationToken cancellationToken = default);


        /// <summary>
        /// 后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        void PostProcess(IAccessor accessor);

        /// <summary>
        /// 异步后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task PostProcessAsync(IAccessor accessor, CancellationToken cancellationToken = default);
    }
}
