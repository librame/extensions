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

namespace Librame.Builders
{
    /// <summary>
    /// 内部图画构建器。
    /// </summary>
    internal class InternalDrawingBuilder : Builder, IDrawingBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalDrawingBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        public InternalDrawingBuilder(IBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<IDrawingBuilder>(this);
        }

    }
}
