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
    /// 动作状态。
    /// </summary>
    [Description("动作状态")]
    public enum ActionStatus
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 列表。
        /// </summary>
        [Description("列表")]
        Index = 1,

        /// <summary>
        /// 详情。
        /// </summary>
        [Description("详情")]
        Details = 2,

        /// <summary>
        /// 新增。
        /// </summary>
        [Description("新增")]
        Create = 4,

        /// <summary>
        /// 修改。
        /// </summary>
        [Description("修改")]
        Edit = 8,

        /// <summary>
        /// 删除。
        /// </summary>
        [Description("删除")]
        Delete = 16
    }
}
