﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 扩展构建器选项接口。
    /// </summary>
    public interface IExtensionBuilderOptions
    {
        /// <summary>
        /// 选项名称（兼容 ConfigurationSourceBinder 设计）。
        /// </summary>
        string OptionsName { get; set; }
    }
}