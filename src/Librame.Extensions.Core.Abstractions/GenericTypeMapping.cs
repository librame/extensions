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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 泛型类型映射。
    /// </summary>
    public class GenericTypeMapping : IEquatable<GenericTypeMapping>
    {
        /// <summary>
        /// 构造一个 <see cref="GenericTypeMapping"/>。
        /// </summary>
        /// <param name="parameterType">给定的定义参数类型。</param>
        /// <param name="argumentType">给定的实现参数类型（可选）。</param>
        public GenericTypeMapping(Type parameterType, Type argumentType = null)
        {
            ParameterType = parameterType.NotNull(nameof(parameterType));
            ArgumentType = argumentType;
        }


        /// <summary>
        /// 定义参数类型。
        /// </summary>
        public Type ParameterType { get; }

        /// <summary>
        /// 实现参数类型。
        /// </summary>
        public Type ArgumentType { get; set; }


        /// <summary>
        /// 定义参数名称。
        /// </summary>
        public string ParameterName
            => ParameterType.Name;

        /// <summary>
        /// 定义参数名称。
        /// </summary>
        public string ArgumentName
            => ArgumentType?.Name;


        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="other">给定的 <see cref="GenericTypeMapping"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(GenericTypeMapping other)
            => ParameterType == other?.ParameterType && ArgumentType == other.ArgumentType;

        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => Equals(obj as GenericTypeMapping);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
        {
            if (ArgumentType.IsNull())
                return ParameterType.GetHashCode();
            
            return ParameterType.GetHashCode() ^ ArgumentType.GetHashCode();
        }


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
        /// 填充数组。
        /// </summary>
        /// <param name="genericTypeDefinition">给定的泛型类型定义。</param>
        /// <param name="genericTypeImplementation">给定的泛型类型实现。</param>
        /// <returns>返回 <see cref="GenericTypeMapping"/> 数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static GenericTypeMapping[] PopulateArray(Type genericTypeDefinition, Type genericTypeImplementation)
        {
            ValidGenericType(genericTypeDefinition, genericTypeImplementation);

            return PopulateArray(genericTypeDefinition.GetTypeInfo().GenericTypeParameters,
                genericTypeImplementation.GenericTypeArguments);
        }

        /// <summary>
        /// 填充数组。
        /// </summary>
        /// <param name="parameterTypes">给定的定义参数类型数组。</param>
        /// <param name="argumentTypes">给定的实现参数类型数组。</param>
        /// <returns>返回 <see cref="GenericTypeMapping"/> 数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static GenericTypeMapping[] PopulateArray(Type[] parameterTypes, Type[] argumentTypes)
        {
            ValidGenericTypes(parameterTypes, argumentTypes);
            
            var parameters = new GenericTypeMapping[parameterTypes.Length];

            for (var i = 0; i < parameterTypes.Length; i++)
            {
                parameters[i] = new GenericTypeMapping(parameterTypes[i], argumentTypes[i]);
            }

            return parameters;
        }


        /// <summary>
        /// 填充数组。
        /// </summary>
        /// <param name="genericTypeDefinition">给定的泛型类型定义。</param>
        /// <param name="genericTypeImplementation">给定的泛型类型实现。</param>
        /// <returns>返回 <see cref="Dictionary{String, GenericTypeParameter}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static Dictionary<string, GenericTypeMapping> PopulateDictionary(Type genericTypeDefinition, Type genericTypeImplementation)
        {
            ValidGenericType(genericTypeDefinition, genericTypeImplementation);

            return PopulateDictionary(genericTypeDefinition.GetTypeInfo().GenericTypeParameters,
                genericTypeImplementation.GenericTypeArguments);
        }

        /// <summary>
        /// 填充字典（默认以定义参数类型名称为键名）。
        /// </summary>
        /// <param name="parameterTypes">给定的定义参数类型数组。</param>
        /// <param name="argumentTypes">给定的实现参数类型数组。</param>
        /// <returns>返回 <see cref="Dictionary{String, GenericTypeParameter}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static Dictionary<string, GenericTypeMapping> PopulateDictionary(Type[] parameterTypes, Type[] argumentTypes)
        {
            ValidGenericTypes(parameterTypes, argumentTypes);

            var parameters = new Dictionary<string, GenericTypeMapping>();

            for (var i = 0; i < parameterTypes.Length; i++)
            {
                var parameter = parameterTypes[i];
                parameters.Add(parameter.Name, new GenericTypeMapping(parameter, argumentTypes[i]));
            }

            return parameters;
        }

    }
}
