#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    using Accessors;
    using Services;

    /// <summary>
    /// 存储接口。
    /// </summary>
    public interface IStore : IInfrastructureService
    {
        /// <summary>
        /// 访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        IAccessor Accessor { get; }
    }
}
