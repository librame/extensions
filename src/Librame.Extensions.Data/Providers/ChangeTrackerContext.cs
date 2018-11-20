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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data
{
    using Services;

    /// <summary>
    /// 变化跟踪器上下文。
    /// </summary>
    public class ChangeTrackerContext : AbstractService<ChangeTrackerContext>, IChangeTrackerContext
    {
        /// <summary>
        /// 构造一个 <see cref="ChangeTrackerContext"/> 实例。
        /// </summary>
        /// <param name="changeHandlers">给定的 <see cref="IEnumerable{IEntityChangeHandler}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{ChangeTrackerHandler}"/>。</param>
        public ChangeTrackerContext(IEnumerable<IEntityChangeHandler> changeHandlers, ILogger<ChangeTrackerContext> logger)
            : base(logger)
        {
            ChangeHandlers = changeHandlers.NotEmpty(nameof(changeHandlers));
        }


        /// <summary>
        /// 实体变化处理程序集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IEnumerable{IEntityChangeHandler}"/>。
        /// </value>
        public IEnumerable<IEntityChangeHandler> ChangeHandlers { get; }


        /// <summary>
        /// 处理变化。
        /// </summary>
        /// <param name="changeTracker">给定的 <see cref="ChangeTracker"/>。</param>
        /// <param name="entityStates">给定要处理的实体状态集合（可选；默认对添加、修改、删除进行处理）。</param>
        public void Process(ChangeTracker changeTracker, params EntityState[] entityStates)
        {
            changeTracker.NotDefault(nameof(changeTracker));

            if (entityStates.IsEmpty())
            {
                // 默认仅审计实体的增删改操作
                entityStates = new EntityState[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };
            }

            try
            {
                var entries = changeTracker.Entries().Where(m => m.Entity.IsNotDefault() && entityStates.Contains(m.State));

                foreach (var entry in entries)
                {
                    foreach (var handler in ChangeHandlers)
                        handler.Process(entry);
                }
            }
            catch (Exception ex)
            {
                Logger.LogDebug(ex.AsInnerMessage());
            }
        }

    }

}
