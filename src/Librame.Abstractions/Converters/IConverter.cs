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
    /// <summary>
    /// 转换器接口。
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// 转换为结果对象。
        /// </summary>
        /// <param name="source">给定的来源对象。</param>
        /// <returns>返回结果对象。</returns>
        object ToResult(object source);

        /// <summary>
        /// 转换为来源对象。
        /// </summary>
        /// <param name="result">给定的结果对象。</param>
        /// <returns>返回来源对象。</returns>
        object ToSource(object result);
    }


    /// <summary>
    /// 转换器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    public interface IConverter<TSource, TResult> : IConverter
    {
        /// <summary>
        /// 转换为结果实例。
        /// </summary>
        /// <param name="source">给定的来源实例。</param>
        /// <returns>返回结果实例。</returns>
        TResult ToResult(TSource source);

        /// <summary>
        /// 转换为来源实例。
        /// </summary>
        /// <param name="result">给定的结果实例。</param>
        /// <returns>返回来源实例。</returns>
        TSource ToSource(TResult result);
    }
}
