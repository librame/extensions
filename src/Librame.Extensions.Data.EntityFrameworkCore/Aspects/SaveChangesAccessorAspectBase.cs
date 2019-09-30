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
    /// 保存变化访问器截面基类（通常用于前置保存变化操作）。
    /// </summary>
    public class SaveChangesAccessorAspectBase : AccessorAspectBase, ISaveChangesAccessorAspect
    {
        /// <summary>
        /// 构造一个 <see cref="SaveChangesAccessorAspectBase"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected SaveChangesAccessorAspectBase(IClockService clock, IStoreIdentifier identifier,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            Clock = clock.NotNull(nameof(clock));
            Identifier = identifier.NotNull(nameof(identifier));
        }


        /// <summary>
        /// 时钟服务。
        /// </summary>
        public IClockService Clock { get; }

        /// <summary>
        /// 标识符。
        /// </summary>
        public IStoreIdentifier Identifier { get; }


        /// <summary>
        /// 后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public override void Postprocess(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                PostprocessCore(dbContextAccessor);

            // 取消需要保存变化操作
            RequiredSaveChanges = false;
        }


        /// <summary>
        /// 异步后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public override Task PostprocessAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor dbContextAccessor)
                return PostprocessCoreAsync(dbContextAccessor, cancellationToken);

            // 取消需要保存变化操作
            RequiredSaveChanges = false;
            return Task.CompletedTask;
        }
    }
}
