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
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Core.Serializers
{
    using Utilities;

    /// <summary>
    /// 序列化器助手。
    /// </summary>
    public static class SerializerHelper
    {
        private static readonly IReadOnlyList<ISerializer> _serializers
            = AssemblyUtility.CreateInstancesByCurrentExportedTypesWithoutSystem<ISerializer>();

        private static Func<ISerializer, bool> GetPredicate(Type sourceType, Type targetType)
            => s => s.SourceType == sourceType && s.TargetType == targetType;


        /// <summary>
        /// 获取指定来源类型的字符串序列化器。
        /// </summary>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <returns>返回 <see cref="IStringSerializer{TSource}"/>。</returns>
        public static IStringSerializer<TSource> GetStringSerializer<TSource>(bool throwIfError = true)
        {
            ISerializer serializer;

            if (throwIfError)
                serializer = _serializers.First(GetPredicate(typeof(TSource), typeof(string)));
            else
                serializer = _serializers.FirstOrDefault(GetPredicate(typeof(TSource), typeof(string)));

            return (IStringSerializer<TSource>)serializer;
        }


        /// <summary>
        /// 获取指定来源与目标类型的序列化器。
        /// </summary>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <typeparam name="TTarget">指定的目标类型。</typeparam>
        /// <param name="throwIfError">如果操作出错是否抛出异常（可选；默认启用）。</param>
        /// <returns>返回 <see cref="ISerializer{TSource, TTarget}"/>。</returns>
        public static ISerializer<TSource, TTarget> GetSerializer<TSource, TTarget>(bool throwIfError = true)
        {
            ISerializer serializer;

            if (throwIfError)
                serializer = _serializers.First(GetPredicate(typeof(TSource), typeof(string)));
            else
                serializer = _serializers.FirstOrDefault(GetPredicate(typeof(TSource), typeof(string)));

            return (ISerializer<TSource, TTarget>)serializer;
        }

        /// <summary>
        /// 获取指定来源与目标类型的序列化器。
        /// </summary>
        /// <param name="sourceType">给定的来源类型。</param>
        /// <param name="targetType">给定的目标类型。</param>
        /// <param name="throwIfError">如果操作出错是否抛出异常（可选；默认启用）。</param>
        /// <returns>返回 <see cref="ISerializer"/>。</returns>
        public static ISerializer GetSerializer(Type sourceType, Type targetType, bool throwIfError = true)
        {
            if (throwIfError)
                return _serializers.First(GetPredicate(sourceType, targetType));

            return _serializers.FirstOrDefault(GetPredicate(sourceType, targetType));
        }

    }
}
