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
using System.Linq.Expressions;

namespace Librame.Data
{
    /// <summary>
    /// 属性绑定器接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    public interface IPropertyBinder<TEntity>
    {
        /// <summary>
        /// 绑定属性。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="markup">给定的绑定标记（可选；默认为所有）。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        IPropertyBinder<TEntity> Bind(string propertyName, BindingMarkup markup = BindingMarkup.All);

        /// <summary>
        /// 绑定属性。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="markup">给定的绑定标记（可选；默认为所有）。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        IPropertyBinder<TEntity> Bind<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression,
            BindingMarkup markup = BindingMarkup.All);


        /// <summary>
        /// 清空绑定的属性集合。
        /// </summary>
        void Clear();


        /// <summary>
        /// 导出绑定的属性集合。
        /// </summary>
        /// <param name="markup">给定要导出的绑定标记（可选；默认为所有）。</param>
        /// <returns>返回属性名称数组。</returns>
        string[] Export(BindingMarkup markup = BindingMarkup.All);
    }
}
