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
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 抽象加密服务。
    /// </summary>
    public abstract class AbstractEncryptionService : AbstractService<EncryptionBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractEncryptionService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{EncryptionBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractEncryptionService(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }

    }
}
