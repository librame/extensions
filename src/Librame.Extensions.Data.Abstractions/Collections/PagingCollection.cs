#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Data.Collections
{
    /// <summary>
    /// 分页集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class PagingCollection<T> : IPageable<T>
    {
        private readonly ICollection<T> _rows;


        /// <summary>
        /// 构造一个 <see cref="PagingCollection{T}"/>。
        /// </summary>
        /// <param name="rows">给定的行集合。</param>
        /// <param name="descriptor">给定的描述符。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public PagingCollection(ICollection<T> rows, PagingDescriptor descriptor)
        {
            rows.NotNull(nameof(rows));
            descriptor.NotNull(nameof(descriptor));

            // 如果未进行分页
            if (rows.Count > descriptor.Size)
            {
                // 手动内存分页
                _rows = rows
                    .Skip(descriptor.Skip)
                    .Take(descriptor.Size)
                    .ToList();
            }
            else
            {
                _rows = rows;
            }

            Descriptor = descriptor;
        }

        private PagingCollection()
        {
            _rows = Array.Empty<T>();
            Descriptor = new PagingDescriptor(0);
        }

        
        /// <summary>
        /// 描述符。
        /// </summary>
        public PagingDescriptor Descriptor { get; }

        /// <summary>
        /// 总条数。
        /// </summary>
        public int Total => Descriptor.Total;

        /// <summary>
        /// 总页数。
        /// </summary>
        public int Pages => Descriptor.Pages;

        /// <summary>
        /// 跳过的条数。
        /// </summary>
        public int Skip => Descriptor.Skip;

        /// <summary>
        /// 页大小或得到的条数。
        /// </summary>
        public int Size => Descriptor.Size;

        /// <summary>
        /// 页索引。
        /// </summary>
        public int Index => Descriptor.Index;


        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>返回枚举器。</returns>
        public IEnumerator<T> GetEnumerator()
            => _rows.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();


        /// <summary>
        /// 空实例。
        /// </summary>
        public readonly static IPageable<T> Empty
            = new PagingCollection<T>();

    }
}
