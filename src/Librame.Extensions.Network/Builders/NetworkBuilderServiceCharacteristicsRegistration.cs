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

namespace Librame.Extensions.Network.Builders
{
    using Core.Services;
    using Network.Requesters;
    using Network.Services;

    /// <summary>
    /// <see cref="INetworkBuilder"/> 服务特征注册。
    /// </summary>
    public static class NetworkBuilderServiceCharacteristicsRegistration
    {
        private static IServiceCharacteristicsRegister _register;

        /// <summary>
        /// 当前注册器。
        /// </summary>
        public static IServiceCharacteristicsRegister Register
        {
            get => _register.EnsureSingleton(() => new ServiceCharacteristicsRegister(InitializeCharacteristics()));
            set => _register = value.NotNull(nameof(value));
        }


        private static IDictionary<Type, ServiceCharacteristics> InitializeCharacteristics()
        {
            return new Dictionary<Type, ServiceCharacteristics>
            {
                // Requesters
                { typeof(IUriRequester), ServiceCharacteristics.Singleton() },

                // Services
                { typeof(IByteCodecService), ServiceCharacteristics.Singleton() },
                { typeof(ICrawlerService), ServiceCharacteristics.Singleton() },
                { typeof(IEmailService), ServiceCharacteristics.Singleton() },
                { typeof(ISmsService), ServiceCharacteristics.Singleton() }
            };
        }

    }
}
