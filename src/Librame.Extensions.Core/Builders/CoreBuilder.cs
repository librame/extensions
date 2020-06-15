#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Core.Builders
{
    using Decorators;
    using Identifiers;
    using Localizers;
    using Mediators;
    using Options;
    using Services;

    /// <summary>
    /// 核心构建器。
    /// </summary>
    public class CoreBuilder : AbstractExtensionBuilder, ICoreBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="CoreBuilder"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependency">给定的 <see cref="CoreBuilderDependency"/>。</param>
        public CoreBuilder(IServiceCollection services, CoreBuilderDependency dependency)
            : base(services, dependency)
        {
            Services.AddSingleton<ICoreBuilder>(this);
            Services.AddSingleton(sp => (IExtensionBuilder)sp.GetRequiredService<ICoreBuilder>());

            AddInternalServices();
        }


        private void AddInternalServices()
        {
            // Decorators
            AddService(typeof(IDecorator<,>), typeof(CoreDecorator<,>));
            AddService(typeof(IDecorator<>), typeof(CoreDecorator<>));

            // Identifiers
            AddService<ISecurityIdentifierKeyRing, SecurityIdentifierKeyRing>();
            AddService<ISecurityIdentifierProtector, SecurityIdentifierProtector>();

            // Localizers
            AddService(typeof(IEnhancedStringLocalizer<>), typeof(EnhancedStringLocalizer<>));
            AddService(typeof(IDictionaryStringLocalizer<>), typeof(DictionaryStringLocalizer<>));
            AddService<IDictionaryStringLocalizerFactory, CoreResourceDictionaryStringLocalizerFactory>();
            Services.TryReplaceAll<IStringLocalizerFactory, CoreResourceManagerStringLocalizerFactory>();

            // Mediators
            AddServices(typeof(IRequestPipelineBehavior<,>),
                typeof(RequestPreProcessorBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
            AddService(typeof(IRequestHandlerWrapper<,>), typeof(RequestHandlerWrapper<,>));
            AddService(typeof(INotificationHandlerWrapper<>), typeof(NotificationHandlerWrapper<>));
            AddService<IMediator, ServiceFactoryMediator>();

            // Options
            Services.TryReplaceAll(typeof(IOptionsFactory<>), typeof(ConsistencyOptionsFactory<>));

            // Services
            AddService<ServiceFactory>(sp => sp.GetService);
            AddService(typeof(IServicesManager<,>), typeof(ServicesManager<,>));
            AddService(typeof(IServicesManager<>), typeof(ServicesManager<>));
            AddService<IInjectionService, InjectionService>();
            AddService<IHumanizationService, HumanizationService>();
            AddService<IClockService, ClockService>();
            AddService<IEnvironmentService, EnvironmentService>();
        }


        /// <summary>
        /// 获取指定服务类型的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => CoreBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);

    }
}
