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
    /// <summary>
    /// 选项配置器。
    /// </summary>
    /// <typeparam name="TOptions">指定的选项类型。</typeparam>
    public class OptionsConfigurator<TOptions> : AbstractOptionsConfigurator
        where TOptions : class
    {
        /// <summary>
        /// 构造一个 <see cref="OptionsConfigurator{TOptions}"/>。
        /// </summary>
        public OptionsConfigurator()
            : base(GetOptionsName<TOptions>(out Type optionsType))
        {
            OptionsType = optionsType;
        }


        /// <summary>
        /// 选项类型。
        /// </summary>
        public override Type OptionsType { get; }
    }
}
