#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// <see cref="IServiceCharacteristicsRegister"/> 静态扩展。
    /// </summary>
    public static class ServiceCharacteristicsRegisterExtensions
    {
        /// <summary>
        /// 添加或设置指定服务的特征。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="register">给定的 <see cref="IServiceCharacteristicsRegister"/>。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public static ServiceCharacteristics AddOrSet<TService>(this IServiceCharacteristicsRegister register,
            ServiceCharacteristics characteristics)
            => register.NotNull(nameof(register)).AddOrSet(typeof(TService), characteristics);

        /// <summary>
        /// 获取指定服务的特征或默认特征（如果服务类型的特征不存在，则添加默认 <see cref="ServiceCharacteristics.Singleton(bool)"/>）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="register">给定的 <see cref="IServiceCharacteristicsRegister"/>。</param>
        /// <param name="addIfNone">如果注册器中不存在特征时，是否自行添加（可选；默认添加）。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public static ServiceCharacteristics GetOrDefault<TService>(this IServiceCharacteristicsRegister register,
            bool addIfNone = true)
            => register.NotNull(nameof(register)).GetOrDefault(typeof(TService), addIfNone);

        /// <summary>
        /// 尝试添加指定服务的特征。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="register">给定的 <see cref="IServiceCharacteristicsRegister"/>。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryAdd<TService>(this IServiceCharacteristicsRegister register,
            ServiceCharacteristics characteristics)
            => register.NotNull(nameof(register)).TryAdd(typeof(TService), characteristics);

        /// <summary>
        /// 尝试获取指定服务的特征。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="register">给定的 <see cref="IServiceCharacteristicsRegister"/>。</param>
        /// <param name="result">输出 <see cref="ServiceCharacteristics"/>（如果服务类型的特征不存在，则默认为 <see cref="ServiceCharacteristics.Singleton(bool)"/>）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGet<TService>(this IServiceCharacteristicsRegister register,
            out ServiceCharacteristics result)
            => register.NotNull(nameof(register)).TryGet(typeof(TService), out result);

    }
}
