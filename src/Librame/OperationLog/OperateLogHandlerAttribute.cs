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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Librame.OperationLog
{
    using Utility;

    /// <summary>
    /// 操作日志处理程序属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class OperateLogHandlerAttribute : HandlerAttribute
    {
        /// <summary>
        /// 创建操作日志访问处理程序。
        /// </summary>
        /// <param name="container">给定的 <see cref="IUnityContainer"/>。</param>
        /// <returns>返回访问处理程序。</returns>
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            container.NotNull(nameof(container));

            var adapter = container.Resolve<IOperationLogAdapter>();

            var descriptor = adapter.Factory.GetInstance();
            var provider = container.Resolve<IOperateLogProvider>();

            var handler = new OperateLogCallHandler(descriptor, provider);
            handler.Order = Order;

            return handler;
        }

    }
}
