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
    internal class DataInitializationValidator : AbstractService, IDataInitializationValidator
    {
        public DataInitializationValidator(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public bool IsInitialized(IAccessor accessor)
        {
            var filePath = GetReportFilePath(accessor as DbContextAccessorBase);
            if (filePath.Exists())
                return true;

            if (!accessor.AnySets())
                return false;

            SetInitialized(accessor);
            return true;
        }

        public Task<bool> IsInitializedAsync(IAccessor accessor, CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelAsync(() => IsInitialized(accessor));


        public void SetInitialized(IAccessor accessor)
        {
            var now = accessor.Clock.GetNowOffset();

            var filePath = GetReportFilePath(accessor as DbContextAccessorBase);
            filePath.WriteAllText($"The data was successfully initialized at {now}.");
        }

        public async Task SetInitializedAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            var now = await accessor.Clock.GetNowOffsetAsync().ConfigureAwait();

            var filePath = GetReportFilePath(accessor as DbContextAccessorBase);
            filePath.WriteAllText($"The data was successfully initialized at {now}.");
        }


        private static FilePathCombiner GetReportFilePath(DbContextAccessorBase accessor)
            => DbContextAccessorHelper.GenerateAccessorFilePath(accessor,
                ".txt", d => d.InitializersReportDirectory);

    }
}
