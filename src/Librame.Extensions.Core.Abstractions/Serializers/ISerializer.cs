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
    /// <summary>
    /// 序列化器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    public interface ISerializer<TSource, TTarget> : ISerializer
    {
        /// <summary>
        /// 将目标反序列化为来源。
        /// </summary>
        /// <param name="target">给定的 <typeparamref name="TTarget"/>。</param>
        /// <returns>返回 <typeparamref name="TSource"/>。</returns>
        TSource Deserialize(TTarget target);

        /// <summary>
        /// 将来源序列化为目标。
        /// </summary>
        /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
        /// <returns>返回 <typeparamref name="TTarget"/>。</returns>
        TTarget Serialize(TSource source);
    }


    /// <summary>
    /// 标记序列化器接口。
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 来源类型。
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// 目标类型。
        /// </summary>
        Type TargetType { get; }
    }
}
