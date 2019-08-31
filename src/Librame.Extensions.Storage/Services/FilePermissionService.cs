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


        private Task<string> GenerateTokenAsync(CancellationToken cancellationToken, string idTraceName)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string token = RandomNumberIdentifier.New();
                Logger.LogTrace($"Generate {idTraceName}: {token}");

                return token;
            });
        }


        public Task<string> GeAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            return GenerateTokenAsync(cancellationToken, "access token");
        }

        public Task<string> GetAuthorizationCodeAsync(CancellationToken cancellationToken = default)
        {
            return GenerateTokenAsync(cancellationToken, "authorization code");
        }

        public Task<string> GetCookieValueAsync(CancellationToken cancellationToken = default)
        {
            return GenerateTokenAsync(cancellationToken, "cookie value");
        }

    }
}
