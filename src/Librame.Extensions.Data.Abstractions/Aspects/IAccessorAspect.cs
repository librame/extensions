#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Core.Services;
    using Data.Accessors;
    using Data.Stores;

    /// <summary>
    /// 数据库上下文访问器截面接口。
    /// </summary>
    public interface IAccessorAspect : ISortableService
    {
        /// <summary>
        /// 标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IStoreIdentifierGenerator"/>。</value>
        public IStoreIdentifierGenerator IdentifierGenerator { get; }

        /// <summary>
        /// 时钟。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock { get; }

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
