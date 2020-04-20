#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象可排序。
    /// </summary>
    public class AbstractSortable : ISortable
    {
        /// <summary>
        /// 默认优先级。
        /// </summary>
        public const float DefaultPriority = 9;


        /// <summary>
        /// 构造一个 <see cref="AbstractSortable"/>。
        /// </summary>
        /// <param name="priority">给定的优先级（可选；默认为 <see cref="DefaultPriority"/>）。</param>
        protected AbstractSortable(float? priority = null)
        {
            Priority = priority ?? DefaultPriority;
        }


        /// <summary>
        /// 优先级（数值越小越优先）。
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
            => obj is ISortable sortable && Priority == sortable?.Priority;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => Priority.GetHashCode();


        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortable"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AbstractSortable left, AbstractSortable right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.Equals(right);

        /// <summary>
        /// 不等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortable"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AbstractSortable left, AbstractSortable right)
            => !(left == right);

        /// <summary>
        /// 小于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortable"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <(AbstractSortable left, AbstractSortable right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        /// <summary>
        /// 小于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortable"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <=(AbstractSortable left, AbstractSortable right)
            => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        /// <summary>
        /// 大于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortable"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >(AbstractSortable left, AbstractSortable right)
            => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        /// <summary>
        /// 大于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractSortable"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractSortable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >=(AbstractSortable left, AbstractSortable right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}
