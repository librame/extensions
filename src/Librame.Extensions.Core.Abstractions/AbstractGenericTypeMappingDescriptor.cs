#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象泛型类型映射描述符。
    /// </summary>
    public abstract class AbstractGenericTypeMappingDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractGenericTypeMappingDescriptor"/>。
        /// </summary>
        /// <param name="mappings">给定的泛型类型映射字典集合。</param>
        protected AbstractGenericTypeMappingDescriptor(Dictionary<string, GenericTypeMapping> mappings)
        {
            Mappings = mappings.NotEmpty(nameof(mappings));
        }


        /// <summary>
        /// 映射字典集合。
        /// </summary>
        public IReadOnlyDictionary<string, GenericTypeMapping> Mappings { get; }


        /// <summary>
        /// 获取映射或默认值。
        /// </summary>
        /// <param name="key">给定的键名（仅支持以字母'T'开始的泛型定义参数名；键名可省略泛型定义首字母'T'，请注意原来以'T'开始的键名）。</param>
        /// <returns>返回 <see cref="GenericTypeMapping"/> 或 NULL。</returns>
        public virtual GenericTypeMapping GetMappingOrDefault(string key)
            => Mappings.TryGetValue(key.EnsureLeading('T'), out var value) ? value : null;
    }
}
