#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Resources
{
    using Core.Resources;

    /// <summary>
    /// 数据状态资源。
    /// </summary>
    public class DataStatusResource : IResource
    {

        #region Groups

        /// <summary>
        /// 全局组。
        /// </summary>
        public string GlobalGroup { get; set; }

        /// <summary>
        /// 范围组。
        /// </summary>
        public string ScopeGroup { get; set; }

        /// <summary>
        /// 状态组。
        /// </summary>
        public string StateGroup { get; set; }

        #endregion


        #region Global Group

        /// <summary>
        /// 默认。
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// 删除。
        /// </summary>
        public string Delete { get; set; }

        #endregion


        #region Scope Group

        /// <summary>
        /// 公开。
        /// </summary>
        public string Public { get; set; }

        /// <summary>
        /// 保护。
        /// </summary>
        public string Protect { get; set; }

        /// <summary>
        /// 内部。
        /// </summary>
        public string Internal { get; set; }

        /// <summary>
        /// 私有。
        /// </summary>
        public string Private { get; set; }

        #endregion


        #region State Group

        /// <summary>
        /// 活动。
        /// </summary>
        public string Active { get; set; }

        /// <summary>
        /// 挂起。
        /// </summary>
        public string Pending { get; set; }

        /// <summary>
        /// 闲置。
        /// </summary>
        public string Inactive { get; set; }

        /// <summary>
        /// 锁定。
        /// </summary>
        public string Locking { get; set; }

        /// <summary>
        /// 禁止。
        /// </summary>
        public string Ban { get; set; }

        #endregion

    }
}
