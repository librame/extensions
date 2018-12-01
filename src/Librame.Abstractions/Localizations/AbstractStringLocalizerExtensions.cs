#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Localizations;
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace Microsoft.Extensions.Localization
{
    /// <summary>
    /// 抽象字符串定位器静态扩展。
    /// </summary>
    public static class AbstractStringLocalizerExtensions
    {

        #region IStringLocalizer

        /// <summary>
        /// 获取字符串。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <param name="localizer">给定的 <see cref="IStringLocalizer{TResource}"/>。</param>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        public static LocalizedString GetString<TResource>(this IStringLocalizer<TResource> localizer,
            Expression<Func<TResource, string>> propertyExpression)
        {
            string name = propertyExpression.AsPropertyName();

            return localizer[name];
        }
        /// <summary>
        /// 获取字符串。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <param name="localizer">给定的 <see cref="IStringLocalizer{TResource}"/>。</param>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="arguments">给定的参数数组。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        public static LocalizedString GetString<TResource>(this IStringLocalizer<TResource> localizer,
            Expression<Func<TResource, string>> propertyExpression, params object[] arguments)
        {
            string name = propertyExpression.AsPropertyName();

            return localizer[name, arguments];
        }

        #endregion


        /// <summary>
        /// 创建一个指定文化名称的字符串定位器副本。
        /// </summary>
        /// <param name="localizer">给定的 <see cref="IStringLocalizer"/>。</param>
        /// <param name="cultureName">给定的文化名称。</param>
        /// <returns>返回 <see cref="IStringLocalizer"/>。</returns>
        public static IStringLocalizer WithCulture(this IStringLocalizer localizer, string cultureName)
        {
            return localizer.WithCulture(CultureInfo.CreateSpecificCulture(cultureName));
        }
        
        /// <summary>
        /// 创建一个指定文化名称的字符串定位器副本。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <param name="localizer">给定的 <see cref="IStringLocalizer{TResource}"/>。</param>
        /// <param name="cultureName">给定的文化名称。</param>
        /// <returns>返回 <see cref="IStringLocalizer{TResource}"/>。</returns>
        public static IStringLocalizer<TResource> WithCulture<TResource>(this IStringLocalizer<TResource> localizer, string cultureName)
        {
            return localizer.WithCulture<TResource>(CultureInfo.CreateSpecificCulture(cultureName));
        }
        /// <summary>
        /// 创建一个指定文化名称的字符串定位器副本。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <param name="localizer">给定的 <see cref="IStringLocalizer{TResource}"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <returns>返回 <see cref="IStringLocalizer{TResource}"/>。</returns>
        public static IStringLocalizer<TResource> WithCulture<TResource>(this IStringLocalizer<TResource> localizer, CultureInfo culture)
        {
            localizer.WithCulture(culture);

            return localizer;
        }

        /// <summary>
        /// 创建一个指定文化名称的字符串定位器副本。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <param name="localizer">给定的 <see cref="IEnhancedStringLocalizer{TResource}"/>。</param>
        /// <param name="cultureName">给定的文化名称。</param>
        /// <returns>返回 <see cref="IEnhancedStringLocalizer{TResource}"/>。</returns>
        public static IEnhancedStringLocalizer<TResource> WithCulture<TResource>(this IEnhancedStringLocalizer<TResource> localizer, string cultureName)
        {
            return localizer.WithCulture<TResource>(CultureInfo.CreateSpecificCulture(cultureName));
        }
        /// <summary>
        /// 创建一个指定文化名称的字符串定位器副本。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <param name="localizer">给定的 <see cref="IEnhancedStringLocalizer{TResource}"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <returns>返回 <see cref="IEnhancedStringLocalizer{TResource}"/>。</returns>
        public static IEnhancedStringLocalizer<TResource> WithCulture<TResource>(this IEnhancedStringLocalizer<TResource> localizer, CultureInfo culture)
        {
            localizer.WithCulture(culture);

            return localizer;
        }

    }
}
