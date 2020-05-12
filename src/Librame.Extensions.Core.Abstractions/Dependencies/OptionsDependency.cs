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
    using Options;
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
        /// <param name="configureOptions">给定的选项配置动作（可选）。</param>
        public OptionsDependency(Action<TOptions> configureOptions = null)
            : this(BuildName<TOptions>(out Type optionsType), optionsType, configureOptions)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="OptionsDependency{TOptions}"/>。
        /// </summary>
        /// <param name="name">给定的依赖名称。</param>
        /// <param name="configureOptions">给定的选项配置动作（可选）。</param>
        protected OptionsDependency(string name, Action<TOptions> configureOptions = null)
            : this(name, typeof(TOptions), configureOptions)
        {
        }

        private OptionsDependency(string name, Type optionsType, Action<TOptions> configureOptions = null)
            : base(name)
        {
            if (optionsType.IsAssignableToBaseType(typeof(IExtensionBuilderDependency)))
                throw new ArgumentException(InternalResource.ArgumentExceptionNotSupportedConfigurationOfDependency);

            OptionsType = new SerializableString<Type>(optionsType);
            Options = ConsistencyOptionsCache.GetOrAdd<TOptions>();
            configureOptions?.Invoke(Options);
        }


        /// <summary>
        /// 选项类型。
        /// </summary>
        public SerializableString<Type> OptionsType { get; }

        /// <summary>
        /// 选项实例。
        /// </summary>
        public TOptions Options { get; }
    }
}
