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

namespace Librame.Extensions.Core.Serializers
{
    using Transformers;

    /// <summary>
    /// 抽象序列化器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    public abstract class AbstractSerializer<TSource, TTarget>
        : AbstractTransformer<TSource, TTarget>, ISerializer<TSource, TTarget>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractTransformer{TSource, TTarget}"/>。
        /// </summary>
        /// <param name="forward">给定的正向序列化工厂方法。</param>
        /// <param name="reverse">给定的反向序列化工厂方法。</param>
        protected AbstractSerializer(Func<TSource, TTarget> forward,
            Func<TTarget, TSource> reverse)
            : base(forward, reverse)
        {
        }


        /// <summary>
        /// 将目标反序列化为来源。
        /// </summary>
        /// <param name="target">给定的 <typeparamref name="TTarget"/>。</param>
        /// <returns>返回 <typeparamref name="TSource"/>。</returns>
        public virtual TSource Deserialize(TTarget target)
            => Reverse.Invoke(target);

        /// <summary>
        /// 将来源序列化为目标。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        /// <returns>返回 <typeparamref name="TTarget"/>。</returns>
        public virtual TTarget Serialize(TSource source)
            => Forward.Invoke(source);
    }
}
