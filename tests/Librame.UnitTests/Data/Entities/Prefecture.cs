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
using System.ComponentModel.DataAnnotations;

namespace Librame.UnitTests.Data.Entities
{
    /// <summary>
    /// 行政区划。
    /// </summary>
    [Description("行政区划")]
    public class Prefecture : AbstractDataIdDescriptor<int>, IParentIdDescriptor<int>, ICreateIdDescriptor<int>, IEntityAutomapping
    {
        /// <summary>
        /// 构造一个 <see cref="Prefecture"/> 实例。
        /// </summary>
        public Prefecture()
            : base()
        {
            CreateTime = DateTime.Now;
        }


        /// <summary>
        /// 父主键。
        /// </summary>
        [DisplayName("父级区划")]
        public virtual int ParentId { get; set; }

        /// <summary>
        /// 国家或地区主键。
        /// </summary>
        [DisplayName("国家或地区")]
        public virtual int RegionId { get; set; }

        /// <summary>
        /// 代码标识。
        /// </summary>
        [DisplayName("区划代码")]
        public virtual string CodeId { get; set; }

        /// <summary>
        /// 初始首字母。
        /// </summary>
        [DisplayName("初始首字母")]
        public virtual string Initial { get; set; }

        /// <summary>
        /// 首字母缩写。
        /// </summary>
        [DisplayName("首字母缩写")]
        public virtual string Acronym { get; set; }

        /// <summary>
        /// 中文拼音。
        /// </summary>
        [DisplayName("中文拼音")]
        public virtual string Name { get; set; }

        /// <summary>
        /// 中文名称。
        /// </summary>
        [DisplayName("中文名称")]
        public virtual string Title { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [DisplayName("备注说明")]
        [DataType(DataType.MultilineText)]
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
