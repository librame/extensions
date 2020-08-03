#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    /// <summary>
    /// <see cref="IDataBuilder"/> 数据库上下文选项扩展。
    /// </summary>
    public class DataBuilderDbContextOptionsExtension : IDbContextOptionsExtension
    {
        private IDataBuilder _dataBuilder;
        private IServiceProvider _serviceProvider;


        /// <summary>
        /// 构造一个 <see cref="DataBuilderDbContextOptionsExtension"/>。
        /// </summary>
        public DataBuilderDbContextOptionsExtension()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="DataBuilderDbContextOptionsExtension"/>。
        /// </summary>
        /// <param name="copyFrom">给定的 <see cref="DataBuilderDbContextOptionsExtension"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected DataBuilderDbContextOptionsExtension(DataBuilderDbContextOptionsExtension copyFrom)
        {
            copyFrom.NotNull(nameof(copyFrom));

            _dataBuilder = copyFrom._dataBuilder;
            _serviceProvider = copyFrom._serviceProvider;
        }


        /// <summary>
        /// 数据库上下文选项扩展信息。
        /// </summary>
        public virtual DbContextOptionsExtensionInfo Info
            => new OptionsExtensionInfo(this);

        /// <summary>
        /// 数据库上下文选项扩展。
        /// </summary>
        /// <returns>返回 <see cref="DataBuilderDbContextOptionsExtension"/>。</returns>
        protected virtual DataBuilderDbContextOptionsExtension Clone()
            => new DataBuilderDbContextOptionsExtension(this);


        /// <summary>
        /// 数据构建器。
        /// </summary>
        public virtual IDataBuilder DataBuilder
            => _dataBuilder;

        /// <summary>
        /// 服务提供程序。
        /// </summary>
        public virtual IServiceProvider ServiceProvider
            => _serviceProvider;


        /// <summary>
        /// 带有指定 <see cref="IDataBuilder"/> 的副本实例。
        /// </summary>
        /// <param name="dataBuilder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>（可选）。</param>
        /// <returns>返回 <see cref="DataBuilderDbContextOptionsExtension"/>。</returns>
        public virtual DataBuilderDbContextOptionsExtension WithDataBuilder(IDataBuilder dataBuilder,
            IServiceProvider serviceProvider = null)
        {
            dataBuilder.NotNull(nameof(dataBuilder));

            var clone = Clone();
            clone._dataBuilder = dataBuilder;

            if (serviceProvider.IsNotNull())
                clone._serviceProvider = serviceProvider;

            return clone;
        }

        /// <summary>
        /// 带有指定 <see cref="IServiceProvider"/> 的副本实例。
        /// </summary>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="DataBuilderDbContextOptionsExtension"/>。</returns>
        public virtual DataBuilderDbContextOptionsExtension WithServiceProvider(IServiceProvider serviceProvider)
        {
            serviceProvider.NotNull(nameof(serviceProvider));

            var clone = Clone();
            clone._serviceProvider = serviceProvider;

            return clone;
        }


        /// <summary>
        /// 应用服务集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        public virtual void ApplyServices(IServiceCollection services)
        {
        }

        /// <summary>
        /// 验证。
        /// </summary>
        /// <param name="options">给定的 <see cref="IDbContextOptions"/>。</param>
        public virtual void Validate(IDbContextOptions options)
        {
        }


        /// <summary>
        /// 提取 <see cref="DataBuilderDbContextOptionsExtension"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IDbContextOptions"/>。</param>
        /// <returns>返回 <see cref="DataBuilderDbContextOptionsExtension"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        [SuppressMessage("Globalization", "CA1303:请不要将文本作为本地化参数传递", Justification = "<挂起>")]
        public static DataBuilderDbContextOptionsExtension Extract(IDbContextOptions options)
        {
            options.NotNull(nameof(options));

            var dataBuilderOptionsExtensions = options.Extensions
                .OfType<DataBuilderDbContextOptionsExtension>()
                .ToList();

            if (dataBuilderOptionsExtensions.Count == 0)
                throw new InvalidOperationException("No data builder dbcontext options extension are configured.");

            if (dataBuilderOptionsExtensions.Count > 1)
                throw new InvalidOperationException("Multiple data builder dbcontext options extension configurations found.");

            return dataBuilderOptionsExtensions[0];
        }


        private sealed class OptionsExtensionInfo : DbContextOptionsExtensionInfo
        {
            private string _logFragment;


            public OptionsExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension)
            {
            }


            public new DataBuilderDbContextOptionsExtension Extension
                => (DataBuilderDbContextOptionsExtension)base.Extension;

            public override bool IsDatabaseProvider
                => false;

            public override string LogFragment
            {
                get
                {
                    if (_logFragment == null)
                    {
                        var builder = new StringBuilder();

                        if (Extension._dataBuilder != null)
                        {
                            builder
                                .Append("DataBuilder=")
                                .Append(Extension._dataBuilder.GetType().GetDisplayNameWithNamespace())
                                .Append(' ');

                            builder
                                .Append("DataBuilderDependency=")
                                .Append(Extension._dataBuilder.Dependency.GetType().GetDisplayNameWithNamespace())
                                .Append(' ');

                            builder
                                .Append("DatabaseDesignTimeType=")
                                .Append(Extension._dataBuilder.DatabaseDesignTimeType.GetDisplayNameWithNamespace())
                                .Append(' ');
                        }

                        if (Extension._serviceProvider != null)
                        {
                            builder
                                .Append("ServiceProvider=")
                                .Append(Extension._serviceProvider.GetType().GetDisplayNameWithNamespace())
                                .Append(' ');
                        }

                        _logFragment = builder.ToString();
                    }

                    return _logFragment;
                }
            }


            public override long GetServiceProviderHashCode()
                => Extension._dataBuilder?.GetType().GetHashCode() ?? 0
                ^ Extension._serviceProvider?.GetType().GetHashCode() ?? 0;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
                debugInfo["DataBuilder:" + nameof(DataBuilderDbContextOptionsBuilder.UseDataBuilder)] =
                    (Extension._dataBuilder?.GetType().GetHashCode() ?? 0).ToString(CultureInfo.InvariantCulture);

                debugInfo["DataBuilder:" + nameof(DataBuilderDbContextOptionsBuilder.UseServiceProvider)] =
                    (Extension._serviceProvider?.GetType().GetHashCode() ?? 0).ToString(CultureInfo.InvariantCulture);
            }

        }
    }
}
