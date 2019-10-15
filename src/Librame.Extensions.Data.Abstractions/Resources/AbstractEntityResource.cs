#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 抽象实体资源。
    /// </summary>
    public class AbstractEntityResource : IResource
    {

        #region Global Group

        /// <summary>
        /// 全局组。
        /// </summary>
        public string GlobalGroup { get; set; }

        /// <summary>
        /// 标识。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 租户标识。
        /// </summary>
        public string TenantId { get; set; }

        #endregion


        #region Data Group

        /// <summary>
        /// 数据组。
        /// </summary>
        public string DataGroup { get; set; }

        /// <summary>
        /// 排序。
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        public string Status { get; set; }

        #endregion


        #region Properties

        /// <summary>
        /// 并发标记。
        /// </summary>
        public string ConcurrencyStamp { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public string CreatedTime { get; set; }

        /// <summary>
        /// 创建时间周期数。
        /// </summary>
        public string CreatedTimeTicks { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        public string UpdatedTime { get; set; }

        /// <summary>
        /// 更新时间周期数。
        /// </summary>
        public string UpdatedTimeTicks { get; set; }

        /// <summary>
        /// 更新者。
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 是默认值。
        /// </summary>
        public string IsDefaultValue { get; set; }

        /// <summary>
        /// 启用锁定。
        /// </summary>
        public string LockoutEnabled { get; set; }

        /// <summary>
        /// 解锁时间。
        /// </summary>
        public string LockoutEndTime { get; set; }

        /// <summary>
        /// 父标识。
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 行版本。
        /// </summary>
        public string RowVersion { get; set; }

        #endregion

    }
}
