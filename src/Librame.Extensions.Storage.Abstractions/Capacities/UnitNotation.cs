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

namespace Librame.Extensions.Storage.Capacities
{
    /// <summary>
    /// 单位计数制。
    /// </summary>
    [Description("单位计数制")]
    public enum UnitNotation
    {
        /// <summary>
        /// 二进制计数。
        /// </summary>
        [Description("二进制计数")]
        BinarySystem = 2,

        /// <summary>
        /// 十进制计数。
        /// </summary>
        [Description("十进制计数")]
        DecimalSystem = 10
    }
}
