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

namespace Librame.Extensions.Drawing.Builders
{
    using Core.Builders;

    internal class DrawingBuilder : AbstractExtensionBuilder, IDrawingBuilder
    {
        public DrawingBuilder(IExtensionBuilder parentBuilder, DrawingBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IDrawingBuilder>(this);
        }

    }
}
