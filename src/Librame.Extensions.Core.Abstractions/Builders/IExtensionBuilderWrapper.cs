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
    /// 扩展构建器封装器接口。
    /// </summary>
    /// <typeparam name="TBuilder">指定的构建器类型。</typeparam>
    public interface IExtensionBuilderWrapper<out TBuilder> : IExtensionBuilder, IBuilderWrapper<TBuilder>
        where TBuilder : class
    {
    }
}
