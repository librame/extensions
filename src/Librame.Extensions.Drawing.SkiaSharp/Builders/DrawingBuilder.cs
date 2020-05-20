#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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

    internal class DrawingBuilder : AbstractExtensionBuilder, IDrawingBuilder
    {
        public DrawingBuilder(IExtensionBuilder parentBuilder, DrawingBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IDrawingBuilder>(this);

            AddDrawingServices();
        }


        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => DrawingBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);


        private void AddDrawingServices()
        {
            // Services
            AddService<ICaptchaService, CaptchaService>();
            AddService<IScaleService, ScaleService>();
            AddService<IWatermarkService, WatermarkService>();
        }

    }
}
