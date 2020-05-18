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
        /// 构造一个 <see cref="GuidStoreInitializer"/>。
        /// </summary>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator{Guid}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected GuidStoreInitializer(IStoreIdentifierGenerator<Guid> identifierGenerator, ILoggerFactory loggerFactory)
            : base(identifierGenerator, loggerFactory)
        {
        }

    }
}
