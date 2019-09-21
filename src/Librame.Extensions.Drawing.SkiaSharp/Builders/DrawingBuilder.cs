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

namespace Librame.Extensions.Drawing
{
    using Core;

    class DrawingBuilder : AbstractExtensionBuilder, IDrawingBuilder
    {
        public DrawingBuilder(IExtensionBuilder builder, DrawingBuilderDependencyOptions dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Services.AddSingleton<IDrawingBuilder>(this);
        }

    }
}
