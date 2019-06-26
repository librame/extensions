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
    /// 抽象验证器。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public abstract class AbstractValidator<TSource> : IValidator<TSource>
    {
        /// <summary>
        /// 构建一个 <see cref="AbstractValidator{TSource}"/> 实例。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        public AbstractValidator(TSource source)
        {
            RawSource = source;
        }


        /// <summary>
        /// 原始源实例。
        /// </summary>
        public TSource RawSource { get; }
    }
}
