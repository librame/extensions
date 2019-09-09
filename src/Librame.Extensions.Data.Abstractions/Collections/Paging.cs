#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 分页集合。
    /// </summary>
    /// <typeparam name="T">指定的分页类型。</typeparam>
    public class Paging<T> : IPageable<T>
    {
        private readonly IList<T> _rows;


        /// <summary>
        /// 构造一个 <see cref="Paging{T}"/>。
        /// </summary>
        /// <param name="rows">给定的行集合。</param>
        /// <param name="descriptor">给定的描述符。</param>
        public Paging(IList<T> rows, PagingDescriptor descriptor)
        {
            rows.NotNull(nameof(rows));

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

    }
}
