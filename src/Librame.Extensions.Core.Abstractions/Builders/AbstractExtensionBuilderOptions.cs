#region License

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
    /// 抽象扩展构建器选项。
    /// </summary>
    public abstract class AbstractExtensionBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// 选项名称。
        /// </summary>
        public string OptionsName { get; set; }
    }
}
