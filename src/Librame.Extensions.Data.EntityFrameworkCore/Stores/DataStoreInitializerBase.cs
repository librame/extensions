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
using System;

namespace Librame.Extensions.Data.Stores
{
    using Core.Combiners;
    using Data.Accessors;
    using Data.Compilers;
    using Data.ValueGenerators;

    /// <summary>
    /// 数据存储初始化器基类。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataStoreInitializerBase<TGenId, TIncremId, TCreatedBy> : AbstractDataStoreInitializer<TGenId, TIncremId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个 <see cref="DataStoreInitializerBase{TGenId, TIncremId, TCreatedBy}"/>。
        /// </summary>
        /// <param name="createdByGenerator">给定的 <see cref="IDefaultValueGenerator{TCreatedBy}"/>。</param>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected DataStoreInitializerBase(IDefaultValueGenerator<TCreatedBy> createdByGenerator,
            IStoreIdentifierGenerator<TGenId> identifierGenerator,
            ILoggerFactory loggerFactory)
            : base(createdByGenerator, identifierGenerator, loggerFactory)
        {
        }


        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public override bool IsInitialized(IAccessor accessor)
        {
            var filePath = GetReportFilePath(accessor);
            return filePath.Exists();
        }

        /// <summary>
        /// 设置已完成初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected override void SetInitialized(IAccessor accessor)
        {
            var filePath = GetReportFilePath(accessor);
            filePath.WriteAllText($"Initialization completed at {Clock.GetNowOffsetAsync().ConfigureAndResult()}.");
        }

        private static FilePathCombiner GetReportFilePath(IAccessor accessor)
            => ModelSnapshotCompiler.CombineFilePath(accessor, d => d.InitializersReportDirectory, ".txt");
    }
}
