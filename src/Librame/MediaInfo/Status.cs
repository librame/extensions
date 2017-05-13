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

namespace Librame.MediaInfo
{
    /// <summary>
    /// 状态。
    /// </summary>
    [Description("状态")]
    public enum Status
    {
        /// <summary>
        /// None。
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Accepted。
        /// </summary>
        Accepted = 0x01,

        /// <summary>
        /// Filled。
        /// </summary>
        Filled = 0x02,

        /// <summary>
        /// Updated。
        /// </summary>
        Updated = 0x04,

        /// <summary>
        /// Finalized。
        /// </summary>
        Finalized = 0x08,
    }
}
