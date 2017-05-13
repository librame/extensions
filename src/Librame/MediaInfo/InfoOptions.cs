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
    /// 信息选项。
    /// </summary>
    [Description("信息选项")]
    public enum InfoOptions
    {
        /// <summary>
        /// ShowInInform。
        /// </summary>
        [Description("ShowInInform")]
        ShowInInform,

        /// <summary>
        /// Support。
        /// </summary>
        [Description("Support")]
        Support,

        /// <summary>
        /// ShowInSupported。
        /// </summary>
        [Description("ShowInSupported")]
        ShowInSupported,

        /// <summary>
        /// TypeOfValue。
        /// </summary>
        [Description("TypeOfValue")]
        TypeOfValue
    }
}
