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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Core.Mappers
{
    /// <summary>
    /// 抽象类型参数映射器。
    /// </summary>
    public abstract class AbstractTypeParameterMapper : IEnumerable<KeyValuePair<string, TypeParameterMapping>>,
        IReadOnlyCollection<KeyValuePair<string, TypeParameterMapping>>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractTypeParameterMapper"/>。
        /// </summary>
        /// <param name="mappings">给定的类型参数映射字典集合。</param>
        protected AbstractTypeParameterMapper(Dictionary<string, TypeParameterMapping> mappings)
            : this(mappings as IReadOnlyDictionary<string, TypeParameterMapping>)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractTypeParameterMapper"/>。
        /// </summary>
        /// <param name="mappings">给定的类型参数映射字典集合。</param>
        protected AbstractTypeParameterMapper(IReadOnlyDictionary<string, TypeParameterMapping> mappings)
        {
            Mappings = mappings.NotEmpty(nameof(mappings));
        }


        /// <summary>
        /// 类型参数映射字典集合。
        /// </summary>
        protected IReadOnlyDictionary<string, TypeParameterMapping> Mappings { get; }


        /// <summary>
        /// 参数类型映射计数。
        /// </summary>
        public int Count
            => Mappings.Count;

        /// <summary>
        /// 得到指定键名的类型参数映射。
        /// </summary>
        /// <param name="key">指定的键名。</param>
        /// <returns>返回 <see cref="TypeParameterMapping"/>。</returns>
        public TypeParameterMapping this[string key]
            => Mappings[key];


        /// <summary>
        /// 获取泛型类型参数映射的调用实参类型。
        /// </summary>
        /// <param name="key">给定的键名（已确保键名首字母以'T'字符开始。如果键名本身以'T'字符开始，请自己输入完整键名；如'TTenant'）。</param>
        /// <returns>返回 <see cref="Type"/>。</returns>
        public virtual Type GetGenericMappingArgumentType(string key)
            => GetGenericMapping(key).ArgumentType;

        /// <summary>
        /// 获取泛型类型参数映射。
        /// </summary>
        /// <param name="key">给定的键名（已确保键名首字母以'T'字符开始。如果键名本身以'T'字符开始，请自己输入完整键名；如'TTenant'）。</param>
        /// <returns>返回 <see cref="TypeParameterMapping"/>。</returns>
        public virtual TypeParameterMapping GetGenericMapping(string key)
            => Mappings[key.EnsureLeading('T')];


        /// <summary>
        /// 获取类型参数映射的调用实参类型。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <returns>返回 <see cref="Type"/>。</returns>
        public virtual Type GetMappingArgumentType(string key)
            => Mappings[key].ArgumentType;

        /// <summary>
        /// 获取类型参数映射。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <returns>返回 <see cref="TypeParameterMapping"/>。</returns>
        public virtual TypeParameterMapping GetMapping(string key)
            => Mappings[key];


        /// <summary>
        /// 尝试获取类型参数映射。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <param name="mapping">输出 <see cref="TypeParameterMapping"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool TryGetMapping(string key, out TypeParameterMapping mapping)
            => Mappings.TryGetValue(key, out mapping);


        /// <summary>
        /// 填充泛型类型定义。
        /// </summary>
        /// <param name="typeDefinition">给定的类型定义。</param>
        /// <param name="populateTypeArgumentsFactory">给定的填充调用类型实参集合工厂方法（可选；默认将当前映射字典集合中包含的所有调用实参类型集合填充到指定的泛型类型定义）。</param>
        /// <returns>返回类型。</returns>
        public virtual Type PopulateGenericTypeDefinition(Type typeDefinition,
            Func<Type[]> populateTypeArgumentsFactory = null)
        {
            typeDefinition.NotNull(nameof(typeDefinition));

            if (false == typeDefinition?.IsGenericTypeDefinition)
                throw new NotSupportedException($"The type definition '{typeDefinition}' only support generic type definition.");

            if (populateTypeArgumentsFactory.IsNull())
                populateTypeArgumentsFactory = () => Mappings.Values.Select(mapping => mapping.ArgumentType).ToArray();

            return typeDefinition.MakeGenericType(populateTypeArgumentsFactory.Invoke());
        }


        /// <summary>
        /// 获取枚举器。
        /// </summary>
        /// <returns>返回 <see cref="IEnumerator{T}"/>。</returns>
        public virtual IEnumerator<KeyValuePair<string, TypeParameterMapping>> GetEnumerator()
            => Mappings.GetEnumerator();

        /// <summary>
        /// 获取枚举器。
        /// </summary>
        /// <returns>返回 <see cref="IEnumerator"/>。</returns>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

    }
}
