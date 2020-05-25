#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Builders
{
    /// <summary>
    /// 扩展构建器适配器接口。
    /// </summary>
    /// <typeparam name="TAdaptionBuilder">指定的适配构建器类型。</typeparam>
    public interface IExtensionBuilderAdapter<out TAdaptionBuilder> : IExtensionBuilder
        where TAdaptionBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 适配构建器。
        /// </summary>
        TAdaptionBuilder AdaptionBuilder { get; }
    }
}
