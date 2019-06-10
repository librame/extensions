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
using System;
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部构建器。
    /// </summary>
    internal class InternalBuilder : AbstractBuilder<BuilderOptions>, IBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalBuilder"/> 实例。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="options">给定的 <see cref="BuilderOptions"/>。</param>
        public InternalBuilder(IServiceCollection services, BuilderOptions options)
            : base(services, options)
        {
            Services.AddSingleton<IBuilder>(this);
        }


        /// <summary>
        /// 初始化构建器。
        /// </summary>
        /// <param name="options">给定的 <see cref="BuilderOptions"/>。</param>
        protected override void Initialize(BuilderOptions options)
        {
            BuilderGlobalization.RegisterCultureInfos(options.CultureInfo, options.CultureUIInfo);

            if (options.UseAutoRegistrationServices)
                AddAutoRegistrationServices();
        }


        /// <summary>
        /// 添加自动注册服务集合。
        /// </summary>
        private void AddAutoRegistrationServices()
        {
            var objectType = typeof(object);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type in assembly.GetTypes())
            {
                if (type.TryGetCustomAttribute(out AutoRegistrationServiceAttribute serviceAttribute))
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
            }
        }

    }
}
