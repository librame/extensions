﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data.Mediators
{
    using Core.Mediators;

    /// <summary>
    /// 实体通知。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    public class EntityNotification<TEntity> : INotificationIndication
        where TEntity : class
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
