#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部核心构建器。
    /// </summary>
    internal class InternalCoreBuilder : AbstractExtensionBuilder, ICoreBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalCoreBuilder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        public InternalCoreBuilder(IServiceCollection services)
            : base(services)
        {
            Services.AddSingleton<ICoreBuilder>(this);
            Services.AddSingleton(sp => (IExtensionBuilder)sp.GetRequiredService<ICoreBuilder>());
        }


        /// <summary>
        /// 初始化构建器。
        /// </summary>
        /// <param name="options">给定的 <see cref="CoreBuilderOptions"/>。</param>
        protected override void Initialize(CoreBuilderOptions options)
        {
            AssemblyHelper.RegisterCultureInfos(options.CultureInfo, options.CultureUIInfo);
        }

    }
}
