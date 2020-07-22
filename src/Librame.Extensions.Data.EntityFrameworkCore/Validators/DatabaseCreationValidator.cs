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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Validators
{
    using Core.Combiners;
    using Core.Services;
    using Data.Accessors;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class DatabaseCreationValidator : AbstractService, IDatabaseCreationValidator
    {
        public DatabaseCreationValidator(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public bool IsCreated(IAccessor accessor)
        {
            var filePath = GetReportFilePath(accessor as DbContextAccessorBase);
            return filePath.Exists();
        }

        public Task<bool> IsCreatedAsync(IAccessor accessor, CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelAsync(() => IsCreated(accessor));


        public void SetCreated(IAccessor accessor)
        {
            var now = accessor.Clock.GetNowOffset();

            var filePath = GetReportFilePath(accessor as DbContextAccessorBase);
            filePath.WriteAllText($"The database was successfully created at {now}.");
        }

        public async Task SetCreatedAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            var now = await accessor.Clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
                .ConfigureAwait();

            var filePath = GetReportFilePath(accessor as DbContextAccessorBase);
            filePath.WriteAllText($"The database was successfully created at {now}.");
        }


        private static FilePathCombiner GetReportFilePath(DbContextAccessorBase accessor)
            => DbContextAccessorHelper.GenerateAccessorFilePath(accessor,
                ".txt", d => d.DatabasesReportDirectory);

    }
}
