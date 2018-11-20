#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 变化跟踪器上下文接口。
    /// </summary>
    public interface IChangeTrackerContext
    {
        /// <summary>
        /// 实体变化处理程序集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IEnumerable{IEntityChangeHandler}"/>。
        /// </value>
        IEnumerable<IEntityChangeHandler> ChangeHandlers { get; }


        /// <summary>
        /// 处理变化。
        /// </summary>
        /// <param name="changeTracker">给定的 <see cref="ChangeTracker"/>。</param>
        /// <param name="entityStates">给定要处理的实体状态集合（可选；默认对添加、修改、删除进行处理）。</param>
        void Process(ChangeTracker changeTracker, params EntityState[] entityStates);
    }
}
