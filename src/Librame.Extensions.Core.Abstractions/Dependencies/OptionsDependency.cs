#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Dependencies
{
    using Builders;
    using Resources;
    using Serializers;

    /// <summary>
    /// 选项依赖。
    /// </summary>
    /// <typeparam name="TOptions">指定的选项类型。</typeparam>
    public class OptionsDependency<TOptions> : AbstractDependency, IOptionsDependency
        where TOptions : class
    {
        /// <summary>
        /// 构造一个 <see cref="OptionsDependency{TOptions}"/>。
        /// </summary>
        /// <param name="configureOptions">给定的配置选项（可选）。</param>
        /// <param name="initialOptions">给定的初始化选项实例（可选；默认使用选项类型构造）。</param>
        /// <param name="autoConfigureOptions">自动配置选项（可选；默认自动配置）。</param>
        /// <param name="autoPostConfigureOptions">自动后置配置选项（可选；默认不自动配置）。</param>
        public OptionsDependency(Action<TOptions> configureOptions = null, TOptions initialOptions = null,
            bool autoConfigureOptions = true, bool autoPostConfigureOptions = false)
            : base(BuildName<TOptions>(out Type optionsType))
        {
            if (optionsType.IsAssignableToBaseType(typeof(IExtensionBuilderDependency)))
                throw new ArgumentException(InternalResource.ArgumentExceptionNotSupportedConfigurationOfDependencyOptions);

            OptionsType = SerializableObjectHelper.CreateType(optionsType);
            Options = initialOptions ?? OptionsType.Source.EnsureCreate<TOptions>();

            ConfigureOptions = configureOptions ?? (_ => { });
            AutoConfigureOptions = autoConfigureOptions;
            AutoPostConfigureOptions = autoPostConfigureOptions;
        }


        /// <summary>
        /// 选项类型。
        /// </summary>
        public SerializableObject<Type> OptionsType { get; }

        /// <summary>
        /// 选项实例。
        /// </summary>
        public TOptions Options { get; }


        /// <summary>
        /// 配置选项。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<TOptions> ConfigureOptions { get; set; }

        /// <summary>
        /// 自动配置选项（默认自动）。
        /// </summary>
        public bool AutoConfigureOptions { get; set; }

        /// <summary>
        /// 自动后置配置选项。
        /// </summary>
        public bool AutoPostConfigureOptions { get; set; }
    }
}
