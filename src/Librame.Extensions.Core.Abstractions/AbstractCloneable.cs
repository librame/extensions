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
using System.Collections.Concurrent;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象可克隆对象。
    /// </summary>
    /// <typeparam name="TCloneable">指定的可克隆对象类型。</typeparam>
    public abstract class AbstractCloneable<TCloneable> : ICloneable<TCloneable>
    {
        private ConcurrentDictionary<Type, object> _clonedTypes = null;


        /// <summary>
        /// 创建一个副本。
        /// </summary>
        /// <returns>返回 <typeparamref name="TCloneable"/>。</returns>
        public virtual object Clone()
            => MemberwiseClone();

        /// <summary>
        /// 复制副本（相当于 <see cref="ICloneable.Clone()"/> 的泛型版本；默认返回浅副本）。
        /// </summary>
        /// <returns>返回 <typeparamref name="TCloneable"/>。</returns>
        public virtual TCloneable Copy()
            => ShallowClone();

        /// <summary>
        /// 创建一个浅副本（在副本中对引用类型的字段值做修改会影响到源对象本身）。
        /// </summary>
        /// <returns>返回 <typeparamref name="TCloneable"/>。</returns>
        public virtual TCloneable ShallowClone()
            => (TCloneable)Clone();

        /// <summary>
        /// 创建一个深副本。
        /// </summary>
        /// <returns>返回 <typeparamref name="TCloneable"/>。</returns>
        public virtual TCloneable DeepClone()
        {
            var clone = this.EnsureClone(typeof(TCloneable), _clonedTypes);
            _clonedTypes.Clear();

            return (TCloneable)clone;
        }

    }
}
