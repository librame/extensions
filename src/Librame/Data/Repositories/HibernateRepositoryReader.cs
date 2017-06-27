#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Librame.Data.Repositories
{
    using Providers;

    /// <summary>
    /// NHibernate 仓库读取器。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class HibernateRepositoryReader<T> : HibernateRepositoryWriter<T>, IRepositoryReader<T>
        where T : class
    {
        /// <summary>
        /// 构造一个 <see cref="HibernateRepository{T}"/> 实例。
        /// </summary>
        /// <param name="provider">给定的 <see cref="HibernateProvider"/>。</param>
        public HibernateRepositoryReader(HibernateProvider provider)
            : base(provider)
        {
        }


        /// <summary>
        /// 复制类型实例。
        /// </summary>
        /// <param name="source">给定的源类型实例。</param>
        /// <param name="target">给定的目标类型实例。</param>
        public virtual void Copy(T source, T target)
        {
            Log.DebugFormat("Copy {0} {1}", source, target);

            ReadyProvider((p, f, s) =>
            {
                var metadata = f.GetClassMetadata(typeof(T));
                var values = metadata.GetPropertyValues(source, EntityMode.Poco);

                //This method is currently only used by StorageVersionFilter<>.Versioning()
                //In order to prevent shared references to the same collection instance
                //Instances of IList<> need to be copied to a new collection instance
                for (var index = 0; index < values.Length; index++)
                {
                    var value = values[index];
                    if (value == null)
                        continue;

                    var type = value.GetType();
                    var isGenericList = type.GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .Any(i => i.GetGenericTypeDefinition() == typeof(IList<>));

                    if (!isGenericList)
                        continue;

                    var genericArgument = type.GetGenericArguments().First();
                    var genericType = typeof(List<>).MakeGenericType(new[] { genericArgument });

                    var listValues = ((IList)value);
                    values[index] = Activator.CreateInstance(genericType, new[] { listValues });

                    metadata.SetPropertyValues(target, values, EntityMode.Poco);
                }
            });
        }


        /// <summary>
        /// 计数查询。
        /// </summary>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则计算所有条数）。</param>
        /// <returns>返回整数。</returns>
        public virtual int Count(Expression<Func<T, bool>> predicate = null)
        {
            return Ready(q => q.AsWhere(predicate).Count());
        }


        /// <summary>
        /// 获取单条指定编号的类型实例。
        /// </summary>
        /// <param name="id">给定的编号。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T Get(object id)
        {
            return ReadyProvider((p, f, s) => s.Get<T>(id));
        }

        /// <summary>
        /// 获取单条符合指定查询表达式的数据。
        /// </summary>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则根据唯一性要求查询第一条数据）。</param>
        /// <param name="isUnique">是否要求唯一性（如果为 True，则表示查询出多条数据将会抛出异常）。</param>
        /// <returns>返回对象。</returns>
        public virtual T Get(Expression<Func<T, bool>> predicate = null, bool isUnique = true)
        {
            return Ready(q => q.AsOne(predicate, isUnique));
        }


        /// <summary>
        /// 获取多条符合指定查询表达式的数据集合。
        /// </summary>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="order">给定的排序方法（可选；如果为空，则采用默认排序）。</param>
        /// <returns>返回数组。</returns>
        public virtual T[] GetMany(Expression<Func<T, bool>> predicate = null, Action<Orderable<T>> order = null)
        {
            return Ready(q => q.AsWhere(predicate).AsOrder(order).ToArray());
        }


        /// <summary>
        /// 获取分页数据。
        /// </summary>
        /// <param name="createInfoFactory">给定创建分页信息的方法。</param>
        /// <param name="order">给定的排序方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <returns>返回 <see cref="IPageable{T}"/>。</returns>
        public virtual IPageable<T> GetPaging(Func<int, PagingInfo> createInfoFactory,
            Action<Orderable<T>> order, Expression<Func<T, bool>> predicate = null)
        {
            return Ready(q => q.AsWhere(predicate).AsPaging(order, createInfoFactory));
        }


        /// <summary>
        /// 获取指定属性值集合。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="selector">给定的属性选择器。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则根据唯一性要求查询第一条数据）。</param>
        /// <param name="removeDuplicates">是否移除重复项（可选；默认移除重复项）。</param>
        /// <returns>返回数组。</returns>
        public virtual TProperty[] GetProperties<TProperty>(Expression<Func<T, TProperty>> selector,
            Expression<Func<T, bool>> predicate = null, bool removeDuplicates = true)
        {
            return Ready(q => q.AsWhere(predicate).AsSelectProperties(selector, removeDuplicates).ToArray());
        }


        #region IRepositoryReader<T>

        void IRepositoryReader<T>.Copy(T source, T target)
        {
            Copy(source, target);
        }


        int IRepositoryReader<T>.Count(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate);
        }


        T IRepositoryReader<T>.Get(object id)
        {
            return Get(id);
        }

        T IRepositoryReader<T>.Get(Expression<Func<T, bool>> predicate, bool isUnique)
        {
            return Get(predicate, isUnique);
        }


        T[] IRepositoryReader<T>.GetMany(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            return GetMany(predicate, order);
        }


        IPageable<T> IRepositoryReader<T>.GetPaging(Func<int, PagingInfo> createInfoFactory,
            Action<Orderable<T>> order, Expression<Func<T, bool>> predicate)
        {
            return GetPaging(createInfoFactory, order, predicate);
        }


        TProperty[] IRepositoryReader<T>.GetProperties<TProperty>(Expression<Func<T, TProperty>> selector,
            Expression<Func<T, bool>> predicate, bool removeDuplicates)
        {
            return GetProperties(selector, predicate, removeDuplicates);
        }

        #endregion

    }
}
