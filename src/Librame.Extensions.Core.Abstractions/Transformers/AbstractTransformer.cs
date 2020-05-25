#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Transformers
{
    /// <summary>
    /// 抽象变换器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    public abstract class AbstractTransformer<TSource, TTarget> : ITransformer
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractTransformer{TSource, TTarget}"/>。
        /// </summary>
        /// <param name="forward">给定的正向变换工厂方法。</param>
        /// <param name="reverse">给定的反向变换工厂方法。</param>
        protected AbstractTransformer(Func<TSource, TTarget> forward,
            Func<TTarget, TSource> reverse)
        {
            Forward = forward.NotNull(nameof(forward));
            Reverse = reverse.NotNull(nameof(reverse));
        }


        /// <summary>
        /// 正向变换。
        /// </summary>
        protected Func<TSource, TTarget> Forward { get; }

        /// <summary>
        /// 反向变换。
        /// </summary>
        protected Func<TTarget, TSource> Reverse { get; }


        /// <summary>
        /// 来源类型。
        /// </summary>
        public Type SourceType
            => typeof(TSource);

        /// <summary>
        /// 目标类型。
        /// </summary>
        public Type TargetType
            => typeof(TTarget);
    }
}
