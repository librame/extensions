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
    /// 中介者构建器静态扩展。
    /// </summary>
    public static class MediatorBuilderExtensions
    {
        /// <summary>
        /// 添加中介者集合扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddMediators(this IBuilder builder)
        {
            builder.Services.AddTransient<ServiceFactoryDelegate>(sp => sp.GetService);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
            builder.Services.AddTransient<IMediator, InternalMediator>();

            if (builder.Options is CoreBuilderOptions options && options.EnableScanHandlersAndProcessors)
                ScanHandlersAndProcessors(builder);

            return builder;
        }

        private static void ScanHandlersAndProcessors(IBuilder builder)
        {
            ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>), builder.Services, false);
            ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>), builder.Services, true);
            ConnectImplementationsToTypesClosing(typeof(IRequestPreProcessor<>), builder.Services, true);
            ConnectImplementationsToTypesClosing(typeof(IRequestPostProcessor<,>), builder.Services, true);

            var multiOpenInterfaces = new[]
            {
                typeof(INotificationHandler<>),
                typeof(IRequestPreProcessor<>),
                typeof(IRequestPostProcessor<,>)
            };

            foreach (var multiOpenInterface in multiOpenInterfaces)
            {
                BuilderGlobalization.RegisterTypes(type =>
                {
                    builder.Services.AddTransient(multiOpenInterface, type);
                },
                types => types
                    .Where(type => Enumerable.Any(type.FindInterfacesThatClose(multiOpenInterface)))
                    .Where(type => type.IsConcreteType() && type.IsOpenGenericType()));
            }
        }

        private static void ConnectImplementationsToTypesClosing(Type openRequestInterface,
            IServiceCollection services, bool addIfAlreadyExists)
        {
            var concretions = new List<Type>();
            var interfaces = new List<Type>();

            BuilderGlobalization.RegisterTypes(type =>
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
                    AddConcretionsThatCouldBeClosed(@interface, concretions, services);
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

        private static void AddConcretionsThatCouldBeClosed(Type @interface, List<Type> concretions, IServiceCollection services)
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
