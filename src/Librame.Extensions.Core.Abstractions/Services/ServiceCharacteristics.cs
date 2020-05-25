#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 服务特征。
    /// </summary>
    public readonly struct ServiceCharacteristics : IEquatable<ServiceCharacteristics>
    {
        /// <summary>
        /// 构造一个 <see cref="ServiceCharacteristics"/>。
        /// </summary>
        /// <param name="lifetime">给定的 <see cref="ServiceLifetime"/>。</param>
        /// <param name="tryAdd">尝试添加（可选；默认 TRUE）。</param>
        public ServiceCharacteristics(ServiceLifetime lifetime,
            bool tryAdd = true)
        {
            Lifetime = lifetime;
            TryAdd = tryAdd;
        }


        /// <summary>
        /// 服务生命周期。
        /// </summary>
        public ServiceLifetime Lifetime { get; }

        /// <summary>
        /// 尝试添加。
        /// </summary>
        public bool TryAdd { get; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(ServiceCharacteristics other)
            => Lifetime == other.Lifetime
            && TryAdd == other.TryAdd;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is ServiceCharacteristics other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数值。</returns>
        public override int GetHashCode()
            => Lifetime.GetHashCode()
            ^ TryAdd.GetHashCode();


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="left">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <param name="right">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(ServiceCharacteristics left, ServiceCharacteristics right)
            => left.Equals(right);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="left">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <param name="right">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(ServiceCharacteristics left, ServiceCharacteristics right)
            => !(left == right);


        /// <summary>
        /// 单例特征。
        /// </summary>
        /// <param name="tryAdd">尝试添加（可选；默认 TRUE）。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public static ServiceCharacteristics Singleton(bool tryAdd = true)
            => new ServiceCharacteristics(ServiceLifetime.Singleton, tryAdd);

        /// <summary>
        /// 域例特征。
        /// </summary>
        /// <param name="tryAdd">尝试添加（可选；默认 TRUE）。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public static ServiceCharacteristics Scoped(bool tryAdd = true)
            => new ServiceCharacteristics(ServiceLifetime.Scoped, tryAdd);

        /// <summary>
        /// 瞬例特征。
        /// </summary>
        /// <param name="tryAdd">尝试添加（可选；默认 TRUE）。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public static ServiceCharacteristics Transient(bool tryAdd = true)
            => new ServiceCharacteristics(ServiceLifetime.Transient, tryAdd);
    }
}
