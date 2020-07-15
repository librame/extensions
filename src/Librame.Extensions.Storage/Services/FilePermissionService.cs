#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage.Services
{
    using Core.Services;
    using Core.Utilities;
    using Storage.Builders;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class FilePermissionService : AbstractExtensionBuilderService<StorageBuilderOptions>, IFilePermissionService
    {
        public FilePermissionService(IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        private Task<string> GenerateTokenAsync(string idTraceName, CancellationToken cancellationToken)
        {
            return cancellationToken.RunOrCancelAsync(() =>
            {
                var buffer = RandomUtility.GenerateByteArray(32);
                var token = buffer.AsBase64String();
                Logger.LogTrace($"Generate {idTraceName}: {token}");

                return token;
            });
        }


        public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
            => GenerateTokenAsync("access token", cancellationToken);

        public Task<string> GetAuthorizationCodeAsync(CancellationToken cancellationToken = default)
            => GenerateTokenAsync("authorization code", cancellationToken);

        public Task<string> GetCookieValueAsync(CancellationToken cancellationToken = default)
            => GenerateTokenAsync("cookie value", cancellationToken);
    }
}
