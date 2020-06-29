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

namespace Librame.Extensions.Data.Mappers
{
    using Core.Mappers;
    using Core.Identifiers;
    using Data.Accessors;
    using Data.Stores;

    /// <summary>
    /// 访问器类型参数映射助手。
    /// </summary>
    public static class AccessorTypeParameterMappingHelper
    {
        /// <summary>
        /// 解析映射器。
        /// </summary>
        /// <param name="accessorTypeImplementation">给定的访问器类型实现。</param>
        /// <returns>返回 <see cref="AccessorTypeParameterMapper"/>。</returns>
        public static AccessorTypeParameterMapper ParseMapper(Type accessorTypeImplementation)
        {
            var mappings = ParseAsDictionary(accessorTypeImplementation, out var accessorMapping);
            return new AccessorTypeParameterMapper(accessorMapping, mappings);
        }


        /// <summary>
        /// 解析为字典。
        /// </summary>
        /// <param name="accessorTypeImplementation">给定的访问器类型实现。</param>
        /// <returns>返回 <see cref="Dictionary{String, TypeParameterMapping}"/>。</returns>
        public static Dictionary<string, TypeParameterMapping> ParseAsDictionary(Type accessorTypeImplementation)
            => ParseAsDictionary(accessorTypeImplementation, out _);

        /// <summary>
        /// 解析为字典。
        /// </summary>
        /// <param name="accessorTypeImplementation">给定的访问器类型实现。</param>
        /// <param name="accessorMapping">输出访问器 <see cref="TypeParameterMapping"/>。</param>
        /// <returns>返回 <see cref="Dictionary{String, TypeParameterMapping}"/>。</returns>
        public static Dictionary<string, TypeParameterMapping> ParseAsDictionary(Type accessorTypeImplementation,
            out TypeParameterMapping accessorMapping)
        {
            var accessorTypeDefinition = typeof(IDbContextAccessor<,,,,>);

            // 因访问器默认服务类型为 IAccessor，所以不强制实现访问器泛型类型定义
            if (!accessorTypeImplementation.IsImplementedInterface(accessorTypeDefinition, out var resultType))
            {
                accessorMapping = null;
                return null;
            }

            accessorMapping = new TypeParameterMapping(accessorTypeDefinition, accessorTypeImplementation);

            var mappings = TypeParameterMappingHelper.ParseAsDictionary(accessorTypeDefinition, resultType);

            var otherTypeDefinition = typeof(IIdentifier<>);

            var otherTypeImplementation = mappings["TAuditProperty"].ArgumentType;
            if (otherTypeImplementation.IsImplementedInterface(otherTypeDefinition, out resultType))
            {
                var genericTypeParameters = otherTypeDefinition.GetTypeInfo().GenericTypeParameters;

                mappings.Add("TIncremId", new TypeParameterMapping(genericTypeParameters[0],
                    resultType.GenericTypeArguments[0]));
            }

            otherTypeImplementation = mappings["TAudit"].ArgumentType;
            if (otherTypeImplementation.IsImplementedInterface(otherTypeDefinition, out resultType))
            {
                var genericTypeParameters = otherTypeDefinition.GetTypeInfo().GenericTypeParameters;

                mappings.Add("TGenId", new TypeParameterMapping(genericTypeParameters[0],
                    resultType.GenericTypeArguments[0]));
            }

            otherTypeDefinition = typeof(ICreation<,>);

            if (otherTypeImplementation.IsImplementedInterface(otherTypeDefinition, out resultType))
            {
                var genericTypeParameters = otherTypeDefinition.GetTypeInfo().GenericTypeParameters;

                mappings.Add("TCreatedBy", new TypeParameterMapping(genericTypeParameters[0],
                    resultType.GenericTypeArguments[0]));

                mappings.Add("TCreatedTime", new TypeParameterMapping(genericTypeParameters[1],
                    resultType.GenericTypeArguments[1]));
            }

            return mappings;
        }

    }
}
