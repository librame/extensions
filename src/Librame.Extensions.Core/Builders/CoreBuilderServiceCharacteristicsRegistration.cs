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
using System.Collections.Generic;

namespace Librame.Extensions.Core.Builders
{
    using Decorators;
    using Identifiers;
    using Localizers;
    using Mediators;
    using Services;
    using Tokens;

    /// <summary>
    /// <see cref="ICoreBuilder"/> 服务特征注册。
    /// </summary>
    public static class CoreBuilderServiceCharacteristicsRegistration
    {
        private static IServiceCharacteristicsRegister _register;

        /// <summary>
        /// 当前注册器。
        /// </summary>
        public static IServiceCharacteristicsRegister Register
        {
            get => _register.EnsureSingleton(() => new ServiceCharacteristicsRegister(InitializeCharacteristics()));
            set => _register = value.NotNull(nameof(value));
        }


        private static IDictionary<Type, ServiceCharacteristics> InitializeCharacteristics()
        {
            return new Dictionary<Type, ServiceCharacteristics>
            {
                // Decorators
                { typeof(IDecorator<,>), ServiceCharacteristics.Transient() },
                { typeof(IDecorator<>), ServiceCharacteristics.Transient() },
                
                // Identifiers
                { typeof(IIdentificationGeneratorFactory), ServiceCharacteristics.Singleton() },

                // Localizers
                { typeof(IEnhancedStringLocalizer<>), ServiceCharacteristics.Transient() },
                { typeof(IDictionaryStringLocalizer<>), ServiceCharacteristics.Transient() },
                { typeof(IDictionaryStringLocalizerFactory), ServiceCharacteristics.Singleton() },

                // Mediators
                { typeof(IRequestPipelineBehavior<,>), ServiceCharacteristics.Transient() },
                { typeof(IRequestHandlerWrapper<,>), ServiceCharacteristics.Transient() },
                { typeof(INotificationHandlerWrapper<>), ServiceCharacteristics.Transient() },
                { typeof(IMediator), ServiceCharacteristics.Transient() },

                // Services
                { typeof(ServiceFactory), ServiceCharacteristics.Transient() },
                { typeof(IServicesManager<,>), ServiceCharacteristics.Transient() },
                { typeof(IServicesManager<>), ServiceCharacteristics.Transient() },
                { typeof(IInjectionService), ServiceCharacteristics.Transient() },
                { typeof(IHumanizationService), ServiceCharacteristics.Singleton() },
                { typeof(IClockService), ServiceCharacteristics.Singleton() },
                { typeof(IEnvironmentService), ServiceCharacteristics.Singleton() },
                
                // Tokens
                { typeof(ISecurityTokenKeyRing), ServiceCharacteristics.Singleton() },
                { typeof(ISecurityTokenProtector), ServiceCharacteristics.Singleton() }
            };
        }

    }
}
