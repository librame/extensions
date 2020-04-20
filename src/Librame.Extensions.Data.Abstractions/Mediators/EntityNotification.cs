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
using System.Collections.Generic;

namespace Librame.Extensions.Data.Mediators
{
    using Core.Mediators;
    using Data.Stores;

    /// <summary>
    /// 实体通知。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public class EntityNotification<TEntity, TGenId> : INotification
        where TEntity : DataEntity<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 添加实体集合。
        /// </summary>
        public IReadOnlyList<TEntity> Adds { get; set; }

        /// <summary>
        /// 更新实体集合。
        /// </summary>
        public IReadOnlyList<TEntity> Updates { get; set; }

        /// <summary>
        /// 删除实体集合。
        /// </summary>
        public IReadOnlyList<TEntity> Removes { get; set; }
    }
}
