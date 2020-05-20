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
    /// 可自动注册服务特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AutoRegisterableServiceAttribute : Attribute
    {
        /// <summary>
        /// 服务特征。
        /// </summary>
        public ServiceCharacteristics Characteristics { get; set; }

        /// <summary>
        /// 服务类型。
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// 使用第一个基础（支持抽象、接口；默认抽象优先）类型充当服务类型（默认启用；反之则使用当前类）。
        /// </summary>
        public bool UseBaseTypeAsServiceType { get; set; }
            = true;
    }
}
