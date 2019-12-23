#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;

namespace Librame.Extensions.Core.Dependencies
{
    /// <summary>
    /// 依赖根接口。
    /// </summary>
    public interface IDependencyRoot : IDependency
    {
        /// <summary>
        /// 依赖配置根。
        /// </summary>
        IConfigurationRoot ConfigurationRoot { get; set; }
    }
}
