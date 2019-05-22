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

    }
}
