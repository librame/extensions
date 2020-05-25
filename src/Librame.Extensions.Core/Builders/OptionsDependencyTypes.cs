#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Core.Builders
{
    using Dependencies;

    /// <summary>
    /// 选项依赖类型集合。
    /// </summary>
    internal static class OptionsDependencyTypes
    {
        /// <summary>
        /// <see cref="IDependency"/> 类型。
        /// </summary>
        public static readonly Type DependencyType
            = typeof(IDependency);


        /// <summary>
        /// <see cref="IOptions{TOptions}"/> 类型定义。
        /// </summary>
        public static readonly Type OptionsTypeDefinition
            = typeof(IOptions<>);

        /// <summary>
        /// <see cref="IConfigureOptions{TOptions}"/> 类型定义。
        /// </summary>
        public static readonly Type ConfigureOptionsTypeDefinition
            = typeof(IConfigureOptions<>);

        /// <summary>
        /// <see cref="IOptionsChangeTokenSource{TOptions}"/> 类型定义。
        /// </summary>
        public static readonly Type OptionsChangeTokenSourceTypeDefinition
            = typeof(IOptionsChangeTokenSource<>);


        /// <summary>
        /// <see cref="ConfigurationChangeTokenSource{TOptions}"/> 类型定义。
        /// </summary>
        public static readonly Type ConfigurationChangeTokenSourceTypeDefinition
            = typeof(ConfigurationChangeTokenSource<>);

        /// <summary>
        /// <see cref="NamedConfigureFromConfigurationOptions{TOptions}"/> 类型定义。
        /// </summary>
        public static readonly Type NamedConfigureFromConfigurationOptionsTypeDefinition
            = typeof(NamedConfigureFromConfigurationOptions<>);


        /// <summary>
        /// <see cref="ConfigureNamedOptions{TOptions}"/> 类型定义。
        /// </summary>
        public static readonly Type ConfigureNamedOptionsTypeDefinition
            = typeof(ConfigureNamedOptions<>);


        /// <summary>
        /// <see cref="IPostConfigureOptions{TOptions}"/> 类型定义。
        /// </summary>
        public static readonly Type BasePostConfigureOptionsTypeDefinition
            = typeof(IPostConfigureOptions<>);

        /// <summary>
        /// <see cref="PostConfigureOptions{TOptions}"/> 类型定义。
        /// </summary>
        public static readonly Type PostConfigureOptionsTypeDefinition
            = typeof(PostConfigureOptions<>);
    }
}
