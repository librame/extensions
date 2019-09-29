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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Librame.Extensions
{
    /// <summary>
    /// 可枚举静态扩展。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 迭代为可枚举集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="instance">给定的类型实例。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<T> YieldEnumerable<T>(this T instance)
        {
            yield return instance;
        }

        /// <summary>
        /// 迭代为可枚举集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="task">给定的异步操作。</param>
        /// <returns>返回 <see cref="IAsyncEnumerable{T}"/>。</returns>
        public static async IAsyncEnumerable<T> YieldAsyncEnumerable<T>(this Task<T> task)
        {
            yield return await task;
        }

        /// <summary>
        /// 转换为异步可枚举集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的可枚举异步操作。</param>
        /// <returns>返回 <see cref="IAsyncEnumerable{T}"/>。</returns>
        public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<Task<T>> enumerable)
        {
            foreach (var task in enumerable)
                yield return await task;
        }


        /// <summary>
        /// 转换为只读列表集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{T}"/>。</returns>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> enumerable)
            => new ReadOnlyCollection<T>(enumerable?.ToList());

        /// <summary>
        /// 转换为只读列表集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="list">给定的 <see cref="IList{T}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{T}"/>。</returns>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> list)
            => new ReadOnlyCollection<T>(list);


        /// <summary>
        /// 新增列表中不包含的值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="list">给定的 <see cref="IList{T}"/>。</param>
        /// <param name="value">给定的新增的值。</param>
        public static void AddIfNotContains<T>(this IList<T> list, T value)
        {
            list.NotNull(nameof(list));

            if (list.Contains(value))
                return;
            
            list.Add(value);
        }


        /// <summary>
        /// 计算可枚举集合的哈希码。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <returns>返回整数。</returns>
        public static int ComputeHashCode<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.IsEmpty()) return -1;
            return enumerable.Aggregate(0, (x, y) => x.GetHashCode() ^ y.GetHashCode());
        }


        #region ForEach

        /// <summary>
        /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// action is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="action">给定的遍历动作。</param>
        /// <param name="breakFactory">给定跳出遍历的动作（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> items, Action<T> action,
            Func<T, bool> breakFactory = null)
        {
            if (items.IsNull()) return;

            action.NotNull(nameof(action));

            await foreach (var item in items)
            {
                action.Invoke(item);

                if (breakFactory?.Invoke(item) == true)
                    break;
            }
        }

        /// <summary>
        /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// action is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="action">给定的遍历动作。</param>
        /// <param name="breakFactory">给定跳出遍历的动作（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> items, Action<T, int> action,
            Func<T, int, bool> breakFactory = null)
        {
            if (items.IsNull()) return;

            action.NotNull(nameof(action));

            int i = 0;
            await foreach (var item in items)
            {
                action.Invoke(item, i);

                if (breakFactory?.Invoke(item, i) == true)
                    break;

                i++;
            }
        }


        /// <summary>
        /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// action is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="action">给定的遍历动作。</param>
        /// <param name="breakFactory">给定跳出遍历的动作（可选）。</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action,
            Func<T, bool> breakFactory = null)
        {
            if (items.IsNull()) return;

            action.NotNull(nameof(action));

            foreach (var item in items)
            {
                action.Invoke(item);

                if (breakFactory?.Invoke(item) == true)
                    break;
            }
        }

        /// <summary>
        /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// action is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="action">给定的遍历动作。</param>
        /// <param name="breakFactory">给定跳出遍历的动作（可选）。</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action,
            Func<T, int, bool> breakFactory = null)
        {
            if (items.IsNull()) return;

            action.NotNull(nameof(action));

            int i = 0;
            foreach (var item in items)
            {
                action.Invoke(item, i);

                if (breakFactory?.Invoke(item, i) == true)
                    break;

                i++;
            }
        }

        #endregion


        #region Trim

        /// <summary>
        /// 修剪可枚举集合的指定初始与末尾项。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="trim">要修剪的实例。</param>
        /// <param name="isLoop">是否循环修剪末尾项（可选；默认循环修剪）。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<T> Trim<T>(this IEnumerable<T> enumerable, T trim, bool isLoop = true)
            => enumerable.Trim(item => item.Equals(trim), isLoop);

        /// <summary>
        /// 修剪可枚举集合的指定初始与末尾项。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="predicateFactory">给定用于修剪的断定工厂方法。</param>
        /// <param name="isLoop">是否循环修剪末尾项（可选；默认循环修剪）。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<T> Trim<T>(this IEnumerable<T> enumerable, Func<T, bool> predicateFactory, bool isLoop = true)
            => enumerable.TrimStart(predicateFactory, isLoop).TrimEnd(predicateFactory, isLoop);


        /// <summary>
        /// 修剪可枚举集合的指定末尾项。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="endItem">给定要修剪的末尾项。</param>
        /// <param name="isLoop">是否循环修剪末尾项（可选；默认循环修剪）。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<T> TrimEnd<T>(this IEnumerable<T> enumerable, T endItem, bool isLoop = true)
            => enumerable.TrimEnd(item => item.Equals(endItem), isLoop);

        /// <summary>
        /// 修剪可枚举集合的指定末尾项。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="endFactory">给定要修剪末尾项的断定工厂方法。</param>
        /// <param name="isLoop">是否循环修剪末尾项（可选；默认循环修剪）。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<T> TrimEnd<T>(this IEnumerable<T> enumerable, Func<T, bool> endFactory, bool isLoop = true)
        {
            endFactory.NotNull(nameof(endFactory));
            return enumerable.TrimLast(endFactory, isLoop);
        }


        /// <summary>
        /// 修剪可枚举集合的指定初始项。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="startItem">给定要修剪的初始项。</param>
        /// <param name="isLoop">是否循环修剪末尾项（可选；默认循环修剪）。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<T> TrimStart<T>(this IEnumerable<T> enumerable, T startItem, bool isLoop = true)
            => enumerable.TrimStart(item => item.Equals(startItem), isLoop);

        /// <summary>
        /// 修剪可枚举集合的指定初始项。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="startFactory">给定要修剪初始项的断定工厂方法。</param>
        /// <param name="isLoop">是否循环修剪末尾项（可选；默认循环修剪）。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<T> TrimStart<T>(this IEnumerable<T> enumerable, Func<T, bool> startFactory, bool isLoop = true)
        {
            startFactory.NotNull(nameof(startFactory));
            return enumerable.Reverse().TrimLast(startFactory, isLoop).Reverse();
        }


        private static IEnumerable<T> TrimLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicateFactory, bool isLoop = true)
        {
            if (enumerable.IsEmpty())
                return enumerable;

            if (predicateFactory.Invoke(enumerable.Last()))
            {
                enumerable = enumerable.Take(enumerable.Count() - 1);

                if (isLoop) // 循环修剪
                    return enumerable.TrimLast(predicateFactory, isLoop);

                return enumerable;
            }

            return enumerable;
        }

        #endregion

    }
}
