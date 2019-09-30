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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 迁移访问器截面基类（通常用于后置保存变化操作）。
    /// </summary>
    public class MigrateAccessorAspectBase : AccessorAspectBase, IMigrateAccessorAspect
    {
        /// <summary>
        /// 构造一个 <see cref="MigrateAccessorAspectBase"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected MigrateAccessorAspectBase(IClockService clock, IStoreIdentifier identifier,
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
    }
}
