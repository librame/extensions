#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象选项配置器。
    /// </summary>
    public abstract class AbstractOptionsConfigurator : IOptionsConfigurator
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractOptionsConfigurator"/>。
        /// </summary>
        /// <param name="name">给定的名称（可选）。</param>
        public AbstractOptionsConfigurator(string name = null)
        {
            Name = name;
        }


        /// <summary>
        /// 选项类型。
        /// </summary>
        public abstract Type OptionsType { get; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 配置。
        /// </summary>
        public IConfiguration Configuration { get; set; }


        /// <summary>
        /// 获取选项名称。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <returns>返回字符串。</returns>
        public static string GetOptionsName<TOptions>()
            where TOptions : class
            => GetOptionsName<TOptions>(out _);

        /// <summary>
        /// 获取选项名称。
        /// </summary>
        /// <typeparam name="TOptions">指定的选项类型。</typeparam>
        /// <param name="optionsType">输出选项类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetOptionsName<TOptions>(out Type optionsType)
            where TOptions : class
        {
            optionsType = typeof(TOptions);
            return GetOptionsName(optionsType);
        }

        /// <summary>
        /// 获取选项名称。
        /// </summary>
        /// <param name="optionsType">给定的选项类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetOptionsName(Type optionsType)
            => optionsType.NotNull(nameof(optionsType)).Name.TrimEnd("Options");
    }
}
