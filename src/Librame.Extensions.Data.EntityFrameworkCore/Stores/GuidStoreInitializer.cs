#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// <see cref="Guid"/> 存储初始化器。
    /// </summary>
    public class GuidStoreInitializer : AbstractStoreInitializer<Guid>
    {
        /// <summary>
        /// 构造一个存储初始化器。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier{Guid}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected GuidStoreInitializer(IStoreIdentifier<Guid> identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
        }

    }
}
