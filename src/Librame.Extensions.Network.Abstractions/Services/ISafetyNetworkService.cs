#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network
{
    using Encryption;

    /// <summary>
    /// 安全网络服务接口。
    /// </summary>
    public interface ISafetyNetworkService : INetworkService
    {
        /// <summary>
        /// 散列算法。
        /// </summary>
        IHashService Hash { get; }
    }
}
