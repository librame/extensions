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
    /// 服务集合静态扩展。
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加自动注册服务集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        public static void AddAutoRegistrationServices(this IServiceCollection services)
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
                            services.AddSingleton(serviceType, type);
                            break;

                        case ServiceLifetime.Scoped:
                            services.AddScoped(serviceType, type);
                            break;

                        case ServiceLifetime.Transient:
                            services.AddTransient(serviceType, type);
                            break;

                        default:
                            break;
                    }
                }
            });
        }

    }
}
