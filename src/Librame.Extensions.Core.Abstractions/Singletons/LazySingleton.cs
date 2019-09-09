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

namespace Librame.Extensions.Core
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
            = new Lazy<TSingleton>(() => DefaultExtensions.EnsureCreate<TSingleton>());

        
        /// <summary>
        /// 得到单例。
        /// </summary>
        public static TSingleton Instance
            => _lazy.Value;
    }
}
