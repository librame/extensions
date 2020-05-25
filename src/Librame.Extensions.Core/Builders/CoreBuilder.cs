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

    internal class CoreBuilder : AbstractExtensionBuilder, ICoreBuilder
    {
        public CoreBuilder(IServiceCollection services, CoreBuilderDependency dependency)
            : base(services, dependency)
        {
            Services.AddSingleton<ICoreBuilder>(this);
            Services.AddSingleton(sp => (IExtensionBuilder)sp.GetRequiredService<ICoreBuilder>());

            AddCoreServices();
        }


        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => CoreBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);


        private void AddCoreServices()
        {
            // Decorators
            AddService(typeof(IDecorator<,>), typeof(CoreDecorator<,>));
            AddService(typeof(IDecorator<>), typeof(CoreDecorator<>));

            // Identifiers
            AddService<ISecurityIdentifierKeyRing, SecurityIdentifierKeyRing>();
            AddService<ISecurityIdentifierProtector, SecurityIdentifierProtector>();

            // Localizers
            AddService(typeof(IDictionaryStringLocalizer<>), typeof(DictionaryStringLocalizer<>));
            AddService<IDictionaryStringLocalizerFactory, CoreResourceDictionaryStringLocalizerFactory>();
            Services.TryReplace<IStringLocalizerFactory, CoreResourceManagerStringLocalizerFactory>();

            // Mediators
            AddServices(typeof(IRequestPipelineBehavior<,>),
                typeof(RequestPreProcessorBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
            AddService(typeof(IRequestHandlerWrapper<,>), typeof(RequestHandlerWrapper<,>));
            AddService(typeof(INotificationHandlerWrapper<>), typeof(NotificationHandlerWrapper<>));
            AddService<IMediator, ServiceFactoryMediator>();

            // Options
            Services.TryReplace(typeof(IOptionsFactory<>), typeof(ConsistencyOptionsFactory<>));

            // Services
            AddService<ServiceFactory>(sp => sp.GetService);
            AddService(typeof(IServicesManager<,>), typeof(ServicesManager<,>));
            AddService(typeof(IServicesManager<>), typeof(ServicesManager<>));
            AddService<IInjectionService, InjectionService>();
            AddService<IHumanizationService, HumanizationService>();
            AddService<IClockService, ClockService>();
            AddService<IEnvironmentService, EnvironmentService>();
        }

    }
}

