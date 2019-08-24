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

    class SigningCredentialsService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, ISigningCredentialsService
    {
        private readonly ConcurrentDictionary<string, SigningCredentials> _credentials;


        public SigningCredentialsService(IEnumerable<KeyValuePair<string, SigningCredentials>> credentials,
            IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _credentials = new ConcurrentDictionary<string, SigningCredentials>(credentials);
        }


        public SigningCredentials GetGlobalSigningCredentials()
        {
            return GetSigningCredentials(EncryptionBuilderOptions.GLOBAL_KEY);
        }

        public SigningCredentials GetSigningCredentials(string key)
        {
            return _credentials[key];
        }

    }
}
