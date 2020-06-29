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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Librame.Extensions.Core.Mappers
{
    /// <summary>
    /// 类型参数映射助手。
    /// </summary>
    public static class TypeParameterMappingHelper
    {
        private static void ValidGenericType(Type genericTypeDefinition, Type genericTypeImplementation)
        {
            genericTypeDefinition.NotNull(nameof(genericTypeDefinition));
            genericTypeImplementation.NotNull(nameof(genericTypeImplementation));

            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new NotSupportedException($"Unsupported generic type definition '{genericTypeDefinition}'.");

            if (!genericTypeImplementation.IsGenericType)
                throw new NotSupportedException($"Unsupported generic type implementation '{genericTypeImplementation}'.");

            if (genericTypeImplementation.IsGenericTypeDefinition)
                throw new NotSupportedException($"The generic type implementation '{genericTypeImplementation}' can't be generic type definition.");
        }

        private static void ValidGenericTypes(Type[] parameterTypes, Type[] argumentTypes)
        {
            parameterTypes.NotNull(nameof(parameterTypes));
            argumentTypes.NotNull(nameof(argumentTypes));

            if (parameterTypes.Length != argumentTypes.Length)
                throw new InvalidOperationException($"Unmatched parameter types length '{parameterTypes.Length}' and argument types length '{argumentTypes.Length}'.");
        }


        /// <summary>
        /// 解析为数组。
        /// </summary>
        /// <param name="genericTypeDefinition">给定的泛型类型定义。</param>
        /// <param name="genericTypeImplementation">给定的泛型类型实现。</param>
        /// <returns>返回 <see cref="TypeParameterMapping"/> 数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static TypeParameterMapping[] ParseAsArray(Type genericTypeDefinition, Type genericTypeImplementation)
        {
            ValidGenericType(genericTypeDefinition, genericTypeImplementation);

            return ParseAsArray(genericTypeDefinition.GetTypeInfo().GenericTypeParameters,
                genericTypeImplementation.GenericTypeArguments);
        }

        /// <summary>
        /// 解析为数组。
        /// </summary>
        /// <param name="parameterTypes">给定的定义参数类型数组。</param>
        /// <param name="argumentTypes">给定的实现参数类型数组。</param>
        /// <returns>返回 <see cref="TypeParameterMapping"/> 数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static TypeParameterMapping[] ParseAsArray(Type[] parameterTypes, Type[] argumentTypes)
        {
            ValidGenericTypes(parameterTypes, argumentTypes);

            var parameters = new TypeParameterMapping[parameterTypes.Length];

            for (var i = 0; i < parameterTypes.Length; i++)
            {
                parameters[i] = new TypeParameterMapping(parameterTypes[i], argumentTypes[i]);
            }

            return parameters;
        }


        /// <summary>
        /// 解析为字典。
        /// </summary>
        /// <param name="genericTypeDefinition">给定的泛型类型定义。</param>
        /// <param name="genericTypeImplementation">给定的泛型类型实现。</param>
        /// <returns>返回 <see cref="Dictionary{String, GenericTypeParameter}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static Dictionary<string, TypeParameterMapping> ParseAsDictionary(Type genericTypeDefinition, Type genericTypeImplementation)
        {
            ValidGenericType(genericTypeDefinition, genericTypeImplementation);

            return ParseAsDictionary(genericTypeDefinition.GetTypeInfo().GenericTypeParameters,
                genericTypeImplementation.GenericTypeArguments);
        }

        /// <summary>
        /// 解析为字典（默认以定义参数类型名称为键名）。
        /// </summary>
        /// <param name="parameterTypes">给定的定义参数类型数组。</param>
        /// <param name="argumentTypes">给定的实现参数类型数组。</param>
        /// <returns>返回 <see cref="Dictionary{String, GenericTypeParameter}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static Dictionary<string, TypeParameterMapping> ParseAsDictionary(Type[] parameterTypes, Type[] argumentTypes)
        {
            ValidGenericTypes(parameterTypes, argumentTypes);

            var parameters = new Dictionary<string, TypeParameterMapping>();

            for (var i = 0; i < parameterTypes.Length; i++)
            {
                var parameter = parameterTypes[i];
                parameters.Add(parameter.Name, new TypeParameterMapping(parameter, argumentTypes[i]));
            }

            return parameters;
        }

    }
}
