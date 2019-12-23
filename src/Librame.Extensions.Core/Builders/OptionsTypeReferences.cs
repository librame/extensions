#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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
    /// 选项类型引用集合。
    /// </summary>
    public static class OptionsTypeReferences
    {
        /// <summary>
        /// <see cref="IDependency"/> 类型。
        /// </summary>
        public static readonly Type BaseDependencyType
            = typeof(IDependency);


        /// <summary>
        /// <see cref="IOptions{TOptions}"/> 类型。
        /// </summary>
        public static readonly Type BaseOptionsType
            = typeof(IOptions<>);

        /// <summary>
        /// <see cref="IConfigureOptions{TOptions}"/> 类型。
        /// </summary>
        public static readonly Type BaseConfigureOptionsType
            = typeof(IConfigureOptions<>);

        /// <summary>
        /// <see cref="IOptionsChangeTokenSource{TOptions}"/> 类型。
        /// </summary>
        public static readonly Type BaseOptionsChangeTokenSourceType
            = typeof(IOptionsChangeTokenSource<>);


        /// <summary>
        /// <see cref="ConfigurationChangeTokenSource{TOptions}"/> 类型。
        /// </summary>
        public static readonly Type ConfigurationChangeTokenSourceType
            = typeof(ConfigurationChangeTokenSource<>);

        /// <summary>
        /// <see cref="NamedConfigureFromConfigurationOptions{TOptions}"/> 类型。
        /// </summary>
        public static readonly Type NamedConfigureFromConfigurationOptionsType
            = typeof(NamedConfigureFromConfigurationOptions<>);


        /// <summary>
        /// <see cref="ConfigureNamedOptions{TOptions}"/> 类型。
        /// </summary>
        public static readonly Type ConfigureNamedOptionsType
            = typeof(ConfigureNamedOptions<>);


        /// <summary>
        /// <see cref="IPostConfigureOptions{TOptions}"/> 类型。
        /// </summary>
        public static readonly Type BasePostConfigureOptionsType
            = typeof(IPostConfigureOptions<>);

        /// <summary>
        /// <see cref="PostConfigureOptions{TOptions}"/> 类型。
        /// </summary>
        public static readonly Type PostConfigureOptionsType
            = typeof(PostConfigureOptions<>);
    }
}
