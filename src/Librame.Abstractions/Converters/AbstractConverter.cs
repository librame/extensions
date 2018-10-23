#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Converters
{
    using Extensions;

    /// <summary>
    /// 抽象转换器。
    /// </summary>
    public abstract class AbstractConverter : IConverter
    {
        /// <summary>
        /// 转换为结果对象。
        /// </summary>
        /// <param name="source">给定的来源对象。</param>
        /// <returns>返回结果对象。</returns>
        public abstract object ToResult(object source);

        /// <summary>
        /// 转换为来源对象。
        /// </summary>
        /// <param name="result">给定的结果对象。</param>
        /// <returns>返回来源对象。</returns>
        public abstract object ToSource(object result);
    }


    /// <summary>
    /// 抽象转换器。
    /// </summary>
    /// <typeparam name="TSource">指定的转换类型。</typeparam>
    /// <typeparam name="TResult">指定的还原类型。</typeparam>
    public abstract class AbstractConverter<TSource, TResult> : AbstractConverter, IConverter<TSource, TResult>
    {
        /// <summary>
        /// 转换为结果对象。
        /// </summary>
        /// <param name="source">给定的来源对象。</param>
        /// <returns>返回结果对象。</returns>
        public override object ToResult(object source)
        {
            var instance = source.SameType<TSource>(nameof(source));

            return ToResult(instance);
        }

        /// <summary>
        /// 转换为来源对象。
        /// </summary>
        /// <param name="result">给定的结果对象。</param>
        /// <returns>返回来源对象。</returns>
        public override object ToSource(object result)
        {
            var instance = result.SameType<TResult>(nameof(result));

            return ToSource(instance);
        }


        /// <summary>
        /// 转换为结果实例。
        /// </summary>
        /// <param name="source">给定的来源实例。</param>
        /// <returns>返回结果实例。</returns>
        public abstract TResult ToResult(TSource source);

        /// <summary>
        /// 转换为来源实例。
        /// </summary>
        /// <param name="result">给定的结果实例。</param>
        /// <returns>返回来源实例。</returns>
        public abstract TSource ToSource(TResult result);

    }
}
