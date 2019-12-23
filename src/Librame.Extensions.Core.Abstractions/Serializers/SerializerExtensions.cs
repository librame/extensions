#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 序列化器静态扩展。
    /// </summary>
    public static class SerializerExtensions
    {
        /// <summary>
        /// 反序列化为来源实例。
        /// </summary>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="serializer">给定的 <see cref="IObjectStringSerializer"/>。</param>
        /// <param name="target">给定的目标字符串。</param>
        /// <returns>返回 <typeparamref name="TSource"/>。</returns>
        public static TSource Deserialize<TSource>(this IObjectStringSerializer serializer, string target)
            => (TSource)serializer?.Deserialize(target);
    }
}
