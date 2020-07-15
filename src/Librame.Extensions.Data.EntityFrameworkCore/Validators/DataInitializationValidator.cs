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
            return filePath.Exists();
        }

        public Task<bool> IsInitializedAsync(IAccessor accessor, CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelAsync(() => IsInitialized(accessor));


        public void SetInitialized(IAccessor accessor)
        {
            var filePath = GetReportFilePath(accessor as DbContextAccessorBase);

            var now = accessor.Clock.GetNowOffsetAsync().ConfigureAwaitCompleted();
            filePath.WriteAllText($"The data was successfully initialized at {now}.");
        }

        public Task SetInitializedAsync(IAccessor accessor, CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelAsync(() => SetInitialized(accessor));


        private static FilePathCombiner GetReportFilePath(DbContextAccessorBase accessor)
            => DbContextAccessorHelper.GenerateAccessorFilePath(accessor,
                ".txt", d => d.InitializersReportDirectory);

    }
}
