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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 数据构建器静态扩展。
    /// </summary>
    public static class DataBuilderExtensions
    {
        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <param name="createFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder builder,
            Action<DataBuilderOptions> setupAction = null,
            Func<IExtensionBuilder, IDataBuilder> createFactory = null)
        {
            // Add Builder
            builder.Services.OnlyConfigure(setupAction);

            var dataBuilder = (createFactory ??
                (b => new DataBuilder(b))).Invoke(builder);

            return dataBuilder
                .AddMediators()
                .AddServices()
                .AddStores();
        }

    }
}
