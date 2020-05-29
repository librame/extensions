#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
        /// <param name="parentDependency">给定的父级 <see cref="IExtensionBuilderDependency"/>（可选）。</param>
        public DrawingBuilderDependency(IExtensionBuilderDependency parentDependency = null)
            : this(nameof(DrawingBuilderDependency), parentDependency)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="DrawingBuilderDependency"/>。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="parentDependency">给定的父级 <see cref="IExtensionBuilderDependency"/>（可选）。</param>
        protected DrawingBuilderDependency(string name, IExtensionBuilderDependency parentDependency = null)
            : base(name, parentDependency)
        {
        }

    }
}
