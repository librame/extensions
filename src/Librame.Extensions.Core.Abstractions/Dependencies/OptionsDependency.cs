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
        public OptionsDependency()
            : this(BuildName<TOptions>(out Type optionsType), optionsType)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="OptionsDependency{TOptions}"/>。
        /// </summary>
        /// <param name="name">给定的依赖名称。</param>
        protected OptionsDependency(string name)
            : this(name, typeof(TOptions))
        {
        }

        private OptionsDependency(string name, Type optionsType)
            : base(name)
        {
            if (optionsType.IsAssignableToBaseType(typeof(IExtensionBuilderDependency)))
                throw new ArgumentException(InternalResource.ArgumentExceptionNotSupportedConfigurationOfDependencyOptions);

            OptionsType = new SerializableString<Type>(optionsType);
            Options = OptionsType.Source.EnsureCreate<TOptions>();
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
