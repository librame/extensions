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

    /// <summary>
    /// 内部图画构建器。
    /// </summary>
    internal class InternalDrawingBuilder : AbstractExtensionBuilder, IDrawingBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalDrawingBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        public InternalDrawingBuilder(IExtensionBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<IDrawingBuilder>(this);
        }

    }
}
