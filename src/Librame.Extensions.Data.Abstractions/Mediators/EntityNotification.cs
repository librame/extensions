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

namespace Librame.Extensions.Data
{
    using Core;

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
        /// 实体集合。
        /// </summary>
        public List<TEntity> Entities { get; set; }
    }
}
