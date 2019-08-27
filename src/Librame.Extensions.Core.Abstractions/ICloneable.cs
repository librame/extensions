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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 可克隆对象接口。
    /// </summary>
    /// <typeparam name="TCloneable">指定的可克隆对象类型。</typeparam>
    public interface ICloneable<TCloneable> : ICloneable
    {
        /// <summary>
        /// 复制副本（相当于 <see cref="ICloneable.Clone()"/> 的泛型版本）。
        /// </summary>
        /// <returns>返回 <typeparamref name="TCloneable"/>。</returns>
        TCloneable Copy();

        /// <summary>
        /// 创建一个浅副本。
        /// </summary>
        /// <returns>返回 <typeparamref name="TCloneable"/>。</returns>
        TCloneable ShallowClone();

        /// <summary>
        /// 创建一个深副本。
        /// </summary>
        /// <returns>返回 <typeparamref name="TCloneable"/>。</returns>
        TCloneable DeepClone();
    }
}
