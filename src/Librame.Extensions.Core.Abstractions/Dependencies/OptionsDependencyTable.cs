#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Librame.Extensions.Core.Dependencies
{
    /// <summary>
    /// 选项依赖表。
    /// </summary>
    public static class OptionsDependencyTable
    {
        private static readonly ConcurrentDictionary<Type, KeyValuePair<object, IDependency>> _optionsDependencies
            = new ConcurrentDictionary<Type, KeyValuePair<object, IDependency>>();


        /// <summary>
        /// 清除所有选项依赖。
        /// </summary>
        public static void Clear()
            => _optionsDependencies.Clear();


        /// <summary>
        /// 添加或更新选项依赖。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="options">给定的选项。</param>
        /// <param name="dependency">给定的 <see cref="IDependency"/>。</param>
        /// <returns>返回 <see cref="IDependency"/>。</returns>
        public static object AddOrUpdate<TOptions>(object options, IDependency dependency)
            => AddOrUpdate(typeof(TOptions), new KeyValuePair<object, IDependency>(options, dependency));

        /// <summary>
        /// 添加或更新选项依赖。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <param name="options">给定的选项。</param>
        /// <param name="dependency">给定的 <see cref="IDependency"/>。</param>
        /// <returns>返回 <see cref="IDependency"/>。</returns>
        public static object AddOrUpdate(Type optionsType, object options, IDependency dependency)
            => AddOrUpdate(optionsType, new KeyValuePair<object, IDependency>(options, dependency));

        /// <summary>
        /// 添加或更新选项依赖。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <param name="optionsDependency">给定的选项依赖键值对。</param>
        /// <returns>返回 <see cref="IDependency"/>。</returns>
        public static object AddOrUpdate(Type optionsType, KeyValuePair<object, IDependency> optionsDependency)
            => _optionsDependencies.AddOrUpdate(optionsType, optionsDependency, (key, value) => optionsDependency);


        /// <summary>
        /// 尝试获取指定选项类型的依赖。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="options">输出选项。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGet<TOptions>(out TOptions options)
        {
            if (TryGet(typeof(TOptions), out KeyValuePair<object, IDependency> optionsDependency))
            {
                options = (TOptions)optionsDependency.Key;
                return true;
            }

            options = default;
            return false;
        }

        /// <summary>
        /// 尝试获取指定选项类型的依赖。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <param name="optionsDependency">输出选项依赖键值对。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGet(Type optionsType, out KeyValuePair<object, IDependency> optionsDependency)
            => _optionsDependencies.TryGetValue(optionsType, out optionsDependency);


        /// <summary>
        /// 尝试移除指定选项类型的依赖。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="options">输出选项。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryRemove<TOptions>(out TOptions options)
        {
            if (TryRemove(typeof(TOptions), out KeyValuePair<object, IDependency> optionsDependency))
            {
                options = (TOptions)optionsDependency.Key;
                return true;
            }

            options = default;
            return false;
        }

        /// <summary>
        /// 尝试移除指定选项类型的依赖。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <param name="optionsDependency">输出选项依赖键值对。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryRemove(Type optionsType, out KeyValuePair<object, IDependency> optionsDependency)
            => _optionsDependencies.TryRemove(optionsType, out optionsDependency);
    }
}
