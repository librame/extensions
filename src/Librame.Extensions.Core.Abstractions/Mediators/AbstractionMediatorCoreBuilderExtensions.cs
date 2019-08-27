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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象中介者核心构建器静态扩展。
    /// </summary>
    public static class AbstractionMediatorCoreBuilderExtensions
    {
        /// <summary>
        /// 通过当前线程应用域的程序集数组添加中介者集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="ICoreBuilder"/>。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddAutoRegistrationMediators(this ICoreBuilder builder)
        {
            return builder.AddAutoRegistrationMediators(AssemblyHelper.CurrentDomainAssembliesWithoutSystem);
        }

        /// <summary>
        /// 通过指定的程序集数组添加中介者集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="ICoreBuilder"/>。</param>
        /// <param name="assemblies">给定要查找的程序集数组。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddAutoRegistrationMediators(this ICoreBuilder builder,
            params Assembly[] assemblies)
        {
            builder.Services.ConnectImplementationsToTypesClosing(assemblies,
                typeof(IRequestHandler<,>), false);
            builder.Services.ConnectImplementationsToTypesClosing(assemblies,
                typeof(INotificationHandler<>), true);
            builder.Services.ConnectImplementationsToTypesClosing(assemblies,
                typeof(IRequestPreProcessor<>), true);
            builder.Services.ConnectImplementationsToTypesClosing(assemblies,
                typeof(IRequestPostProcessor<,>), true);

            var multiOpenInterfaces = new[]
            {
                typeof(INotificationHandler<>),
                typeof(IRequestPreProcessor<>),
                typeof(IRequestPostProcessor<,>)
            };

            foreach (var multiOpenInterface in multiOpenInterfaces)
            {
                assemblies.InvokeTypes(type =>
                {
                    builder.Services.AddTransient(multiOpenInterface, type);
                },
                types => types
                    .Where(type => Enumerable.Any(type.FindInterfacesThatClose(multiOpenInterface)))
                    .Where(type => type.IsConcreteType() && type.IsOpenGenericType()));
            }

            return builder;
        }

        private static void ConnectImplementationsToTypesClosing(this IServiceCollection services,
            Assembly[] assemblies, Type openRequestInterface, bool addIfAlreadyExists)
        {
            var concretions = new List<Type>();
            var interfaces = new List<Type>();

            assemblies.InvokeTypes(type =>
            {
                var interfaceTypes = Enumerable.ToArray(type.FindInterfacesThatClose(openRequestInterface));
                if (interfaceTypes.Any())
                {
                    if (type.IsConcreteType())
                    {
                        concretions.Add(type);
                    }

                    foreach (var interfaceType in interfaceTypes)
                    {
                        interfaces.AddIfNotContains(interfaceType);
                    }
                }
            },
            types => types.Where(t => !t.IsOpenGenericType()));

            foreach (var @interface in interfaces)
            {
                var exactMatches = concretions.Where(x => x.CanBeCastTo(@interface)).ToList();
                if (addIfAlreadyExists)
                {
                    foreach (var type in exactMatches)
                    {
                        services.AddTransient(@interface, type);
                    }
                }
                else
                {
                    if (exactMatches.Count > 1)
                    {
                        exactMatches.RemoveAll(m => !IsMatchingWithInterface(m, @interface));
                    }

                    foreach (var type in exactMatches)
                    {
                        services.TryAddTransient(@interface, type);
                    }
                }

                if (!@interface.IsOpenGenericType())
                {
                    services.AddConcretionsThatCouldBeClosed(@interface, concretions);
                }
            }
        }

        private static void AddConcretionsThatCouldBeClosed(this IServiceCollection services,
            Type @interface, List<Type> concretions)
        {
            foreach (var type in concretions
                .Where(x => x.IsOpenGenericType() && x.CouldCloseTo(@interface)))
            {
                try
                {
                    services.TryAddTransient(@interface, type.MakeGenericType(@interface.GenericTypeArguments));
                }
                catch (Exception)
                {
                }
            }
        }

        private static bool IsMatchingWithInterface(Type handlerType, Type handlerInterface)
        {
            if (handlerType == null || handlerInterface == null)
            {
                return false;
            }

            if (handlerType.IsInterface)
            {
                if (handlerType.GenericTypeArguments.SequenceEqual(handlerInterface.GenericTypeArguments))
                {
                    return true;
                }
            }
            else
            {
                return IsMatchingWithInterface(handlerType.GetInterface(handlerInterface.Name), handlerInterface);
            }

            return false;
        }

        private static bool CouldCloseTo(this Type openConcretion, Type closedInterface)
        {
            var openInterface = closedInterface.GetGenericTypeDefinition();
            var arguments = closedInterface.GenericTypeArguments;

            var concreteArguments = openConcretion.GenericTypeArguments;
            return arguments.Length == concreteArguments.Length && openConcretion.CanBeCastTo(openInterface);
        }

        private static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType == null) return false;

            if (pluggedType == pluginType) return true;

            return pluginType.IsAssignableFrom(pluggedType);
        }

        private static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType)
        {
            return Enumerable.Distinct(FindInterfacesThatClosesCore(pluggedType, templateType));
        }

        private static IEnumerable<Type> FindInterfacesThatClosesCore(Type pluggedType, Type templateType)
        {
            if (pluggedType == null) yield break;

            if (!pluggedType.IsConcreteType()) yield break;

            if (templateType.GetTypeInfo().IsInterface)
            {
                foreach (
                    var interfaceType in
                    pluggedType.GetInterfaces()
                        .Where(type => type.IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
                {
                    yield return interfaceType;
                }
            }
            else if (pluggedType.BaseType.IsGenericType &&
                (pluggedType.BaseType.GetGenericTypeDefinition() == templateType))
            {
                yield return pluggedType.BaseType;
            }

            if (pluggedType.GetTypeInfo().BaseType == typeof(object)) yield break;

            foreach (var interfaceType in FindInterfacesThatClosesCore(pluggedType.GetTypeInfo().BaseType, templateType))
            {
                yield return interfaceType;
            }
        }

    }
}
