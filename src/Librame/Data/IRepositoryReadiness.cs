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
using System.Linq;
using System.Threading.Tasks;

namespace Librame.Data
{
    /// <summary>
    /// 仓库准备接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IRepositoryReadiness<T>
    {
        /// <summary>
        /// 准备查询工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回值实例。</returns>
        TValue Ready<TValue>(Func<IQueryable<T>, TValue> factory);
    }


    /// <summary>
    /// 仓库准备静态扩展。
    /// </summary>
    public static class RepositoryReadinessExtensions
    {
        /// <summary>
        /// 异步准备查询工厂方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="repotitory">给定的仓库准备接口。</param>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回一个带值实例的异步操作。</returns>
        public static Task<TValue> ReadyAsync<T, TValue>(this IRepositoryReadiness<T> repotitory, Func<IQueryable<T>, TValue> factory)
        {
            return Task.Factory.StartNew(() => repotitory.Ready(factory));
        }

    }
}
