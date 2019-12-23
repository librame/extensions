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

namespace Librame.Extensions.Core.Builders
{
    /// <summary>
    /// 抽象扩展构建器。
    /// </summary>
    public abstract class AbstractExtensionBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected AbstractExtensionBuilder(IExtensionBuilder parentBuilder, IExtensionBuilderDependency dependency)
        {
            Dependency = dependency.NotNull(nameof(dependency));
            ParentBuilder = parentBuilder.NotNull(nameof(parentBuilder));

            Services = parentBuilder.Services;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        protected AbstractExtensionBuilder(IServiceCollection services, IExtensionBuilderDependency dependency)
        {
            Dependency = dependency.NotNull(nameof(dependency));
            Services = services.NotNull(nameof(services));

            ParentBuilder = null;
        }


        /// <summary>
        /// 父级构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilder"/>。
        /// </value>
        public IExtensionBuilder ParentBuilder { get; }

        /// <summary>
        /// 构建器依赖。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependency"/>。
        /// </value>
        public IExtensionBuilderDependency Dependency { get; }

        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        public IServiceCollection Services { get; }
    }
}
