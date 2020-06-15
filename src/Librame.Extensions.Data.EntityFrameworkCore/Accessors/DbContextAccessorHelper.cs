#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Accessors
{
    using Core.Identifiers;
    using Data.Stores;

    /// <summary>
    /// 数据库上下文访问器助手。
    /// </summary>
    public static class DbContextAccessorHelper
    {
        /// <summary>
        /// 解析访问器泛型类型参数集合。
        /// </summary>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <returns>返回 <see cref="AccessorGenericTypeArguments"/>。</returns>
        public static AccessorGenericTypeArguments ParseAccessorGenericTypeArguments(Type accessorType)
        {
            var accessorTypeDefinition = typeof(IDbContextAccessor<,,,,>);

            // 不强制实现数据类访问器
            if (!accessorType.IsImplementedInterface(accessorTypeDefinition, out var resultType))
                return null;

            var arguments = new AccessorGenericTypeArguments
            {
                AccessorType = accessorType,
                AuditType = resultType.GenericTypeArguments[0],
                AuditPropertyType = resultType.GenericTypeArguments[1],
                EntityType = resultType.GenericTypeArguments[2],
                MigrationType = resultType.GenericTypeArguments[3],
                TenantType = resultType.GenericTypeArguments[4],
            };

            if (!accessorType.IsImplementedInterface(typeof(IDbContextAccessor<,,>), out resultType))
            {
                var baseEntityType = arguments.TenantType
                    ?? arguments.MigrationType
                    ?? arguments.EntityType
                    ?? arguments.AuditType;

                if (baseEntityType.IsImplementedInterface(typeof(IIdentifier<>), out resultType))
                    arguments.GenIdType = resultType.GenericTypeArguments[0];

                if (baseEntityType.IsImplementedInterface(typeof(ICreation<,>), out resultType))
                {
                    arguments.CreatedByType = resultType.GenericTypeArguments[0];
                    arguments.CreatedTimeType = resultType.GenericTypeArguments[1];
                }

                if (arguments.AuditPropertyType.IsImplementedInterface(typeof(IIdentifier<>), out resultType))
                    arguments.IncremIdType = resultType.GenericTypeArguments[0];
            }
            else
            {
                arguments.GenIdType = resultType.GenericTypeArguments[0];
                arguments.IncremIdType = resultType.GenericTypeArguments[1];
                arguments.CreatedByType = resultType.GenericTypeArguments[2];

                if (arguments.TenantType.IsImplementedInterface(typeof(ICreation<,>), out resultType))
                    arguments.CreatedTimeType = resultType.GenericTypeArguments[1];
            }

            return arguments;
        }

    }
}
