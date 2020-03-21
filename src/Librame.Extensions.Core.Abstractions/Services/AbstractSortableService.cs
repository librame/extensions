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

namespace Librame.Extensions.Core.Services
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
        /// <param name="priority">给定的服务优先级（数值越小越优先）。</param>
        protected AbstractSortableService(ILoggerFactory loggerFactory, float priority)
            : base(loggerFactory)
        {
            Priority = priority;
        }


        /// <summary>
        /// 服务优先级（数值越小越优先）。
        /// </summary>
        public float Priority { get; set; }


        /// <summary>
        /// 比较优先级。
        /// </summary>
        /// <param name="other">给定的 <see cref="ISortable"/>。</param>
        /// <returns>返回整数。</returns>
        public virtual int CompareTo(ISortable other)
            => Priority.CompareTo((float)other?.Priority);


        /// <summary>
        /// 优先级相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is ISortableService sortable ? Priority == sortable?.Priority : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => Priority.GetHashCode();


        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AbstractSortableService left, AbstractSortableService right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.Equals(right);

        /// <summary>
        /// 不等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AbstractSortableService left, AbstractSortableService right)
            => !(left == right);

        /// <summary>
        /// 小于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <(AbstractSortableService left, AbstractSortableService right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        /// <summary>
        /// 小于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <=(AbstractSortableService left, AbstractSortableService right)
            => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        /// <summary>
        /// 大于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >(AbstractSortableService left, AbstractSortableService right)
            => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        /// <summary>
        /// 大于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortableService"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >=(AbstractSortableService left, AbstractSortableService right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}
