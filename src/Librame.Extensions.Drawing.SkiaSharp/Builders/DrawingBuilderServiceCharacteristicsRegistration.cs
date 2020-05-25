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

namespace Librame.Extensions.Drawing.Builders
{
    using Core.Services;
    using Drawing.Services;

    /// <summary>
    /// <see cref="IDrawingBuilder"/> 服务特征注册。
    /// </summary>
    public static class DrawingBuilderServiceCharacteristicsRegistration
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
                // Services
                { typeof(ICaptchaService), ServiceCharacteristics.Singleton() },
                { typeof(IScaleService), ServiceCharacteristics.Singleton() },
                { typeof(IWatermarkService), ServiceCharacteristics.Singleton() }
            };
        }

    }
}
