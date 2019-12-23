#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Services
{
    using Threads;

    /// <summary>
    /// 并发服务接口。
    /// </summary>
    public interface IConcurrentService : IService
    {
        /// <summary>
        /// 内存锁定器。
        /// </summary>
        IMemoryLocker Locker { get; }
    }
}
