#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Core;
    using Core.Builders;
    using Core.Services;
    using Data.Accessors;
    using Data.Stores;

    /// <summary>
    /// 抽象访问器截面。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractAccessorAspect<TBuilderOptions>
        : AbstractExtensionBuilderService<TBuilderOptions>, IAccessorAspect
        where TBuilderOptions : class, IExtensionBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{TBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="priority">给定的优先级（数值越小越优先）。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        protected AbstractAccessorAspect(IStoreIdentifierGenerator identifierGenerator,
            IOptions<TBuilderOptions> options, ILoggerFactory loggerFactory, float priority)
            : base(options, loggerFactory)
        {
            IdentifierGenerator = identifierGenerator.NotNull(nameof(identifierGenerator));
            Priority = priority;

            Clock = identifierGenerator.Clock;
        }


        /// <summary>
        /// 存储标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IStoreIdentifierGenerator"/>。</value>
        public IStoreIdentifierGenerator IdentifierGenerator { get; }

        /// <summary>
        /// 时钟。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock { get; }

        /// <summary>
        /// 服务优先级（数值越小越优先）。
        /// </summary>
        public float Priority { get; set; }

        /// <summary>
        /// 启用截面。
        /// </summary>
        public virtual bool Enabled { get; }


        /// <summary>
        /// 前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public abstract void PreProcess(IAccessor accessor);

        /// <summary>
        /// 异步前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public abstract Task PreProcessAsync(IAccessor accessor, CancellationToken cancellationToken = default);


        /// <summary>
        /// 后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public abstract void PostProcess(IAccessor accessor);

        /// <summary>
        /// 异步后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public abstract Task PostProcessAsync(IAccessor accessor, CancellationToken cancellationToken = default);


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
            => obj is AbstractAccessorAspect<TBuilderOptions> sortable
            && Priority == sortable?.Priority;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => Priority.GetHashCode();


        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AbstractAccessorAspect<TBuilderOptions> left,
            AbstractAccessorAspect<TBuilderOptions> right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.Equals(right);

        /// <summary>
        /// 不等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AbstractAccessorAspect<TBuilderOptions> left,
            AbstractAccessorAspect<TBuilderOptions> right)
            => !(left == right);

        /// <summary>
        /// 小于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <(AbstractAccessorAspect<TBuilderOptions> left,
            AbstractAccessorAspect<TBuilderOptions> right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        /// <summary>
        /// 小于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/></param>
        /// <param name="right">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <=(AbstractAccessorAspect<TBuilderOptions> left,
            AbstractAccessorAspect<TBuilderOptions> right)
            => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        /// <summary>
        /// 大于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >(AbstractAccessorAspect<TBuilderOptions> left,
            AbstractAccessorAspect<TBuilderOptions> right)
            => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        /// <summary>
        /// 大于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <param name="right">给定的 <see cref="AbstractAccessorAspect{TBuilderOptions}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >=(AbstractAccessorAspect<TBuilderOptions> left,
            AbstractAccessorAspect<TBuilderOptions> right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}
