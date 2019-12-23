#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Builders
{
    using Core.Builders;

    /// <summary>
    /// 图画构建器依赖。
    /// </summary>
    public class DrawingBuilderDependency : AbstractExtensionBuilderDependency<DrawingBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DrawingBuilderDependency"/>。
        /// </summary>
        public DrawingBuilderDependency()
            : base(nameof(DrawingBuilderDependency))
        {
        }

    }
}
