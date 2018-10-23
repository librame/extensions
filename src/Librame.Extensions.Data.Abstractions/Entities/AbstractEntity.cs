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
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象实体。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TDateTime">指定的日期与时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public abstract class AbstractEntity<TId, TDateTime, TStatus> : AbstractId<TId>, IEntity<TId, TDateTime, TStatus>
        where TId : IEquatable<TId>
        where TDateTime : struct
        where TStatus : struct
    {
        /// <summary>
        /// 数据排序。
        /// </summary>
        //[Display(Name = "DataRankName",
        //    ShortName = "DataRankShortName",
        //    Description = "DataRankDescription",
        //    Prompt = "DataRankPrompt",
        //    GroupName = "DataGroup",
        //    ResourceType = typeof(Resources.EntityStores)
        //)]
        public virtual int DataRank { get; set; }

        /// <summary>
        /// 数据状态。
        /// </summary>
        //[Display(Name = "DataStatusName",
        //    ShortName = "DataStatusShortName",
        //    Description = "DataStatusDescription",
        //    Prompt = "DataStatusPrompt",
        //    GroupName = "DataGroup",
        //    ResourceType = typeof(Resources.EntityStores)
        //)]
        public virtual TStatus DataStatus { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        //[Display(Name = "CreateTimeName",
        //    ShortName = "CreateTimeShortName",
        //    Description = "CreateTimeDescription",
        //    Prompt = "CreateTimePrompt",
        //    GroupName = "CreateGroup",
        //    ResourceType = typeof(Resources.EntityStores)
        //)]
        [DataType(DataType.DateTime)]
        public virtual TDateTime CreateTime { get; set; }

        /// <summary>
        /// 创建者身份。
        /// </summary>
        //[Display(Name = "CreatorIdName",
        //    ShortName = "CreatorIdShortName",
        //    Description = "CreatorIdDescription",
        //    Prompt = "CreatorIdPrompt",
        //    GroupName = "CreateGroup",
        //    ResourceType = typeof(Resources.EntityStores)
        //)]
        public virtual TId CreatorId { get; set; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        //[Display(Name = "CreateTimeName",
        //    ShortName = "CreateTimeShortName",
        //    Description = "CreateTimeDescription",
        //    Prompt = "CreateTimePrompt",
        //    GroupName = "UpdateGroup",
        //    ResourceType = typeof(Resources.EntityStores)
        //)]
        [DataType(DataType.DateTime)]
        public virtual TDateTime UpdateTime { get; set; }

        /// <summary>
        /// 更新者身份。
        /// </summary>
        //[Display(Name = "CreatorIdName",
        //    ShortName = "CreatorIdShortName",
        //    Description = "CreatorIdDescription",
        //    Prompt = "CreatorIdPrompt",
        //    GroupName = "UpdateGroup",
        //    ResourceType = typeof(Resources.EntityStores)
        //)]
        public virtual TId UpdatorId { get; set; }
    }
}
