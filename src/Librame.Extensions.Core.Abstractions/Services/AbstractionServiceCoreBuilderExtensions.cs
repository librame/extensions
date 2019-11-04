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
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象服务核心构建器静态扩展。
    /// </summary>
    public static class AbstractionServiceCoreBuilderExtensions
    {
        /// <summary>
        /// 添加当前线程应用域的程序集数组中已定义 <see cref="AutoRegistrationServiceAttribute"/> 特性的服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="ICoreBuilder"/>。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddAutoRegistrationServices(this ICoreBuilder builder)
            => builder.AddAutoRegistrationServices(AssemblyUtility.CurrentDomainAssembliesWithoutSystem);

        /// <summary>
        /// 添加指定程序集数组中已定义 <see cref="AutoRegistrationServiceAttribute"/> 特性的服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="ICoreBuilder"/>。</param>
        /// <param name="assemblies">给定要查找的程序集数组。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddAutoRegistrationServices(this ICoreBuilder builder,
            IEnumerable<Assembly> assemblies)
        {
            var objectType = typeof(object);
            
            assemblies.InvokeTypes(type =>
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
                            builder.Services.TryAddSingleton(serviceType, type);
                            break;

                        case ServiceLifetime.Scoped:
                            builder.Services.TryAddScoped(serviceType, type);
                            break;

                        case ServiceLifetime.Transient:
                            builder.Services.TryAddTransient(serviceType, type);
                            break;

                        default:
                            break;
                    }
                }
            });

            return builder;
        }

    }
}
