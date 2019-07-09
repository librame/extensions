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
    internal class InternalCoreBuilder : AbstractBuilder<CoreBuilderOptions>, ICoreBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalCoreBuilder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <see cref="CoreBuilderOptions"/>。</param>
        public InternalCoreBuilder(IServiceCollection services, CoreBuilderOptions options)
            : base(services, options)
        {
            Services.AddSingleton<ICoreBuilder>(this);
            Services.AddSingleton(serviceProvider => (IBuilder)serviceProvider.GetRequiredService<ICoreBuilder>());
        }


        /// <summary>
        /// 初始化构建器。
        /// </summary>
        /// <param name="options">给定的 <see cref="CoreBuilderOptions"/>。</param>
        protected override void Initialize(CoreBuilderOptions options)
        {
            BuilderGlobalization.RegisterCultureInfos(options.CultureInfo, options.CultureUIInfo);

            if (options.EnableAutoRegistrationMediators)
                Services.AddAutoRegistrationMediators();

            if (options.EnableAutoRegistrationServices)
                Services.AddAutoRegistrationServices();
        }

    }
}
