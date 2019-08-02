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
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 内部签名证书服务。
    /// </summary>
    internal class InternalSigningCredentialsService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, ISigningCredentialsService
    {
        private readonly ConcurrentDictionary<string, SigningCredentials> _credentials;


        /// <summary>
        /// 构造一个 <see cref="InternalSigningCredentialsService"/>。
        /// </summary>
        /// <param name="credentials">给定的签名证书集合。</param>
        /// <param name="options">给定的 <see cref="IOptions{EncryptionBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalSigningCredentialsService(IEnumerable<KeyValuePair<string, SigningCredentials>> credentials,
            IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _credentials = new ConcurrentDictionary<string, SigningCredentials>(credentials);
        }


        /// <summary>
        /// 获取全局签名证书。
        /// </summary>
        /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
        public SigningCredentials GetGlobalSigningCredentials()
        {
            return GetSigningCredentials(EncryptionBuilderOptions.GLOBAL_KEY);
        }

        /// <summary>
        /// 获取签名证书。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
        public SigningCredentials GetSigningCredentials(string key)
        {
            return _credentials[key];
        }

    }
}
