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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 选项配置器接口。
    /// </summary>
    public interface IOptionsConfigurator
    {
        /// <summary>
        /// 选项类型。
        /// </summary>
        Type OptionsType { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 配置。
        /// </summary>
        IConfiguration Configuration { get; set; }
    }
}
