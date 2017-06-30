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
    /// 属性绑定器静态扩展。
    /// </summary>
    public static class PropertyBinderExtensions
    {

        /// <summary>
        /// 绑定用于创建标记的属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="propertyBinder">给定的属性绑定器。</param>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        public static IPropertyBinder<TEntity> BindCreate<TEntity>(this IPropertyBinder<TEntity> propertyBinder,
            string propertyName)
        {
            return propertyBinder.Bind(propertyName, BindingMarkup.Create);
        }
        /// <summary>
        /// 绑定用于创建标记的属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyBinder">给定的属性绑定器。</param>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        public static IPropertyBinder<TEntity> BindCreate<TEntity, TProperty>(this IPropertyBinder<TEntity> propertyBinder,
            Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            return propertyBinder.Bind(propertyExpression, BindingMarkup.Create);
        }


        /// <summary>
        /// 绑定用于修改标记的属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="propertyBinder">给定的属性绑定器。</param>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        public static IPropertyBinder<TEntity> BindEdit<TEntity>(this IPropertyBinder<TEntity> propertyBinder,
            string propertyName)
        {
            return propertyBinder.Bind(propertyName, BindingMarkup.Edit);
        }
        /// <summary>
        /// 绑定用于修改标记的属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyBinder">给定的属性绑定器。</param>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        public static IPropertyBinder<TEntity> BindEdit<TEntity, TProperty>(this IPropertyBinder<TEntity> propertyBinder,
            Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            return propertyBinder.Bind(propertyExpression, BindingMarkup.Edit);
        }


        /// <summary>
        /// 绑定用于删除标记的属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="propertyBinder">给定的属性绑定器。</param>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        public static IPropertyBinder<TEntity> BindDelete<TEntity>(this IPropertyBinder<TEntity> propertyBinder,
            string propertyName)
        {
            return propertyBinder.Bind(propertyName, BindingMarkup.Delete);
        }
        /// <summary>
        /// 绑定用于删除标记的属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyBinder">给定的属性绑定器。</param>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        public static IPropertyBinder<TEntity> BindDelete<TEntity, TProperty>(this IPropertyBinder<TEntity> propertyBinder,
            Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            return propertyBinder.Bind(propertyExpression, BindingMarkup.Delete);
        }


        /// <summary>
        /// 导出用于创建标记的属性集合。
        /// </summary>
        /// <returns>返回属性名称数组。</returns>
        public static string[] ExportCreate<TEntity>(this IPropertyBinder<TEntity> propertyBinder)
        {
            return propertyBinder.Export(BindingMarkup.Create);
        }

        /// <summary>
        /// 导出用于修改标记的属性集合。
        /// </summary>
        /// <returns>返回属性名称数组。</returns>
        public static string[] ExportEdit<TEntity>(this IPropertyBinder<TEntity> propertyBinder)
        {
            return propertyBinder.Export(BindingMarkup.Edit);
        }

        /// <summary>
        /// 导出用于删除标记的属性集合。
        /// </summary>
        /// <returns>返回属性名称数组。</returns>
        public static string[] ExportDelete<TEntity>(this IPropertyBinder<TEntity> propertyBinder)
        {
            return propertyBinder.Export(BindingMarkup.Delete);
        }

    }
}
