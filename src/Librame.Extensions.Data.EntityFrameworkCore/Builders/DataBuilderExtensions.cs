#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core.Builders;
using Librame.Extensions.Data;
using Librame.Extensions.Data.Builders;
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
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureOptions">给定的配置选项动作方法。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder baseBuilder,
            Action<DataBuilderOptions> configureOptions,
            Func<IExtensionBuilder, DataBuilderDependency, IDataBuilder> builderFactory = null)
        {
            configureOptions.NotNull(nameof(configureOptions));

            return baseBuilder.AddData(dependency =>
            {
                dependency.Builder.ConfigureOptions = configureOptions;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder baseBuilder,
            Action<DataBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, DataBuilderDependency, IDataBuilder> builderFactory = null)
            => baseBuilder.AddData<DataBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "baseBuilder")]
        public static IDataBuilder AddData<TDependencyOptions>(this IExtensionBuilder baseBuilder,
            Action<TDependencyOptions> configureDependency = null,
            Func<IExtensionBuilder, TDependencyOptions, IDataBuilder> builderFactory = null)
            where TDependencyOptions : DataBuilderDependency, new()
        {
            baseBuilder.NotNull(nameof(baseBuilder));

            // Configure Dependency
            var dependency = configureDependency.ConfigureDependency(baseBuilder);

            // Add Dependencies
            baseBuilder.Services
                .AddEntityFrameworkDesignTimeServices();

            // Create Builder
            var dataBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new DataBuilder(b, d)).Invoke(baseBuilder, dependency);

            // Configure Builder
            return dataBuilder
                .AddAspects()
                .AddMediators()
                .AddServices()
                .AddStores();
        }


        /// <summary>
        /// 添加数据库设计时。
        /// </summary>
        /// <typeparam name="TDesignTime">指定的设计时类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddDbDesignTime<TDesignTime>(this IDataBuilder builder)
            where TDesignTime : class, IDesignTimeServices
        {
            builder.NotNull(nameof(builder));

            var designTimeType = typeof(TDesignTime);
            var designTime = designTimeType.EnsureCreate<TDesignTime>();
            designTime.ConfigureDesignTimeServices(builder.Services);

            builder.Services.Configure<DataBuilderOptions>(options =>
            {
                var reference = AssemblyReference.ByAssembly(designTimeType.Assembly);
                if (!options.MigrationAssemblyReferences.Contains(reference))
                    options.MigrationAssemblyReferences.Add(reference);
            });

            return builder;
        }

    }
}
