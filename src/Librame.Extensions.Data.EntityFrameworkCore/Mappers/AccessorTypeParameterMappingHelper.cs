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
            var mappings = ParseCollection(accessorTypeImplementation, out var accessorMapping);
            return new AccessorTypeParameterMapper(accessorMapping, mappings);
        }


        /// <summary>
        /// 解析类型参数映射集合（默认以定义参数类型名称为键名）。
        /// </summary>
        /// <param name="accessorTypeImplementation">给定的访问器类型实现。</param>
        /// <param name="accessorMapping">输出访问器 <see cref="TypeParameterMapping"/>。</param>
        /// <returns>返回 <see cref="TypeParameterMappingCollection"/>。</returns>
        public static TypeParameterMappingCollection ParseCollection(Type accessorTypeImplementation,
            out TypeParameterMapping accessorMapping)
        {
            var accessorTypeDefinition = typeof(IDataAccessor<,,,,>);

            // 因访问器默认服务类型为 IAccessor，所以不强制实现访问器泛型类型定义
            if (!accessorTypeImplementation.IsImplementedInterfaceType(accessorTypeDefinition, out var resultType))
            {
                accessorMapping = null;
                return null;
            }

            accessorMapping = new TypeParameterMapping(accessorTypeDefinition, accessorTypeImplementation);

            var mappings = TypeParameterMappingHelper.ParseCollection(accessorTypeDefinition, resultType);

            mappings.TryFindTypeDefinitionFromValuesAndAddMapping(typeof(IGenerativeIdentifier<>));
            mappings.TryFindTypeDefinitionFromValuesAndAddMapping(typeof(IIncrementalIdentifier<>));
            mappings.TryFindTypeDefinitionFromValuesAndAddMapping(typeof(ICreation<,>));

            return mappings;
        }

    }
}
