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
using System.Linq;

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

            if (options.EnableAutoRegistrationServices)
                AddAutoRegistrationServices();
        }


        /// <summary>
        /// 添加自动注册服务集合。
        /// </summary>
        private void AddAutoRegistrationServices()
        {
            var objectType = typeof(object);

            BuilderGlobalization.RegisterTypes(type =>
            {
                if (type.TryGetCustomAttribute(out RegistrationServiceAttribute serviceAttribute))
                {
                    var serviceType = serviceAttribute.ServiceType;

                    if (serviceType.IsNull() && serviceAttribute.UseBaseTypeAsServiceType)
                    {
                        // 如果类型的基础类型为空或是接口，则基础类型返回 Object 类型
                        serviceType = type.BaseType != objectType ? type.BaseType
                            : type.GetInterfaces().FirstOrDefault();
                    }

                    if (serviceType.IsNull())
                    {
                        // 使用当前类型为服务类型
                        serviceType = type;
                    }

                    switch (serviceAttribute.Lifetime)
                    {
                        case ServiceLifetime.Singleton:
                            Services.AddSingleton(serviceType, type);
                            break;

                        case ServiceLifetime.Scoped:
                            Services.AddScoped(serviceType, type);
                            break;

                        case ServiceLifetime.Transient:
                            Services.AddTransient(serviceType, type);
                            break;

                        default:
                            break;
                    }
                }
            });
        }

    }
}
