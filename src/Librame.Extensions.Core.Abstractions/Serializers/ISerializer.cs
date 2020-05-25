#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Serializers
{
    using Transformers;

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
    /// 表示可自动注册的序列化器接口。如果不需要自动注册，请在实现类型中标记 <see cref="NonRegisteredAttribute"/>。
    /// </summary>
    public interface ISerializer : ITransformer
    {
    }
}
