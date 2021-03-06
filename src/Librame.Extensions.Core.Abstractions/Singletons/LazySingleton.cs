﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Singletons
{
    /// <summary>
    /// 延迟单例。
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/leolion/p/10275027.html
    /// </remarks>
    public static class LazySingleton
    {
        /// <summary>
        /// 获取指定类型的单例。
        /// </summary>
        /// <typeparam name="TSingleton">指定要创建或获取的单例类型。</typeparam>
        /// <returns>返回单例。</returns>
        public static TSingleton GetInstance<TSingleton>()
            where TSingleton : class
            => LazySingleton<TSingleton>.Instance;
    }


    /// <summary>
    /// 延迟单例。
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/leolion/p/10275027.html
    /// </remarks>
    /// <typeparam name="TSingleton">指定要创建或获取的单例类型。</typeparam>
    public static class LazySingleton<TSingleton>
        where TSingleton : class
    {
        /// <summary>
        /// 表示在每个线程中都是唯一。
        /// </summary>
        [ThreadStatic]
        private static readonly Lazy<TSingleton> _lazy
            = new Lazy<TSingleton>(() => ObjectExtensions.EnsureCreate<TSingleton>());


        /// <summary>
        /// 得到单例。
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TSingleton Instance
            => _lazy.Value;
    }
}
