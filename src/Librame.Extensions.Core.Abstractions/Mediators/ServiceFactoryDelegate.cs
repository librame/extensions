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
using System.Collections.Generic;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 用于解析所有服务的工厂方法。对于多个实例，它将根据 <see cref="IEnumerable{T}"/> 解析。
    /// </summary>
    /// <param name="serviceType">指定的解析服务类型。</param>
    /// <returns>返回 <paramref name="serviceType" /> 的实例。</returns>
    public delegate object ServiceFactoryDelegate(Type serviceType);


    /// <summary>
    /// 服务工厂委托静态扩展。
    /// </summary>
    public static class ServiceFactoryDelegateExtensions
    {
        /// <summary>
        /// 调用处理程序。
        /// </summary>
        /// <typeparam name="THandler">指定的处理程序类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <returns>返回 <typeparamref name="THandler"/>。</returns>
        public static THandler InvokeHandler<THandler>(this ServiceFactoryDelegate serviceFactory)
            where THandler : class
        {
            THandler handler = null;

            try
            {
                handler = serviceFactory.Invoke<THandler>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error constructing handler for request of type {typeof(THandler)}. Register your handlers with the container. See the samples in GitHub for examples.", e);
            }

            if (handler.IsNull())
            {
                throw new InvalidOperationException($"Handler was not found for request of type {typeof(THandler)}. Register your handlers with the container. See the samples in GitHub for examples.");
            }

            return handler;
        }

        /// <summary>
        /// 调用委托。
        /// </summary>
        /// <typeparam name="TService">指定的类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <returns>返回 <typeparamref name="TService"/>。</returns>
        public static TService Invoke<TService>(this ServiceFactoryDelegate serviceFactory)
            => (TService)serviceFactory.Invoke(typeof(TService));

        /// <summary>
        /// 调用可枚举委托。
        /// </summary>
        /// <typeparam name="TService">指定的类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <returns>返回 <see cref="IEnumerable{TService}"/>。</returns>
        public static IEnumerable<TService> Invokes<TService>(this ServiceFactoryDelegate serviceFactory)
            => (IEnumerable<TService>)serviceFactory(typeof(IEnumerable<TService>));
    }
}
