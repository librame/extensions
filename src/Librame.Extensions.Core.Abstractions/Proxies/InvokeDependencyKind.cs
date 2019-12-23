#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Extensions.Core.Proxies
{
    /// <summary>
    /// 调用依赖种类。
    /// </summary>
    [Description("调用依赖种类")]
    public enum InvokeDependencyKind
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        [DefaultValue("")]
        Default,

        /// <summary>
        /// 属性读写访问器。
        /// </summary>
        [Description("属性读写访问器")]
        [DefaultValue("get_,set_")]
        Property,

        /// <summary>
        /// 属性读取访问器。
        /// </summary>
        [Description("属性读取访问器")]
        [DefaultValue("get_")]
        PropertyGet,

        /// <summary>
        /// 属性写入访问器。
        /// </summary>
        [Description("属性写入访问器")]
        [DefaultValue("set_")]
        PropertySet
    }
}
