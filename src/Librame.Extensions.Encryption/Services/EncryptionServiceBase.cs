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
    /// 加密服务基类。
    /// </summary>
    public class EncryptionServiceBase : AbstractService
    {
        /// <summary>
        /// 构造一个 <see cref="EncryptionServiceBase"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{EncryptionBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public EncryptionServiceBase(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 加密构建器选项。
        /// </summary>
        /// <value>返回 <see cref="EncryptionBuilderOptions"/>。</value>
        public EncryptionBuilderOptions Options { get; }
    }
}
