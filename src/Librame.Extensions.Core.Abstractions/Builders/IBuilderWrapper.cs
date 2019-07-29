#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 构建器封装器接口。
    /// </summary>
    /// <typeparam name="TBuilder">指定的构建器类型。</typeparam>
    public interface IBuilderWrapper<out TBuilder>
        where TBuilder : class
    {
        /// <summary>
        /// 原始构建器。
        /// </summary>
        TBuilder RawBuilder { get; }
    }
}
