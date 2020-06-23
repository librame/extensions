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

namespace Librame.Extensions.Data.Stores
{
    using Core.Combiners;
    using Core.Services;
    using Data.Accessors;
    using Data.Compilers;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class StoreInitializationValidator : AbstractService, IStoreInitializationValidator
    {
        public StoreInitializationValidator(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public bool IsInitialized(IAccessor accessor)
        {
            var filePath = GetReportFilePath(accessor);
            return filePath.Exists();
        }

        public void SetInitialized(IAccessor accessor)
        {
            var filePath = GetReportFilePath(accessor);

            var now = accessor.Clock.GetNowOffsetAsync().ConfigureAndResult();
            filePath.WriteAllText($"Initialization completed at {now}.");
        }


        private static FilePathCombiner GetReportFilePath(IAccessor accessor)
            => ModelSnapshotCompiler.CombineFilePath(accessor, d => d.InitializersReportDirectory, ".txt");
    }
}
