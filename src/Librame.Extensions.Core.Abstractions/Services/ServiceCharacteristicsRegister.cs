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
using System.Collections.Generic;

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 服务特征注册器。
    /// </summary>
    public sealed class ServiceCharacteristicsRegister : IServiceCharacteristicsRegister
    {
        private IDictionary<Type, ServiceCharacteristics> _dictionary;


        /// <summary>
        /// 构造一个 <see cref="ServiceCharacteristicsRegister"/>。
        /// </summary>
        /// <param name="dictionary">给定的服务特征字典。</param>
        public ServiceCharacteristicsRegister(IDictionary<Type, ServiceCharacteristics> dictionary)
        {
            _dictionary = dictionary.NotNull(nameof(dictionary));
        }


        /// <summary>
        /// 添加或设置指定服务的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public ServiceCharacteristics AddOrSet(Type serviceType, ServiceCharacteristics characteristics)
        {
            return ExtensionSettings.Preference.RunLocker(() =>
            {
                if (_dictionary.ContainsKey(serviceType))
                    _dictionary[serviceType] = characteristics;
                else
                    _dictionary.Add(serviceType, characteristics);

                return characteristics;
            });
        }

        /// <summary>
        /// 获取指定服务的特征或默认特征（如果服务类型的特征不存在，则添加默认 <see cref="ServiceCharacteristics.Singleton(bool)"/>）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="addIfNone">如果注册器中不存在特征时，是否自行添加（可选；默认添加）。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public ServiceCharacteristics GetOrDefault(Type serviceType, bool addIfNone = true)
        {
            if (!TryGet(serviceType, out var result) && addIfNone)
            {
                // result 为默认单例
                ExtensionSettings.Preference.RunLocker(() => _dictionary.Add(serviceType, result));
            }

            return result;
        }

        /// <summary>
        /// 尝试添加指定服务的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool TryAdd(Type serviceType, ServiceCharacteristics characteristics)
        {
            return ExtensionSettings.Preference.RunLocker(() =>
            {
                if (!_dictionary.ContainsKey(serviceType))
                {
                    _dictionary.Add(serviceType, characteristics);
                    return true;
                }

                return false;
            });
        }

        /// <summary>
        /// 尝试获取指定服务的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="result">输出 <see cref="ServiceCharacteristics"/>（如果服务类型的特征不存在，则默认为 <see cref="ServiceCharacteristics.Singleton(bool)"/>）。</param>
        /// <returns>返回布尔值。</returns>
        public bool TryGet(Type serviceType, out ServiceCharacteristics result)
        {
            lock (ExtensionSettings.Preference.GetLocker())
            {
                foreach (var pair in _dictionary)
                {
                    if (pair.Key == serviceType)
                    {
                        result = pair.Value;
                        return true;
                    }
                }
            }
            
            // 默认单例
            result = ServiceCharacteristics.Singleton();
            return false;
        }

    }
}
