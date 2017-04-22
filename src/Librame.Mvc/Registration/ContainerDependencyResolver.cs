#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Container;
using Librame.Utility;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    /// <summary>
    /// 容器依赖解析器。
    /// </summary>
    public class ContainerDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// 获取容器适配器。
        /// </summary>
        protected IContainerAdapter ContainerAdapter { get; }

        /// <summary>
        /// 构造一个 <see cref="ContainerDependencyResolver"/> 实例。
        /// </summary>
        /// <param name="containerAdapter">给定的容器适配器。</param>
        public ContainerDependencyResolver(IContainerAdapter containerAdapter)
        {
            ContainerAdapter = containerAdapter.NotNull(nameof(containerAdapter));
        }


        /// <summary>
        /// 解析支持任意对象创建的一次注册的服务。
        /// </summary>
        /// <param name="serviceType">给定所请求的服务或对象的类型。</param>
        /// <returns>返回请求的服务或对象。</returns>
        public object GetService(Type serviceType)
        {
            if (typeof(IController).IsAssignableFrom(serviceType))
            {
                return ContainerAdapter.Resolve(serviceType);
            }

            try
            {
                // 如果未注册，则返回空（表示由 ASP.NET MVC 自己解析）
                if (!ContainerAdapter.IsRegistered(serviceType))
                    return null;

                return ContainerAdapter.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// 解析多次注册的服务。
        /// </summary>
        /// <param name="serviceType">给定所请求的服务的类型。</param>
        /// <returns>请求的服务。</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ContainerAdapter.ResolveAll(serviceType);
        }

    }
}
