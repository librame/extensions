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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Core;

    class FilePermissionService : ExtensionBuilderServiceBase<StorageBuilderOptions>, IFilePermissionService
    {
        public FilePermissionService(IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        public Task<string> GeAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string accessToken = RandomNumberIdentifier.New();
                Logger.LogInformation($"Get access token: {accessToken}");

                return accessToken;
            });
        }

        public Task<string> GetAuthorizationCodeAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string authorizationCode = RandomNumberIdentifier.New();
                Logger.LogInformation($"Get authorization code: {authorizationCode}");

                return authorizationCode;
            });
        }

        public Task<string> GetCookieValueAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string cookieValue = RandomNumberIdentifier.New();
                Logger.LogInformation($"Get cookie value: {cookieValue}");

                return cookieValue;
            });
        }

    }
}
