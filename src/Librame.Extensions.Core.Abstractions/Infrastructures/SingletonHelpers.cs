﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 单例助手。
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/leolion/p/10275027.html
    /// </remarks>
    public static class SingletonHelper
    {
        /// <summary>
        /// 获取指定类型的单例。
        /// </summary>
        /// <typeparam name="TSingleton">指定要创建或获取的单例类型。</typeparam>
        /// <returns>返回单例。</returns>
        public static TSingleton GetInstance<TSingleton>()
            where TSingleton : class, new()
        {
            return SingletonHelper<TSingleton>.Instance;
        }
    }


    /// <summary>
    /// 单例助手。
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/leolion/p/10275027.html
    /// </remarks>
    /// <typeparam name="TSingleton">指定要创建或获取的单例类型。</typeparam>
    public static class SingletonHelper<TSingleton>
        where TSingleton : class, new()
    {
        /// <summary>
        /// 获取给定类型的单例。
        /// </summary>
        private static readonly Lazy<TSingleton> _lazy
            = new Lazy<TSingleton>(() => new TSingleton());

        /// <summary>
        /// 获取给定类型的单例。
        /// </summary>
        public static TSingleton Instance => _lazy.Value;
    }
}
