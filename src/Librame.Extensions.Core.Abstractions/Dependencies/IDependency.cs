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
using System;

namespace Librame.Extensions.Core.Dependencies
{
    using Serializers;

    /// <summary>
    /// 依赖接口。
    /// </summary>
    public interface IDependency
    {
        /// <summary>
        /// 依赖名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 依赖类型。
        /// </summary>
        SerializableString<Type> Type { get; }

        /// <summary>
        /// 依赖配置。
        /// </summary>
        IConfiguration Configuration { get; set; }
    }
}
