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

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Array"/> 实用工具。
    /// </summary>
    public class ArrayUtility
    {
        /// <summary>
        /// 将源数组转换为结果数组。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的目标类型。</typeparam>
        /// <param name="sources">给定的源数组。</param>
        /// <returns>返回结果数组。</returns>
        public static TResult[] As<TSource, TResult>(TSource[] sources)
            where TResult : class
        {
            if (ReferenceEquals(sources, null)) return null;

            var list = new List<TResult>();

            if (sources.Length > 0)
            {
                for (var i = 0; i < sources.Length; i++)
                {
                    var m = sources[i];

                    if (m is TResult)
                        list.Add(m as TResult);
                    else
                        throw new ArgumentException(string.Format("{0} must be from {1}",
                            typeof(TSource).FullName, typeof(TResult).FullName));
                }
            }

            return list.ToArray();
        }


        #region Sort

        /// <summary>
        /// 冒泡排序。
        /// </summary>
        /// <param name="array">给定的整数数组。</param>
        public static void BubbleSort(int[] array)
        {
            int i, j, temp;
            bool done = false;
            j = 1;

            while ((j < array.Length) && (!done))
            {
                done = true;
                for (i = 0; i < array.Length - j; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        done = false;
                        temp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = temp;
                    }
                }

                j++;
            }
        }

        /// <summary>
        /// 选择排序。
        /// </summary>
        /// <param name="array">给定的整数数组。</param>
        public static void SelectionSort(int[] array)
        {
            int min = 0;
            int pointer = min;

            for (int i = 0; i < array.Length - 1; i++)
            {
                pointer = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[pointer])
                        pointer = j;
                }

                int t = array[pointer];
                array[pointer] = array[i];
                array[i] = t;
            }
        }

        #endregion

    }


    /// <summary>
    /// <see cref="ArrayUtility"/> 静态扩展。
    /// </summary>
    public static class ArrayUtilityExtensions
    {
        /// <summary>
        /// 冒泡排序。
        /// </summary>
        /// <param name="array">给定的整数数组。</param>
        public static void BubbleSort(this int[] array)
        {
            ArrayUtility.BubbleSort(array);
        }

        /// <summary>
        /// 选择排序。
        /// </summary>
        /// <param name="array">给定的整数数组。</param>
        public static void SelectionSort(this int[] array)
        {
            ArrayUtility.SelectionSort(array);
        }

    }
}
