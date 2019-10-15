#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象可排序服务。
    /// </summary>
    public abstract class AbstractSortableService : AbstractService, ISortableService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractSortableService"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="rank">给定的服务优先级（数值越小越优先）。</param>
        protected AbstractSortableService(ILoggerFactory loggerFactory, float rank)
            : base(loggerFactory)
        {
            Priority = rank;
        }


        /// <summary>
        /// 服务优先级（数值越小越优先）。
        /// </summary>
        public float Priority { get; set; }
    }
}
