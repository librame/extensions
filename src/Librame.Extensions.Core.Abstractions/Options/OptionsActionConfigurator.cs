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

namespace Librame.Extensions.Core
{
    using Resources;

    /// <summary>
    /// 选项动作配置器。
    /// </summary>
    /// <typeparam name="TOptions">指定的选项类型。</typeparam>
    public class OptionsActionConfigurator<TOptions> : OptionsConfigurator<TOptions>, IOptionsActionConfigurator
        where TOptions : class
    {
        /// <summary>
        /// 构造一个 <see cref="OptionsActionConfigurator{TOptions}"/>。
        /// </summary>
        /// <param name="action">给定的选项动作（可选）。</param>
        /// <param name="autoConfigureAction">是否自动配置动作（可选；默认自动配置）。</param>
        public OptionsActionConfigurator(Action<TOptions> action = null, bool autoConfigureAction = true)
            : base()
        {
            if (OptionsType.IsAssignableToBaseType(typeof(IExtensionBuilderDependencyOptions)))
                throw new ArgumentException(InternalResource.ArgumentExceptionNotSupportedConfigurationOfDependencyOptions);

            Action = action ?? (_ => { });
            AutoConfigureAction = autoConfigureAction;
        }


        /// <summary>
        /// 动作。
        /// </summary>
        public Action<TOptions> Action { get; set; }

        /// <summary>
        /// 自动配置动作（默认自动）。
        /// </summary>
        public bool AutoConfigureAction { get; set; }

        /// <summary>
        /// 自动后置配置动作。
        /// </summary>
        public bool AutoPostConfigureAction { get; set; }
    }
}
