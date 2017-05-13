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


    ///// <summary>
    ///// Librame 单例。
    ///// </summary>
    ///// <remarks>
    ///// 适用于基类与派生类实例的管理。
    ///// </remarks>
    ///// <typeparam name="TValue">指定的值类型。</typeparam>
    //public class LibrameSingletons<TValue> : KeyBuilder
    //    where TValue : class
    //{
    //    private static readonly ConcurrentDictionary<string, TValue> _instances =
    //        new ConcurrentDictionary<string, TValue>();


    //    /// <summary>
    //    /// 所有实例。
    //    /// </summary>
    //    public static ConcurrentDictionary<string, TValue> AllInstances
    //    {
    //        get { return _instances; }
    //    }


    //    /// <summary>
    //    /// 注册实例。
    //    /// </summary>
    //    /// <typeparam name="TKey">指定的键类型。</typeparam>
    //    /// <param name="addFactory">用于为键生成实例的工厂方法。</param>
    //    /// <returns>返回实例。</returns>
    //    public static TKey Regist<TKey>(Func<string, TKey> addFactory)
    //        where TKey : TValue
    //    {
    //        string key = BuildKey<TKey>();

    //        return Regist(key, addFactory);
    //    }
    //    /// <summary>
    //    /// 注册实例。
    //    /// </summary>
    //    /// <typeparam name="TKey">给定的实例类型。</typeparam>
    //    /// <param name="key">给定的键名。</param>
    //    /// <param name="addFactory">用于为键生成值的函数。</param>
    //    /// <returns>返回实例。</returns>
    //    public static TKey Regist<TKey>(string key, Func<string, TKey> addFactory)
    //        where TKey : TValue
    //    {
    //        return (TKey)_instances.AddOrUpdate(key, k => addFactory(k), (k, v) => addFactory(k));
    //    }


    //    /// <summary>
    //    /// 移除实例。
    //    /// </summary>
    //    /// <typeparam name="TKey">指定的键类型。</typeparam>
    //    /// <returns>返回实例。</returns>
    //    public static TKey Remove<TKey>()
    //        where TKey : TValue
    //    {
    //        string key = BuildKey<TKey>();
            
    //        return (TKey)Remove(key);
    //    }
    //    /// <summary>
    //    /// 移除实例。
    //    /// </summary>
    //    /// <param name="key">给定的键名。</param>
    //    /// <returns>返回实例。</returns>
    //    public static TValue Remove(string key)
    //    {
    //        TValue value;
    //        _instances.TryRemove(key, out value);

    //        return value;
    //    }


    //    /// <summary>
    //    /// 解析实例。
    //    /// </summary>
    //    /// <typeparam name="TKey">指定的键类型。</typeparam>
    //    /// <param name="addFactory">用于为键生成值的函数。</param>
    //    /// <returns>返回实例。</returns>
    //    public static TKey Resolve<TKey>(Func<string, TKey> addFactory = null)
    //        where TKey : TValue
    //    {
    //        string key = BuildKey<TKey>();

    //        return Resolve(key, addFactory);
    //    }
    //    /// <summary>
    //    /// 解析指定键的实例。
    //    /// </summary>
    //    /// <typeparam name="TKey">给定的实例类型。</typeparam>
    //    /// <param name="key">给定的键名。</param>
    //    /// <param name="addFactory">用于为键生成值的函数。</param>
    //    /// <returns>返回实例。</returns>
    //    public static TKey Resolve<TKey>(string key, Func<string, TKey> addFactory = null)
    //        where TKey : TValue
    //    {
    //        return (TKey)_instances.GetOrAdd(key, k =>
    //        {
    //            addFactory.NotNull(nameof(addFactory));

    //            return addFactory(k);
    //        });
    //    }

    //}
}
