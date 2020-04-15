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

namespace Librame.Extensions.Core.Options
{
    /// <summary>
    /// 一致性选项池。
    /// </summary>
    public static class ConsistencyOptionsPool
    {
        private static readonly ConcurrentDictionary<Type, object> _optionsPool
            = new ConcurrentDictionary<Type, object>();


        /// <summary>
        /// 清空选项池。
        /// </summary>
        public static void Clear()
            => _optionsPool.Clear();


        /// <summary>
        /// 添加或更新选项。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
        /// <returns>返回 <typeparamref name="TOptions"/>。</returns>
        public static TOptions AddOrUpdate<TOptions>(TOptions options)
            where TOptions : class
            => (TOptions)_optionsPool.AddOrUpdate(typeof(TOptions), options, (key, value) => options);

        /// <summary>
        /// 添加或更新选项。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <param name="options">给定的选项对象。</param>
        /// <returns>返回选项对象。</returns>
        public static object AddOrUpdate(Type optionsType, object options)
            => _optionsPool.AddOrUpdate(optionsType, options, (key, value) => options);


        /// <summary>
        /// 获取或添加选项。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <returns>返回 <typeparamref name="TOptions"/>。</returns>
        public static TOptions GetOrAdd<TOptions>()
            => (TOptions)_optionsPool.GetOrAdd(typeof(TOptions),
                key => key.EnsureCreate<TOptions>());

        /// <summary>
        /// 获取或添加选项。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <returns>返回选项对象。</returns>
        public static object GetOrAdd(Type optionsType)
            => _optionsPool.GetOrAdd(optionsType, key => key.EnsureCreateObject());


        /// <summary>
        /// 尝试获取选项。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="options">输出 <typeparamref name="TOptions"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet<TOptions>(out TOptions options)
            where TOptions : class
        {
            var result = _optionsPool.TryGetValue(typeof(TOptions), out object obj);
            options = obj as TOptions;
            return result;
        }

        /// <summary>
        /// 尝试获取选项。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <param name="options">输出选项对象。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet(Type optionsType, out object options)
            => _optionsPool.TryGetValue(optionsType, out options);


        /// <summary>
        /// 尝试移除选项。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="options">输出 <typeparamref name="TOptions"/>。</param>
        /// <returns>返回是否成功移除的布尔值。</returns>
        public static bool TryRemove<TOptions>(out TOptions options)
            where TOptions : class
        {
            var result = _optionsPool.TryRemove(typeof(TOptions), out object obj);
            options = obj as TOptions;
            return result;
        }

        /// <summary>
        /// 尝试移除选项。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <param name="options">输出选项对象。</param>
        /// <returns>返回是否成功移除的布尔值。</returns>
        public static bool TryRemove(Type optionsType, out object options)
            => _optionsPool.TryRemove(optionsType, out options);
    }
}
