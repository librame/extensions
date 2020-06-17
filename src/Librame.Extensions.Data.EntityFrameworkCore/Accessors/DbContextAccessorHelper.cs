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
using System.Collections.Generic;
using System.Reflection;

namespace Librame.Extensions.Data.Accessors
{
    using Core;
    using Core.Identifiers;
    using Data.Stores;

    /// <summary>
    /// 数据库上下文访问器助手。
    /// </summary>
    public static class DbContextAccessorHelper
    {
        /// <summary>
        /// 解析泛型类型映射描述符。
        /// </summary>
        /// <param name="accessorTypeImplementation">给定的访问器类型实现。</param>
        /// <returns>返回 <see cref="AccessorGenericTypeMappingDescriptor"/>。</returns>
        public static AccessorGenericTypeMappingDescriptor ParseMappingDescriptor(Type accessorTypeImplementation)
        {
            var mappings = ParseMappingDictionary(accessorTypeImplementation, out var accessorMapping);
            return new AccessorGenericTypeMappingDescriptor(accessorMapping, mappings);
        }


        /// <summary>
        /// 解析泛型类型映射字典。
        /// </summary>
        /// <param name="accessorTypeImplementation">给定的访问器类型实现。</param>
        /// <returns>返回 <see cref="Dictionary{String, GenericTypeMapping}"/>。</returns>
        public static Dictionary<string, GenericTypeMapping> ParseMappingDictionary(Type accessorTypeImplementation)
            => ParseMappingDictionary(accessorTypeImplementation, out _);

        /// <summary>
        /// 解析泛型类型映射字典。
        /// </summary>
        /// <param name="accessorTypeImplementation">给定的访问器类型实现。</param>
        /// <param name="accessorMapping">输出访问器 <see cref="GenericTypeMapping"/>。</param>
        /// <returns>返回 <see cref="Dictionary{String, GenericTypeMapping}"/>。</returns>
        public static Dictionary<string, GenericTypeMapping> ParseMappingDictionary(Type accessorTypeImplementation,
            out GenericTypeMapping accessorMapping)
        {
            var accessorTypeDefinition = typeof(IDbContextAccessor<,,,,>);

            // 因访问器默认服务类型为 IAccessor，所以不强制实现访问器泛型类型定义
            if (!accessorTypeImplementation.IsImplementedInterface(accessorTypeDefinition, out var resultType))
            {
                accessorMapping = null;
                return null;
            }

            accessorMapping = new GenericTypeMapping(accessorTypeDefinition, accessorTypeImplementation);

            var parameters = GenericTypeMapping.PopulateDictionary(accessorTypeDefinition, resultType);

            accessorTypeDefinition = typeof(IDbContextAccessor<,,>);
            var accessorTypeParameters = accessorTypeDefinition.GetTypeInfo().GenericTypeParameters;

            var auditType = parameters["TAudit"].ArgumentType;
            if (!accessorTypeImplementation.IsImplementedInterface(accessorTypeDefinition, out resultType))
            {
                var auditPropertyType = parameters["TAuditProperty"].ArgumentType;

                var identifierTypeDefinition = typeof(IIdentifier<>);
                var identifierTypeParameter = identifierTypeDefinition.GetTypeInfo().GenericTypeParameters[0];

                if (auditType.IsImplementedInterface(identifierTypeDefinition, out resultType))
                {
                    parameters.Add("TGenId", new GenericTypeMapping(identifierTypeParameter,
                        resultType.GenericTypeArguments[0]));
                }

                if (auditPropertyType.IsImplementedInterface(identifierTypeDefinition, out resultType))
                {
                    parameters.Add("TIncremId", new GenericTypeMapping(identifierTypeParameter,
                        resultType.GenericTypeArguments[0]));
                }
            }
            else
            {
                parameters.Add("TGenId", new GenericTypeMapping(accessorTypeParameters[0],
                    resultType.GenericTypeArguments[0]));

                parameters.Add("TIncremId", new GenericTypeMapping(accessorTypeParameters[1],
                    resultType.GenericTypeArguments[1]));
            }

            var creationTypeDefinition = typeof(ICreation<,>);
            var creationTypeParameters = creationTypeDefinition.GetTypeInfo().GenericTypeParameters;

            if (auditType.IsImplementedInterface(creationTypeDefinition, out resultType))
            {
                parameters.Add("TCreatedBy", new GenericTypeMapping(creationTypeParameters[0],
                    resultType.GenericTypeArguments[0]));

                parameters.Add("TCreatedTime", new GenericTypeMapping(creationTypeParameters[1],
                    resultType.GenericTypeArguments[1]));
            }

            return parameters;
        }

    }
}
