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

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 容量单位计数制。
    /// </summary>
    [Description("容量单位计数制")]
    public enum CapacityUnitNotation
    {
        /// <summary>
        /// 二进制。
        /// </summary>
        [Description("二进制")]
        Binary = 2,

        /// <summary>
        /// 十进制。
        /// </summary>
        [Description("十进制")]
        Decimal = 10
    }
}
