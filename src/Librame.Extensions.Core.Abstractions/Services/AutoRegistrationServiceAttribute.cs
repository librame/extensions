#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 自注册服务特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoRegistrationServiceAttribute : Attribute
    {
        /// <summary>
        /// 生命周期。
        /// </summary>
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Singleton;

        /// <summary>
        /// 服务类型。
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// 使用第一个基础（支持抽象、接口；默认抽象优先）类型充当服务类型（默认启用；反之则使用当前类）。
        /// </summary>
        public bool UseBaseTypeAsServiceType { get; set; } = true;
    }
}
