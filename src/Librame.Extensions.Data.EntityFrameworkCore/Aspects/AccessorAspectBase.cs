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
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 访问器截面基类。
    /// </summary>
    public class AccessorAspectBase : AbstractExtensionBuilderService<DataBuilderOptions>, IAccessorAspect
    {
        /// <summary>
        /// 构造一个 <see cref="AccessorAspectBase"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AccessorAspectBase(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        /// <summary>
        /// 启用截面。
        /// </summary>
        public virtual bool Enabled { get; }


        /// <summary>
        /// 需要保存变化。
        /// </summary>
        public bool RequiredSaveChanges { get; set; }


        #region Preprocess

        /// <summary>
        /// 前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public virtual void Preprocess(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                PreprocessCore(dbContextAccessor);
        }

        /// <summary>
        /// 前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        protected virtual void PreprocessCore(DbContextAccessor dbContextAccessor)
        {
        }


        /// <summary>
        /// 异步前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task PreprocessAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                return PreprocessCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual Task PreprocessCoreAsync(DbContextAccessor dbContextAccessor,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        #endregion


        #region Postprocess

        /// <summary>
        /// 后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public virtual void Postprocess(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                PostprocessCore(dbContextAccessor);
        }

        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        protected virtual void PostprocessCore(DbContextAccessor dbContextAccessor)
        {
        }


        /// <summary>
        /// 异步后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task PostprocessAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                return PostprocessCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual Task PostprocessCoreAsync(DbContextAccessor dbContextAccessor,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        #endregion

    }
}
