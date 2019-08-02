#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 存储构建器静态扩展。
    /// </summary>
    public static class StorageBuilderExtensions
    {
        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IExtensionBuilder builder,
            Action<StorageBuilderOptions> setupAction = null)
        {
            return builder.AddStorage(b => new InternalStorageBuilder(b), setupAction);
        }

        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="createFactory">给定创建存储构建器的工厂方法。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IExtensionBuilder builder,
            Func<IExtensionBuilder, IStorageBuilder> createFactory,
            Action<StorageBuilderOptions> setupAction = null)
        {
            createFactory.NotNull(nameof(createFactory));

            // Add Builder
            builder.Services.OnlyConfigure(setupAction);

            var storageBuilder = createFactory.Invoke(builder);

            return storageBuilder
                .AddServices();
        }

    }
}
