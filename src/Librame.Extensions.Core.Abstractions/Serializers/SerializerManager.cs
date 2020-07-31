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

namespace Librame.Extensions.Core.Serializers
{
    using Singletons;
    using Transformers;

    /// <summary>
    /// 序列化器管理器。
    /// </summary>
    public class SerializerManager : AbstractTransformerManager<ISerializer>
    {
        private SerializerManager()
            : base()
        {
        }


        /// <summary>
        /// 默认实例。
        /// </summary>
        public static SerializerManager Default
            => SingletonFactory<SerializerManager>.Instance;


        #region GetBySourceAndTarget

        /// <summary>
        /// 通过来源类型获取字符串序列化器。
        /// </summary>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <returns>返回 <see cref="IStringSerializer{TSource}"/>。</returns>
        public static IStringSerializer<TSource> GetBySource<TSource>
            (Func<IEnumerable<ISerializer>, ISerializer> singleFactory = null)
            => (IStringSerializer<TSource>)Default.GetBySourceAndTarget(typeof(TSource),
                typeof(string), singleFactory);


        /// <summary>
        /// 通过来源与目标类型获取序列化器。
        /// </summary>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <typeparam name="TTarget">指定的目标类型。</typeparam>
        /// <param name="singleFactory">如果操作出错是否抛出异常（可选；默认返回集合中第一个序列化器）。</param>
        /// <returns>返回 <see cref="ISerializer{TSource, TTarget}"/>。</returns>
        public static ISerializer<TSource, TTarget> GetBySourceAndTarget<TSource, TTarget>
            (Func<IEnumerable<ISerializer>, ISerializer> singleFactory = null)
            => (ISerializer<TSource, TTarget>)Default.GetBySourceAndTarget(typeof(TSource),
                typeof(TTarget), singleFactory);

        #endregion


        #region Get

        /// <summary>
        /// 获取指定类型的字符串序列化器。
        /// </summary>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="stringSerializerType">给定的字符串序列化器类型。</param>
        /// <returns>返回 <see cref="IStringSerializer{TSource}"/>。</returns>
        public static IStringSerializer<TSource> GetString<TSource>(Type stringSerializerType)
        {
            typeof(IStringSerializer<TSource>).AssignableFromTarget(stringSerializerType);
            return (IStringSerializer<TSource>)Default.Get(stringSerializerType);
        }


        /// <summary>
        /// 获取指定类型的序列化器。
        /// </summary>
        /// <typeparam name="TSerializer">指定的序列化器类型。</typeparam>
        /// <returns>返回 <typeparamref name="TSerializer"/>。</returns>
        public static TSerializer Get<TSerializer>()
            where TSerializer : ISerializer
            => (TSerializer)Default.Get(typeof(TSerializer));

        #endregion

    }
}
