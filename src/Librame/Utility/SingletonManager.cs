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

namespace Librame.Utility
{
    /// <summary>
    /// 单例管理器。
    /// </summary>
    /// <remarks>
    /// 用于管理所有自由实例。
    /// </remarks>
    public class SingletonManager : KeyBuilder
    {
        private static readonly ConcurrentDictionary<string, object> _instances
            = new ConcurrentDictionary<string, object>();


        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <typeparam name="T">指定的键类型。</typeparam>
        /// <param name="addFactory">用于为键生成实例的工厂方法。</param>
        /// <returns>返回实例。</returns>
        public static T Regist<T>(Func<string, T> addFactory)
        {
            string key = BuildKey<T>();

            return Regist(key, addFactory);
        }
        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <typeparam name="T">给定的实例类型。</typeparam>
        /// <param name="key">给定的键名。</param>
        /// <param name="addFactory">用于为键生成值的函数。</param>
        /// <returns>返回实例。</returns>
        public static T Regist<T>(string key, Func<string, T> addFactory)
        {
            return (T)_instances.AddOrUpdate(key, k => addFactory(k), (k, v) => addFactory(k));
        }


        /// <summary>
        /// 解析实例。
        /// </summary>
        /// <typeparam name="T">指定的键类型。</typeparam>
        /// <param name="addFactory">用于为键生成值的函数。</param>
        /// <returns>返回实例。</returns>
        public static T Resolve<T>(Func<string, T> addFactory = null)
        {
            string key = BuildKey<T>();

            return Resolve(key, addFactory);
        }
        /// <summary>
        /// 解析指定键的实例。
        /// </summary>
        /// <typeparam name="T">给定的实例类型。</typeparam>
        /// <param name="key">给定的键名。</param>
        /// <param name="addFactory">用于为键生成值的函数。</param>
        /// <returns>返回实例。</returns>
        public static T Resolve<T>(string key, Func<string, T> addFactory = null)
        {
            return (T)_instances.GetOrAdd(key, k =>
            {
                addFactory.NotNull(nameof(addFactory));

                return addFactory(k);
            });
        }

    }
}
