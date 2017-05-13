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
using System.Collections;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    /// <summary>
    /// 容器筛选器特性的筛选器提供程序。
    /// </summary>
    public class ContainerFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        /// <summary>
        /// 获取容器适配器。
        /// </summary>
        protected IContainerAdapter ContainerAdapter { get; }

        /// <summary>
        /// 构造一个 <see cref="ContainerFilterAttributeFilterProvider"/> 实例。
        /// </summary>
        /// <param name="containerAdapter">给定的容器适配器。</param>
        public ContainerFilterAttributeFilterProvider(IContainerAdapter containerAdapter)
        {
            ContainerAdapter = containerAdapter.NotNull(nameof(containerAdapter));
        }


        /// <summary>
        /// 获取控制器特性集合。
        /// </summary>
        /// <param name="controllerContext">给定的控制器上下文。</param>
        /// <param name="actionDescriptor">给定的动作描述符。</param>
        /// <returns>返回筛选器特性集合。</returns>
        protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetControllerAttributes(controllerContext, actionDescriptor);

            BuildUpAttributes(attributes);

            return attributes;
        }

        /// <summary>
        /// 获取动作特性集合。
        /// </summary>
        /// <param name="controllerContext">给定的控制器上下文。</param>
        /// <param name="actionDescriptor">给定的动作描述符。</param>
        /// <returns>返回筛选器特性集合。</returns>
        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetActionAttributes(controllerContext, actionDescriptor);

            BuildUpAttributes(attributes);

            return attributes;
        }


        private void BuildUpAttributes(IEnumerable attributes)
        {
            foreach (FilterAttribute attribute in attributes)
            {
                ContainerAdapter.BuildUp(attribute.GetType(), attribute);
            }
        }

    }
}
