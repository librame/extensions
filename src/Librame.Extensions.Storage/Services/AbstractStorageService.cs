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
using System.Text;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 抽象存储服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public class AbstractStorageService<TService> : AbstractService<TService>, IService
        where TService : class, IService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStorageService{TService}"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{StorageBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{TService}"/>。</param>
        public AbstractStorageService(IOptions<StorageBuilderOptions> options,
            ILogger<TService> logger)
            : base(logger)
        {
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        public StorageBuilderOptions Options { get; }

        /// <summary>
        /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
