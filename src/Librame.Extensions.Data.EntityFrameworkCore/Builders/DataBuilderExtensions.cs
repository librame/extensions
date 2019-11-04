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
using Librame.Extensions.Core;
using Librame.Extensions.Data;
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
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder builder,
            Action<DataBuilderOptions> builderAction,
            Func<IExtensionBuilder, DataBuilderDependencyOptions, IDataBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddData(dependency =>
            {
                dependency.Builder.Action = builderAction;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder builder,
            Action<DataBuilderDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, DataBuilderDependencyOptions, IDataBuilder> builderFactory = null)
            => builder.AddData<DataBuilderDependencyOptions>(dependencyAction, builderFactory);

        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IDataBuilder AddData<TDependencyOptions>(this IExtensionBuilder builder,
            Action<TDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, TDependencyOptions, IDataBuilder> builderFactory = null)
            where TDependencyOptions : DataBuilderDependencyOptions, new()
        {
            builder.NotNull(nameof(builder));

            // Configure DependencyOptions
            var dependency = dependencyAction.ConfigureDependency();
            builder.Services.AddAllOptionsConfigurators(dependency);

            // AddEntityFrameworkDesignTimeServices
            builder.Services.AddEntityFrameworkDesignTimeServices();

            // Create Builder
            var dataBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new DataBuilder(b, d)).Invoke(builder, dependency);

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
