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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据状态。
    /// </summary>
    [Flags]
    [Description("数据状态")]
    public enum DataStatus
    {

        #region Global

        /// <summary>
        /// 默认。
        /// </summary>
        //[Display(Name = "DefaultName",
        //    ShortName = "DefaultShortName",
        //    Description = "DefaultDescription",
        //    Prompt = "DefaultPrompt",
        //    GroupName = "GlobalGroup",
        //    ResourceType = typeof(Resources.DataStatus),
        //    Order = -9
        //)]
        None = -1,

        /// <summary>
        /// 删除的。
        /// </summary>
        //[Display(Name = "DeletedName",
        //    ShortName = "DeletedShortName",
        //    Description = "DeletedDescription",
        //    Prompt = "DeletedPrompt",
        //    GroupName = "StateGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Deleted = 0,

        #endregion


        #region Scope

        /// <summary>
        /// 公开的。
        /// </summary>
        //[Display(Name = "PublicName",
        //    ShortName = "PublicShortName",
        //    Description = "PublicDescription",
        //    Prompt = "PublicPrompt",
        //    GroupName = "ScopeGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Public = 1,

        /// <summary>
        /// 受保护。
        /// </summary>
        //[Display(Name = "ProtectedName",
        //    ShortName = "ProtectedShortName",
        //    Description = "ProtectedDescription",
        //    Prompt = "ProtectedPrompt",
        //    GroupName = "ScopeGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Protected = 2,

        /// <summary>
        /// 内部的。
        /// </summary>
        //[Display(Name = "InternalName",
        //    ShortName = "InternalShortName",
        //    Description = "InternalDescription",
        //    Prompt = "InternalPrompt",
        //    GroupName = "ScopeGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Internal = 4,

        /// <summary>
        /// 私有的。
        /// </summary>
        //[Display(Name = "PrivateName",
        //    ShortName = "PrivateShortName",
        //    Description = "PrivateDescription",
        //    Prompt = "PrivatePrompt",
        //    GroupName = "ScopeGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Private = 8,

        #endregion


        #region State

        /// <summary>
        /// 活跃的。
        /// </summary>
        //[Display(Name = "ActiveName",
        //    ShortName = "ActiveShortName",
        //    Description = "ActiveDescription",
        //    Prompt = "ActivePrompt",
        //    GroupName = "StateGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Active = 16,

        /// <summary>
        /// 挂起的。
        /// </summary>
        //[Display(Name = "PendingName",
        //    ShortName = "PendingShortName",
        //    Description = "PendingDescription",
        //    Prompt = "PendingPrompt",
        //    GroupName = "StateGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Pending = 32,

        /// <summary>
        /// 闲置的。
        /// </summary>
        //[Display(Name = "InactiveName",
        //    ShortName = "InactiveShortName",
        //    Description = "InactiveDescription",
        //    Prompt = "InactivePrompt",
        //    GroupName = "StateGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Inactive = 64,

        /// <summary>
        /// 锁定的。
        /// </summary>
        //[Display(Name = "LockedName",
        //    ShortName = "LockedShortName",
        //    Description = "LockedDescription",
        //    Prompt = "LockedPrompt",
        //    GroupName = "StateGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Locked = 128,

        /// <summary>
        /// 被禁的。
        /// </summary>
        //[Display(Name = "BannedName",
        //    ShortName = "BannedShortName",
        //    Description = "BannedDescription",
        //    Prompt = "BannedPrompt",
        //    GroupName = "StateGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Banned = 256,

        /// <summary>
        /// 废弃的。
        /// </summary>
        //[Display(Name = "ObsoletedName",
        //    ShortName = "ObsoletedShortName",
        //    Description = "ObsoletedDescription",
        //    Prompt = "ObsoletedPrompt",
        //    GroupName = "StateGroup",
        //    ResourceType = typeof(Resources.DataStatus)
        //)]
        Obsoleted = 512
        
        #endregion
        
    }
}