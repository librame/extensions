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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象扩展构建器。
    /// </summary>
    public abstract class AbstractExtensionBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependencyOptions">给定的 <see cref="IExtensionBuilderDependencyOptions"/>。</param>
        protected AbstractExtensionBuilder(IServiceCollection services, IExtensionBuilderDependencyOptions dependencyOptions)
        {
            ParentBuilder = null;

            Services = services.NotNull(nameof(services));
            DependencyOptions = dependencyOptions.NotNull(nameof(dependencyOptions));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyOptions">给定的 <see cref="IExtensionBuilderDependencyOptions"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        protected AbstractExtensionBuilder(IExtensionBuilder builder, IExtensionBuilderDependencyOptions dependencyOptions)
        {
            ParentBuilder = builder.NotNull(nameof(builder));

            Services = builder.Services;
            DependencyOptions = dependencyOptions.NotNull(nameof(dependencyOptions));
        }


        /// <summary>
        /// 父构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilder"/>。
        /// </value>
        public IExtensionBuilder ParentBuilder { get; }


        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 依赖选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependencyOptions"/>。
        /// </value>
        public IExtensionBuilderDependencyOptions DependencyOptions { get; }
    }
}
