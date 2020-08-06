#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;
    using Data.Accessors;
    using Data.Validators;

    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public abstract class AbstractStoreInitializer<TAccessor> : AbstractStoreInitializer
        where TAccessor : class, IAccessor
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer"/>。
        /// </summary>
        /// <param name="validator">给定的 <see cref="IDataInitializationValidator"/>。</param>
        /// <param name="generator">给定的 <see cref="IStoreIdentificationGenerator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreInitializer(IDataInitializationValidator validator,
            IStoreIdentificationGenerator generator, ILoggerFactory loggerFactory)
            : base(validator, generator, loggerFactory)
        {
        }


        /// <summary>
        /// 当前访问器（在初始化核心 <see cref="InitializeCore(IAccessor)"/> 后方可使用）。
        /// </summary>
        /// <value>返回 <typeparamref name="TAccessor"/>。</value>
        protected TAccessor Accessor { get; private set; }


        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected override void InitializeCore(IAccessor accessor)
        {
            if (accessor is TAccessor dataAccessor)
            {
                Accessor = dataAccessor;

                InitializeStores();
            }
        }

        /// <summary>
        /// 异步初始化核心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected override Task InitializeCoreAsync(IAccessor accessor, CancellationToken cancellationToken)
        {
            if (accessor is TAccessor dataAccessor)
            {
                Accessor = dataAccessor;

                return InitializeStoresAsync(cancellationToken);
            }

            return Task.CompletedTask;
        }


        /// <summary>
        /// 初始化存储集合。
        /// </summary>
        protected abstract void InitializeStores();

        /// <summary>
        /// 异步初始化存储集合。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected abstract Task InitializeStoresAsync(CancellationToken cancellationToken);

    }


    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    public abstract class AbstractStoreInitializer : AbstractService, IStoreInitializer
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer"/>。
        /// </summary>
        /// <param name="validator">给定的 <see cref="IDataInitializationValidator"/>。</param>
        /// <param name="generator">给定的 <see cref="IStoreIdentificationGenerator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreInitializer(IDataInitializationValidator validator,
            IStoreIdentificationGenerator generator, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Validator = validator.NotNull(nameof(validator));
            Generator = generator.NotNull(nameof(generator));
        }


        /// <summary>
        /// 初始化验证器。
        /// </summary>
        /// <value>返回 <see cref="IDataInitializationValidator"/>。</value>
        public IDataInitializationValidator Validator { get; }

        /// <summary>
        /// 标识生成器。
        /// </summary>
        /// <value>返回 <see cref="IStoreIdentificationGenerator"/>。</value>
        public IStoreIdentificationGenerator Generator { get; }

        /// <summary>
        /// 时钟。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock
            => Generator.Clock;


        /// <summary>
        /// 初始化访问器。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public virtual void Initialize(IAccessor accessor)
        {
            if (!Validator.IsInitialized(accessor))
            {
                InitializeCore(accessor);

                // 设置已初始化标识改由底层 BatchExecutorReplacement 实现
                //Validator.SetInitialized(accessor);
            }
        }

        /// <summary>
        /// 异步初始化访问器。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual async Task InitializeAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            if (!await Validator.IsInitializedAsync(accessor, cancellationToken).ConfigureAwait())
            {
                await InitializeCoreAsync(accessor, cancellationToken).ConfigureAwait();

                // 设置已初始化标识改由底层 BatchExecutorReplacement 实现
                //await Validator.SetInitializedAsync(accessor, cancellationToken).ConfigureAwait();
            }
        }


        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected abstract void InitializeCore(IAccessor accessor);

        /// <summary>
        /// 异步初始化核心。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected abstract Task InitializeCoreAsync(IAccessor accessor, CancellationToken cancellationToken);

    }
}
