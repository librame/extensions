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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 扩展构建器依赖选项。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public class ExtensionBuilderDependencyOptions<TBuilderOptions> : IExtensionBuilderDependencyOptions
        where TBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// <typeparamref name="TBuilderOptions"/> 配置动作。
        /// </summary>
        public Action<TBuilderOptions> SetupAction { get; set; }
    }
}
