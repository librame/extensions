#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace System.Web.Mvc
{
    /// <summary>
    /// 注意级别。
    /// </summary>
    [Description("注意级别")]
    public enum AttentionLevel
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 信息。
        /// </summary>
        [Description("信息")]
        Info = 1,

        /// <summary>
        /// 阻止。
        /// </summary>
        [Description("阻止")]
        Block = 2,

        /// <summary>
        /// 错误。
        /// </summary>
        [Description("错误")]
        Error = 4,

        /// <summary>
        /// 成功。
        /// </summary>
        [Description("成功")]
        Success = 8
    }
}
