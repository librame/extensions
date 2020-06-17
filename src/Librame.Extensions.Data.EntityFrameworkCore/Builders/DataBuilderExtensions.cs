#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core;
using Librame.Extensions.Core.Builders;
using Librame.Extensions.Core.Options;
using Librame.Extensions.Data.Aspects;
using Librame.Extensions.Data.Builders;
using Librame.Extensions.Data.Services;
using Librame.Extensions.Data.Stores;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据构建器静态扩展。
    /// </summary>
    public static class DataBuilderExtensions
    {
        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder parentBuilder,
            Action<DataBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, DataBuilderDependency, IDataBuilder> builderFactory = null)
            => parentBuilder.AddData<DataBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDataBuilder AddData<TDependency>(this IExtensionBuilder parentBuilder,
            Action<TDependency> configureDependency = null,
            Func<IExtensionBuilder, TDependency, IDataBuilder> builderFactory = null)
            where TDependency : DataBuilderDependency
        {
            // Clear Options Cache
            ConsistencyOptionsCache.TryRemove<DataBuilderOptions>();

            // Add Builder Dependency
            var dependency = parentBuilder.AddBuilderDependency(out var dependencyType, configureDependency);
            parentBuilder.Services.TryAddReferenceBuilderDependency<DataBuilderDependency>(dependency, dependencyType);

            // Add Dependencies
            if (dependency.SupportsEntityFrameworkDesignTimeServices)
            {
                parentBuilder.Services
                    .AddEntityFrameworkDesignTimeServices();
            }

            // Create Builder
            return builderFactory.NotNullOrDefault(()
                => (b, d) => new DataBuilder(b, d)).Invoke(parentBuilder, dependency);
        }


        /// <summary>
        /// 添加数据库设计时。
        /// </summary>
        /// <typeparam name="TDesignTime">指定的设计时类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDataBuilder AddDatabaseDesignTime<TDesignTime>(this IDataBuilder builder)
            where TDesignTime : class, IDesignTimeServices
        {
            builder.NotNull(nameof(builder));

            var designTimeType = typeof(TDesignTime);
            builder.SetProperty(p => p.DatabaseDesignTimeType, designTimeType);

            var designTime = designTimeType.EnsureCreate<TDesignTime>();
            designTime.ConfigureDesignTimeServices(builder.Services);

            builder.Services.Configure<DataBuilderOptions>(options =>
            {
                var reference = AssemblyReference.Load(designTimeType.Assembly);
                if (!options.MigrationAssemblyReferences.Contains(reference))
                    options.MigrationAssemblyReferences.Add(reference);
            });

            return builder;
        }


        #region AddAccessorAspect

        /// <summary>
        /// 添加访问器截面。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddMigrateAccessorAspect(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorAspect(typeof(IMigrateAccessorAspect<,>), implementationTypeDefinition);

        /// <summary>
        /// 添加访问器截面。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddSaveChangesAccessorAspect(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorAspect(typeof(ISaveChangesAccessorAspect<,>), implementationTypeDefinition);

        /// <summary>
        /// 添加访问器截面。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="serviceType">给定的截面服务类型（支持接口类型定义）。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static IDataBuilder AddAccessorAspect(this IDataBuilder builder, Type serviceType,
            Type implementationTypeDefinition)
        {
            builder.NotNull(nameof(builder));

            return builder.AddGenericServiceByPopulateMappingDescriptor(serviceType, implementationTypeDefinition,
                (type, descriptor) => type.MakeGenericType(descriptor.GenId.ArgumentType, descriptor.CreatedBy.ArgumentType),
                addEnumerable: true);
        }

        #endregion


        #region AddAccessorService

        /// <summary>
        /// 添加访问器服务。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddMigrationAccessorService(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorService<IMigrationAccessorService>(implementationTypeDefinition);

        /// <summary>
        /// 添加访问器服务。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddMultiTenantAccessorService(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorService<IMultiTenantAccessorService>(implementationTypeDefinition);

        /// <summary>
        /// 添加访问器服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddAccessorService<TService>(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorService(typeof(TService), implementationTypeDefinition);

        /// <summary>
        /// 添加访问器服务。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static IDataBuilder AddAccessorService(this IDataBuilder builder,
            Type serviceType, Type implementationTypeDefinition)
        {
            builder.NotNull(nameof(builder));

            return builder.AddGenericServiceByPopulateMappingDescriptor(serviceType,
                implementationTypeDefinition);
        }

        #endregion


        #region AddStoreHub

        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="TStoreHub">指定的存储中心类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddStoreHub<TStoreHub>(this IDataBuilder builder)
            where TStoreHub : class, IStoreHubIndication
            => builder.AddStoreHub(typeof(TStoreHub));

        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="hubType">给定的存储中心类型。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static IDataBuilder AddStoreHub(this IDataBuilder builder, Type hubType)
        {
            builder.NotNull(nameof(builder));

            builder.AddGenericService(typeof(IStoreHub<>), hubType,
                addImplementationTypeItself: true);

            return builder;
        }

        #endregion


        #region AddStoreInitializer

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定的初始化器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddStoreInitializer<TInitializer>(this IDataBuilder builder)
            where TInitializer : class, IStoreInitializerIndication
            => builder.AddStoreInitializer(typeof(TInitializer));

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="initializerType">给定的初始化器类型。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static IDataBuilder AddStoreInitializer(this IDataBuilder builder, Type initializerType)
        {
            builder.NotNull(nameof(builder));

            builder.AddGenericService(typeof(IStoreInitializer<>), initializerType);
            return builder;
        }

        #endregion

    }
}
