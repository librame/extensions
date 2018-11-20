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

namespace Librame.Options
{
    /// <summary>
    /// 选项注册。
    /// </summary>
    public static class OptionsRegistration
    {
        private static ConcurrentDictionary<Type, object> _cache
            = new ConcurrentDictionary<Type, object>();


        /// <summary>
        /// 增加选项。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="options">给定的选项对象。</param>
        /// <returns>返回选项实例。</returns>
        public static TOptions AddOptions<TOptions>(object options)
        {
            return (TOptions)_cache.AddOrUpdate(typeof(TOptions), options, (key, value) => options);
        }

        /// <summary>
        /// 增加选项。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <param name="options">给定的选项对象。</param>
        /// <returns>返回选项对象。</returns>
        public static object AddOptions(Type optionsType, object options)
        {
            return _cache.AddOrUpdate(optionsType, options, (key, value) => options);
        }


        /// <summary>
        /// 获取或创建选项。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <returns>返回选项实例。</returns>
        public static TOptions GetOrCreate<TOptions>()
            where TOptions : class, new()
        {
            if (_cache.TryGetValue(typeof(TOptions), out object value))
                return value as TOptions;

            return new TOptions();
        }

    }
}
