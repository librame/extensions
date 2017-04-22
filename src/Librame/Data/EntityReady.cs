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
    /// 实体准备。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    public class EntityReady<TEntity>
        where TEntity : class
    {
        private readonly ConcurrentDictionary<string, Expression> _properties = null;

        /// <summary>
        /// 构造一个 <see cref="EntityReady{TEntity}"/> 实例。
        /// </summary>
        public EntityReady()
        {
            _properties = new ConcurrentDictionary<string, Expression>();
        }


        /// <summary>
        /// 准备属性。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <returns>返回 <see cref="EntityReady{TEntity}"/>。</returns>
        public virtual EntityReady<TEntity> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            var propertyName = ExpressionUtility.AsPropertyName(propertyExpression);
            _properties.AddOrUpdate(propertyName, propertyExpression, (key, value) => propertyExpression);

            return this;
        }


        /// <summary>
        /// 清空所有准备对象。
        /// </summary>
        public virtual void Clear()
        {
            _properties.Clear();
        }


        /// <summary>
        /// 转换为属性名集合。
        /// </summary>
        /// <returns>返回字符串数组。</returns>
        public virtual string[] ToProperties()
        {
            return _properties.Keys.ToArray();
        }

    }
}
