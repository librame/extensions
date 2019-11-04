#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象定位器静态扩展。
    /// </summary>
    public static class AbstractionLocalizerExtensions
    {

        #region IStringLocalizer

        /// <summary>
        /// 获取字符串。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="localizer">给定的 <see cref="IStringLocalizer{TResource}"/>。</param>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "localizer")]
        public static LocalizedString GetString<TResource, TProperty>(this IStringLocalizer<TResource> localizer,
            Expression<Func<TResource, TProperty>> propertyExpression)
            where TResource : class
        {
            localizer.NotNull(nameof(localizer));

            var name = propertyExpression.AsPropertyName();
            return localizer[name];
        }

        /// <summary>
        /// 获取字符串。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="localizer">给定的 <see cref="IStringLocalizer{TResource}"/>。</param>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="arguments">给定的参数数组。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "localizer")]
        public static LocalizedString GetString<TResource, TProperty>(this IStringLocalizer<TResource> localizer,
            Expression<Func<TResource, TProperty>> propertyExpression, params object[] arguments)
            where TResource : class
        {
            localizer.NotNull(nameof(localizer));

            var name = propertyExpression.AsPropertyName();
            return localizer[name, arguments];
        }

        #endregion

    }
}
