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
using Librame.Extensions.Data.Aspects;
using Librame.Extensions.Data.Builders;
using Librame.Extensions.Data.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IDataBuilder"/> 静态扩展。
    /// </summary>
    public static class AbstractionDataBuilderExtensions
    {

        #region AddAccessorAspect

        /// <summary>
        /// 添加访问器截面。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddMigrateAccessorAspect(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorAspect<IMigrateAccessorAspect>(implementationTypeDefinition);

        /// <summary>
        /// 添加访问器截面。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddSaveChangesAccessorAspect(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorAspect<ISaveChangesAccessorAspect>(implementationTypeDefinition);


        /// <summary>
        /// 添加访问器截面。
        /// </summary>
        /// <typeparam name="TService">指定实现 <see cref="IAccessorAspect"/> 接口的服务类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddAccessorAspect<TService>(this IDataBuilder builder,
            Type implementationTypeDefinition)
            where TService : IAccessorAspect
            => builder.AddAccessorAspect(typeof(TService), implementationTypeDefinition);

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

            return builder.AddGenericServiceByPopulateAccessorTypeParameters(serviceType, implementationTypeDefinition,
                (type, descriptor) => type.MakeGenericType(descriptor.GenId.ArgumentType, descriptor.CreatedBy.ArgumentType),
                addEnumerable: true);
        }

        #endregion


        #region AddAccessorService

        /// <summary>
        /// 添加迁移访问器服务。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddMigrationAccessorService(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorService<IMigrationAccessorService>(implementationTypeDefinition);

        /// <summary>
        /// 添加多租户访问器服务。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddMultiTenantAccessorService(this IDataBuilder builder,
            Type implementationTypeDefinition)
            => builder.AddAccessorService<IMultiTenancyAccessorService>(implementationTypeDefinition);


        /// <summary>
        /// 添加访问器服务。
        /// </summary>
        /// <typeparam name="TService">指定实现 <see cref="IAccessorService"/> 接口的服务类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddAccessorService<TService>(this IDataBuilder builder,
            Type implementationTypeDefinition)
            where TService : IAccessorService
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

            return builder.AddGenericServiceByPopulateAccessorTypeParameters(serviceType,
                implementationTypeDefinition);
        }

        #endregion

    }
}
