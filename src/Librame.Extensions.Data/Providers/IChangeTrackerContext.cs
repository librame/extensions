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
        IEnumerable<IChangeHandler> ChangeHandlers { get; }


        /// <summary>
        /// 处理变化。
        /// </summary>
        /// <param name="changeTracker">给定的 <see cref="ChangeTracker"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <param name="entityStates">给定要处理的实体状态集合（可选；默认对添加、修改、删除进行处理）。</param>
        void Process(ChangeTracker changeTracker, DataBuilderOptions builderOptions, params EntityState[] entityStates);


        /// <summary>
        /// 尝试获取指定的变化处理程序。
        /// </summary>
        /// <typeparam name="TChangeHandler">指定的变化处理程序类型。</typeparam>
        /// <param name="changeHandler">输出变化处理程序或 NULL。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        bool TryGetChangeHandler<TChangeHandler>(out TChangeHandler changeHandler)
            where TChangeHandler : class, IChangeHandler;
    }
}
