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
    /// 依赖代理接口。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    public interface IDependencyProxy<TInterface>
        where TInterface : class
    {
        /// <summary>
        /// 调用依赖。
        /// </summary>
        IInvokeDependency<TInterface> Dependency { get; }

        /// <summary>
        /// 源实例。
        /// </summary>
        TInterface Source { get; set; }
    }
}
