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
using System.Linq.Expressions;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 表达式定位器。
    /// </summary>
    /// <typeparam name="TResource">指定的资源类型。</typeparam>
    public class ExpressionLocalizer<TResource> : StringLocalizer<TResource>, IExpressionLocalizer<TResource>
        where TResource : class
    {
        /// <summary>
        /// 构造一个 <see cref="ExpressionLocalizer{TResource}"/>。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IStringLocalizerFactory"/>。</param>
        public ExpressionLocalizer(IStringLocalizerFactory factory)
            : base(factory)
        {
        }


        /// <summary>
        /// 获取字符串属性的本地化字符串。
        /// </summary>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        public virtual LocalizedString this[Expression<Func<TResource, string>> propertyExpression]
            => this.GetString(propertyExpression);

        /// <summary>
        /// 获取字符串属性的本地化字符串。
        /// </summary>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="arguments">给定的参数数组。</param>
        /// <returns>返回 <see cref="LocalizedString"/>。</returns>
        public virtual LocalizedString this[Expression<Func<TResource, string>> propertyExpression, params object[] arguments]
            => this.GetString(propertyExpression, arguments);
    }
}
