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
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;

namespace Librame.Data
{
    using Utility;

    /// <summary>
    /// 属性绑定器。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    public class PropertyBinder<TEntity> : IPropertyBinder<TEntity>
    {
        private readonly ConcurrentDictionary<string, BindingMarkup> _properties = null;

        /// <summary>
        /// 构造一个属性绑定器实例。
        /// </summary>
        public PropertyBinder()
        {
            _properties = new ConcurrentDictionary<string, BindingMarkup>();
        }


        /// <summary>
        /// 绑定属性。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="markup">给定的绑定标记（可选；默认为所有）。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        public virtual IPropertyBinder<TEntity> Bind(string propertyName, BindingMarkup markup = BindingMarkup.All)
        {
            _properties.AddOrUpdate(propertyName, markup, (key, value) => markup);

            return this;
        }

        /// <summary>
        /// 绑定属性。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="markup">给定的绑定标记（可选；默认为所有）。</param>
        /// <returns>返回 <see cref="IPropertyBinder{TEntity}"/>。</returns>
        public virtual IPropertyBinder<TEntity> Bind<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression,
            BindingMarkup markup = BindingMarkup.All)
        {
            var propertyName = ExpressionUtility.AsPropertyName(propertyExpression);
            _properties.AddOrUpdate(propertyName, markup, (key, value) => markup);

            return this;
        }


        /// <summary>
        /// 清空绑定的属性集合。
        /// </summary>
        public virtual void Clear()
        {
            _properties.Clear();
        }


        /// <summary>
        /// 导出绑定的属性集合。
        /// </summary>
        /// <param name="markup">给定要导出的绑定标记（可选；默认为所有）。</param>
        /// <returns>返回属性名称数组。</returns>
        public virtual string[] Export(BindingMarkup markup = BindingMarkup.All)
        {
            if (markup != BindingMarkup.All)
                return _properties.Where(pair => pair.Value == markup).Select(s => s.Key).ToArray();

            return _properties.Select(s => s.Key).ToArray();
        }

    }
}
