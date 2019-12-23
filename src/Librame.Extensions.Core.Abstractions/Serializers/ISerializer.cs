#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;

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
        /// <returns>返回来源。</returns>
        TSource Deserialize(TTarget target);

        /// <summary>
        /// 将来源序列化为目标。
        /// </summary>
        /// <returns>返回目标。</returns>
        TTarget Serialize(TSource source);
    }


    /// <summary>
    /// 序列化器接口。
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    public interface ISerializer
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }
    }
}
