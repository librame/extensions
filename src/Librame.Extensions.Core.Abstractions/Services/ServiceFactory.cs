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

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 服务工厂委托。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <returns>返回对象。</returns>
    public delegate object ServiceFactory(Type serviceType);
}
