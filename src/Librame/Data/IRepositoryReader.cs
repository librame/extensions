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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Librame.Data
{
    /// <summary>
    /// 仓库读取器接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IRepositoryReader<T> : IRepositoryWriter<T>
    {
        /// <summary>
        /// 复制类型实例。
        /// </summary>
        /// <param name="source">给定的源类型实例。</param>
        /// <param name="target">给定的目标类型实例。</param>
        void Copy(T source, T target);


        /// <summary>
        /// 计数查询。
        /// </summary>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则计算所有条数）。</param>
        /// <returns>返回整数。</returns>
        int Count(Expression<Func<T, bool>> predicate = null);


        /// <summary>
        /// 获取单条指定编号的类型实例。
        /// </summary>
        /// <param name="id">给定的主键。</param>
        /// <returns>返回类型实例。</returns>
        T Get(object id);

        /// <summary>
        /// 获取单条符合指定查询表达式的数据。
        /// </summary>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则根据唯一性要求查询第一条数据）。</param>
        /// <param name="isUnique">是否要求唯一性（如果为 True，则表示查询出多条数据将会抛出异常）。</param>
        /// <returns>返回对象。</returns>
        T Get(Expression<Func<T, bool>> predicate = null, bool isUnique = true);


        /// <summary>
        /// 获取多条符合指定查询表达式的数据集合。
        /// </summary>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="order">给定的排序方法（可选；如果为空，则采用默认排序）。</param>
        /// <returns>返回数组。</returns>
        T[] GetMany(Expression<Func<T, bool>> predicate = null, Action<Orderable<T>> order = null);

        
        /// <summary>
        /// 获取分页数据。
        /// </summary>
        /// <param name="createInfoFactory">给定创建分页信息的方法。</param>
        /// <param name="order">给定的排序方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <returns>返回 <see cref="IPagingable{T}"/>。</returns>
        IPagingable<T> GetPaging(Func<int, PagingInfo> createInfoFactory,
            Action<Orderable<T>> order, Expression<Func<T, bool>> predicate = null);


        /// <summary>
        /// 获取指定属性值集合。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="selector">给定的属性选择器。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则根据唯一性要求查询第一条数据）。</param>
        /// <param name="removeDuplicates">是否移除重复项（可选；默认移除重复项）。</param>
        /// <returns>返回数组。</returns>
        TProperty[] GetProperties<TProperty>(Expression<Func<T, TProperty>> selector,
            Expression<Func<T, bool>> predicate = null, bool removeDuplicates = true);
    }


    /// <summary>
    /// 仓库读取器静态扩展。
    /// </summary>
    public static class RepositoryReaderExtensions
    {
        /// <summary>
        /// 异步复制类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="source">给定的源类型实例。</param>
        /// <param name="target">给定的目标类型实例。</param>
        /// <returns>返回一个异步操作。</returns>
        public static Task CopyAsync<T>(this IRepositoryReader<T> repotitory, T source, T target)
        {
            return Task.Factory.StartNew(() => repotitory.Copy(source, target));
        }


        /// <summary>
        /// 异步计数查询。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则计算所有条数）。</param>
        /// <returns>返回整数。</returns>
        public static Task<int> CountAsync<T>(this IRepositoryReader<T> repotitory,
            Expression<Func<T, bool>> predicate = null)
        {
            return Task.Factory.StartNew(() => repotitory.Count(predicate));
        }


        /// <summary>
        /// 检测指定筛选条件的数据项是否存在。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="predicate">给定的筛选条件。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists<T>(this IRepositoryReader<T> repotitory, Expression<Func<T, bool>> predicate)
        {
            T item = default(T);

            return repotitory.Exists(predicate, out item);
        }
        /// <summary>
        /// 检测指定筛选条件的数据项是否存在。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="predicate">给定的筛选条件。</param>
        /// <param name="item">输出数据项。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists<T>(this IRepositoryReader<T> repotitory, Expression<Func<T, bool>> predicate,
            out T item)
        {
            item = repotitory.Get(predicate);

            return (!ReferenceEquals(item, null));
        }

        /// <summary>
        /// 异步检测指定筛选条件的数据项是否存在。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="predicate">给定的筛选条件。</param>
        /// <returns>返回布尔值。</returns>
        public static Task<bool> ExistsAsync<T>(this IRepositoryReader<T> repotitory,
            Expression<Func<T, bool>> predicate)
        {
            return Task<bool>.Factory.StartNew(() => repotitory.Exists(predicate));
        }


        /// <summary>
        /// 异步获取单条指定编号的类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="id">给定的编号。</param>
        /// <returns>返回一个带类型实例的异步操作。</returns>
        public static Task<T> GetAsync<T>(this IRepositoryReader<T> repotitory, object id)
        {
            return Task<T>.Factory.StartNew(() => repotitory.Get(id));
        }

        /// <summary>
        /// 异步获取单条符合指定查询表达式的数据。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则根据唯一性要求查询第一条数据）。</param>
        /// <param name="isUnique">是否要求唯一性（如果为 True，则表示查询出多条数据将会抛出异常）。</param>
        /// <returns>返回一个带类型实例的异步操作。</returns>
        public static Task<T> GetAsync<T>(this IRepositoryReader<T> repotitory,
            Expression<Func<T, bool>> predicate = null, bool isUnique = true)
        {
            return Task<T>.Factory.StartNew(() => repotitory.Get(predicate, isUnique));
        }


        /// <summary>
        /// 异步获取多条符合指定查询表达式的数据集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="order">给定的排序方法（可选；如果为空，则采用默认排序）。</param>
        /// <returns>返回一个带类型实例数组的异步操作。</returns>
        public static Task<T[]> GetManyAsync<T>(this IRepositoryReader<T> repotitory,
            Expression<Func<T, bool>> predicate = null, Action<Orderable<T>> order = null)
        {
            return Task<T[]>.Factory.StartNew(() => repotitory.GetMany(predicate, order));
        }


        /// <summary>
        /// 获取分页数据。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的取得条数。</param>
        /// <param name="order">给定的排序方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <returns>返回一个分页集合。</returns>
        public static IPagingable<T> GetPagingBySkip<T>(this IRepositoryReader<T> repotitory,
            int skip, int take, Action<Orderable<T>> order, Expression<Func<T, bool>> predicate = null)
        {
            return repotitory.GetPaging(total => PagingHelper.CreateInfoBySkip(skip, take, total),
                order, predicate);
        }
        /// <summary>
        /// 异步获取分页数据。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的取得条数。</param>
        /// <param name="order">给定的排序方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <returns>返回一个带分页集合的异步操作。</returns>
        public static Task<IPagingable<T>> GetPagingBySkipAsync<T>(this IRepositoryReader<T> repotitory,
            int skip, int take, Action<Orderable<T>> order, Expression<Func<T, bool>> predicate = null)
        {
            return Task<IPagingable<T>>.Factory.StartNew(() => repotitory.GetPagingBySkip(skip, take, order, predicate));
        }

        /// <summary>
        /// 获取分页数据。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <param name="order">给定的排序方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <returns>返回一个分页集合。</returns>
        public static IPagingable<T> GetPagingByIndex<T>(this IRepositoryReader<T> repotitory,
            int index, int size, Action<Orderable<T>> order, Expression<Func<T, bool>> predicate = null)
        {
            return repotitory.GetPaging(total => PagingHelper.CreateInfoByIndex(index, size, total),
                order, predicate);
        }
        /// <summary>
        /// 异步获取分页数据。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <param name="order">给定的排序方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <returns>返回一个带分页集合的异步操作。</returns>
        public static Task<IPagingable<T>> GetPagingByIndexAsync<T>(this IRepositoryReader<T> repotitory,
            int index, int size, Action<Orderable<T>> order, Expression<Func<T, bool>> predicate = null)
        {
            return Task<IPagingable<T>>.Factory.StartNew(() => repotitory.GetPagingByIndex(index, size, order, predicate));
        }


        /// <summary>
        /// 获取指定属性值集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="repotitory">给定的仓库读取器接口。</param>
        /// <param name="selector">给定的属性选择器。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则根据唯一性要求查询第一条数据）。</param>
        /// <param name="removeDuplicates">是否移除重复项（可选；默认移除重复项）。</param>
        /// <returns>返回一个带属性值数组的异步操作。</returns>
        public static Task<TProperty[]> GetPropertiesAsync<T, TProperty>(this IRepositoryReader<T> repotitory,
            Expression<Func<T, TProperty>> selector, Expression<Func<T, bool>> predicate = null,
            bool removeDuplicates = true)
        {
            return Task<TProperty[]>.Factory.StartNew(() => repotitory.GetProperties(selector, predicate, removeDuplicates));
        }

    }
}
