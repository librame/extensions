#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Web.Routing;

namespace System.Web.Mvc
{
    /// <summary>
    /// 容器控制器激活器。
    /// </summary>
    public class ContainerControllerActivator : IControllerActivator
    {
        /// <summary>
        /// 在类中实现时创建控制器。
        /// </summary>
        /// <param name="requestContext">请求上下文。</param>
        /// <param name="controllerType">控制器类型。</param>
        /// <returns>返回创建的控制器。</returns>
        public virtual IController Create(RequestContext requestContext, Type controllerType)
        {
            return DependencyResolver.Current.GetService(controllerType) as IController;
        }

    }
}
