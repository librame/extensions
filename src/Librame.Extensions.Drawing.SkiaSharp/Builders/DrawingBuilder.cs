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

namespace Librame.Extensions.Drawing.Builders
{
    using Core.Builders;
    using Core.Services;
    using Drawing.Services;

    /// <summary>
    /// 图画构建器。
    /// </summary>
    public class DrawingBuilder : AbstractExtensionBuilder, IDrawingBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="DrawingBuilder"/>。
        /// </summary>
        /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="DrawingBuilderDependency"/>。</param>
        public DrawingBuilder(IExtensionBuilder parentBuilder, DrawingBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IDrawingBuilder>(this);

            AddDrawingServices();
        }


        private void AddDrawingServices()
        {
            // Services
            AddService<ICaptchaService, CaptchaService>();
            AddService<IScaleService, ScaleService>();
            AddService<IWatermarkService, WatermarkService>();
        }


        /// <summary>
        /// 获取服务特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => DrawingBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);

    }
}
