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

    class FilePermissionService : AbstractExtensionBuilderService<StorageBuilderOptions>, IFilePermissionService
    {
        public FilePermissionService(IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        private Task<string> GenerateTokenAsync(CancellationToken cancellationToken, string idTraceName)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string token = RandomNumberAlgorithmIdentifier.New(32);
                Logger.LogTrace($"Generate {idTraceName}: {token}");

                return token;
            });
        }


        public Task<string> GeAccessTokenAsync(CancellationToken cancellationToken = default)
            => GenerateTokenAsync(cancellationToken, "access token");

        public Task<string> GetAuthorizationCodeAsync(CancellationToken cancellationToken = default)
            => GenerateTokenAsync(cancellationToken, "authorization code");

        public Task<string> GetCookieValueAsync(CancellationToken cancellationToken = default)
            => GenerateTokenAsync(cancellationToken, "cookie value");

    }
}
