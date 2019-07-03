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
    /// 构建器选项接口。
    /// </summary>
    public interface IBuilderOptions
    {
        /// <summary>
        /// 选项名称。
        /// </summary>
        string OptionsName { get; set; }
    }
}
