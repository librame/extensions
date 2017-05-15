#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Data;
using Librame.Data.Descriptors;
using System;
using System.ComponentModel;

namespace Librame.Tests.Data.Entities
{
    /// <summary>
    /// 国家或地区。
    /// </summary>
    [Description("国家或地区")]
    public class Region : AbstractDataIdDescriptor<int>, IParentIdDescriptor<int>, ICreateIdDescriptor<int>, IEntityAutomapping
    {
        /// <summary>
        /// 构造一个 <see cref="Region"/> 实例。
        /// </summary>
        public Region()
            : base()
        {
            CreateTime = DateTime.Now;
        }


        /// <summary>
        /// 父主键。
        /// </summary>
        [DisplayName("父级地区")]
        public virtual int ParentId { get; set; }

        /// <summary>
        /// 英文简称。
        /// </summary>
        [DisplayName("英文简称")]
        public virtual string AbbrName { get; set; }

        /// <summary>
        /// 英文全称。
        /// </summary>
        [DisplayName("英文全称")]
        public virtual string Name { get; set; }

        /// <summary>
        /// 中文简称。
        /// </summary>
        [DisplayName("中文简称")]
        public virtual string AbbrTitle { get; set; }

        /// <summary>
        /// 中文全称。
        /// </summary>
        [DisplayName("中文全称")]
        public virtual string Title { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public virtual string Descr { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        [DisplayName("创建时间")]
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建用户。
        /// </summary>
        [DisplayName("创建用户")]
        public virtual int CreatorId { get; set; }
    }
}
