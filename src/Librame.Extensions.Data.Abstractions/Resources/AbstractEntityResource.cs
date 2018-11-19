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
    /// <summary>
    /// 抽象实体资源。
    /// </summary>
    public class AbstractEntityResource : Resources.IResource
    {

        #region Global Group

        /// <summary>
        /// 标识。
        /// </summary>
        public string Id { get; set; }

        #endregion


        #region Data Group

        /// <summary>
        /// 数据排序。
        /// </summary>
        public string DataRank { get; set; }

        /// <summary>
        /// 数据状态。
        /// </summary>
        public string DataStatus { get; set; }

        #endregion


        #region Create Group

        /// <summary>
        /// 创建时间。
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 创建者标识。
        /// </summary>
        public string CreatorId { get; set; }

        #endregion


        #region Update Group

        /// <summary>
        /// 更新时间。
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 更新者标识。
        /// </summary>
        public string UpdatorId { get; set; }

        #endregion

    }
}
