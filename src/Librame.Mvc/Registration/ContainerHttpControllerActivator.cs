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
using System.Net.Http;
using System.Web.Http.Controllers;

namespace System.Web.Http.Dispatcher
{
    /// <summary>
    /// 容器类 HTTP 控制器激活器。
    /// </summary>
    public class ContainerHttpControllerActivator : IHttpControllerActivator
    {
        /// <summary>
        /// 获取容器适配器。
        /// </summary>
        protected IContainerAdapter ContainerAdapter { get; }

        /// <summary>
        /// 构造一个 <see cref="ContainerHttpControllerActivator"/> 实例。
        /// </summary>
        /// <param name="containerAdapter">给定的容器适配器。</param>
        public ContainerHttpControllerActivator(IContainerAdapter containerAdapter)
        {
            ContainerAdapter = containerAdapter.NotNull(nameof(containerAdapter));
        }


        /// <summary>
        /// 创建 HTTP 控制器。
        /// </summary>
        /// <param name="request">给定的 HTTP 请求消息。</param>
        /// <param name="controllerDescriptor">给定的 HTTP 控制器描述符。</param>
        /// <param name="controllerType">给定的容器类型。</param>
        /// <returns>返回 HTTP 控制器。</returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return (IHttpController)ContainerAdapter.Resolve(controllerType);
        }

    }
}
