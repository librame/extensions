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
using System.Collections.Generic;

namespace Librame.Extensions.Core.Converters
{
    using Transformers;

    /// <summary>
    /// 转换器管理器。
    /// </summary>
    public class ConverterManager : AbstractTransformerManager<IConverter>
    {
        private ConverterManager()
        {
        }


        private static readonly Type _baseAlgorithmConverterType
            = typeof(IAlgorithmConverter);


        /// <summary>
        /// 默认实例。
        /// </summary>
        public static readonly ConverterManager Default
            = new ConverterManager();


        #region GetBySourceAndTarget

        /// <summary>
        /// 通过来源与目标类型获取转换器。
        /// </summary>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <typeparam name="TTarget">指定的目标类型。</typeparam>
        /// <param name="singleFactory">如果操作出错是否抛出异常（可选；默认返回集合中第一个转换器）。</param>
        /// <returns>返回 <see cref="IConverter{TSource, TTarget}"/>。</returns>
        public static IConverter<TSource, TTarget> GetBySourceAndTarget<TSource, TTarget>
            (Func<IEnumerable<IConverter>, IConverter> singleFactory = null)
            => (IConverter<TSource, TTarget>)Default.GetBySourceAndTarget(typeof(TSource),
                typeof(TTarget), singleFactory);

        #endregion


        #region Get

        /// <summary>
        /// 获取算法转换器。
        /// </summary>
        /// <typeparam name="TConverter">指定的算法转换器类型。</typeparam>
        /// <returns>返回 <see cref="IAlgorithmConverter"/>。</returns>
        public static TConverter GetAlgorithm<TConverter>()
            where TConverter : IAlgorithmConverter
            => (TConverter)Default.Get(typeof(TConverter));

        /// <summary>
        /// 获取指定类型的算法转换器。
        /// </summary>
        /// <param name="algorithmConverterType">给定的算法转换器类型。</param>
        /// <returns>返回 <see cref="IAlgorithmConverter"/>。</returns>
        public static IAlgorithmConverter GetAlgorithm(Type algorithmConverterType)
        {
            _baseAlgorithmConverterType.AssignableFromTarget(algorithmConverterType);
            return (IAlgorithmConverter)Default.Get(algorithmConverterType);
        }


        /// <summary>
        /// 获取指定类型的转换器。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <returns>返回 <typeparamref name="TConverter"/>。</returns>
        public static TConverter Get<TConverter>()
            where TConverter : IConverter
            => (TConverter)Default.Get(typeof(TConverter));

        #endregion

    }
}
