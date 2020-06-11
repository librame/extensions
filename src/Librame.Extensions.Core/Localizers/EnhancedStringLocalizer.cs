#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Librame.Extensions.Core.Localizers
{
    /// <summary>
    /// 增强型字符串定位器。
    /// </summary>
    public class EnhancedStringLocalizer<TResource> : StringLocalizer<TResource>, IEnhancedStringLocalizer<TResource>
        where TResource : class
    {
        private readonly IStringLocalizerFactory _factory;


        /// <summary>
        /// 构造一个 <see cref="EnhancedStringLocalizer{TResource}"/>。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IStringLocalizerFactory"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public EnhancedStringLocalizer(IStringLocalizerFactory factory)
            : base(factory)
        {
            _factory = factory.NotNull(nameof(factory));
        }


        /// <summary>
        /// 获取属性表达式的本地化字符串。
        /// </summary>
        /// <param name="expression">给定的属性表达式</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        [SuppressMessage("Design", "CA1043:将整型或字符串参数用于索引器")]
        public LocalizedString this[Expression<Func<TResource, string>> expression]
            => this[expression.AsPropertyName()];

        /// <summary>
        /// 获取属性表达式的本地化字符串。
        /// </summary>
        /// <param name="expression">给定的属性表达式</param>
        /// <param name="arguments">给定的格式对象数组。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        public LocalizedString this[Expression<Func<TResource, string>> expression, params object[] arguments]
            => this[expression.AsPropertyName(), arguments];


        /// <summary>
        /// 带有资源。
        /// </summary>
        /// <typeparam name="TNewResource">指定的新资源类型。</typeparam>
        /// <returns>返回 <see cref="IEnhancedStringLocalizer{TNewResource}"/>。</returns>
        public IEnhancedStringLocalizer<TNewResource> WithResource<TNewResource>()
            where TNewResource : class
            => new EnhancedStringLocalizer<TNewResource>(_factory);
    }
}
