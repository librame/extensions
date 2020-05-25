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
    using Decorators;

    /// <summary>
    /// 扩展构建器装饰器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public interface IExtensionBuilderDecorator<out TSource> : IExtensionBuilder, IDecorator<TSource>
        where TSource : class
    {
    }
}
