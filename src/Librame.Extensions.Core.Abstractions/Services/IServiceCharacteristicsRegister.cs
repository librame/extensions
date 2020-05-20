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

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 服务特征注册器接口。
    /// </summary>
    public interface IServiceCharacteristicsRegister
    {
        /// <summary>
        /// 添加或设置指定服务的特征。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        ServiceCharacteristics AddOrSet<TService>(ServiceCharacteristics characteristics);

        /// <summary>
        /// 添加或设置指定服务的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        ServiceCharacteristics AddOrSet(Type serviceType, ServiceCharacteristics characteristics);


        /// <summary>
        /// 获取指定服务的特征或默认特征（如果服务类型的特征不存在，则默认）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        ServiceCharacteristics GetOrDefault<TService>();

        /// <summary>
        /// 获取指定服务的特征或默认特征（如果服务类型的特征不存在，则默认）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        ServiceCharacteristics GetOrDefault(Type serviceType);


        /// <summary>
        /// 尝试添加指定服务的特征。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        bool TryAdd<TService>(ServiceCharacteristics characteristics);

        /// <summary>
        /// 尝试添加指定服务的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        bool TryAdd(Type serviceType, ServiceCharacteristics characteristics);


        /// <summary>
        /// 尝试获取指定服务的特征。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="result">输出 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        bool TryGet<TService>(out ServiceCharacteristics result);

        /// <summary>
        /// 尝试获取指定服务的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="result">输出 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        bool TryGet(Type serviceType, out ServiceCharacteristics result);
    }
}
