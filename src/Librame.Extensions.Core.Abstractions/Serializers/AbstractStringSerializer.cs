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

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 抽象字符串序列化器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public abstract class AbstractStringSerializer<TSource>
        : AbstractSerializer<TSource, string>, IStringSerializer<TSource>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStringSerializer{TSource}"/>。
        /// </summary>
        /// <param name="forward">给定的正向序列化工厂方法。</param>
        /// <param name="reverse">给定的反向序列化工厂方法。</param>
        protected AbstractStringSerializer(Func<TSource, string> forward,
            Func<string, TSource> reverse)
            : base(forward, reverse)
        {
        }

    }
}
