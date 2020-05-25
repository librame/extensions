#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Converters
{
    using Transformers;

    /// <summary>
    /// 转换器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    public interface IConverter<TSource, TTarget> : IConverter
    {
        /// <summary>
        /// 来源转自目标。
        /// </summary>
        /// <param name="target">给定的 <typeparamref name="TTarget"/>。</param>
        /// <returns>返回 <typeparamref name="TSource"/>。</returns>
        TSource ConvertFrom(TTarget target);

        /// <summary>
        /// 来源转为目标。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        /// <returns>返回 <typeparamref name="TTarget"/>。</returns>
        TTarget ConvertTo(TSource source);
    }


    /// <summary>
    /// 表示可自动注册的转换器接口。如果不需要自动注册，请在实现类型中标记 <see cref="NonRegisteredAttribute"/>。
    /// </summary>
    public interface IConverter : ITransformer
    {
    }
}
