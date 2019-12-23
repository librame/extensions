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

namespace Librame.Extensions.Data.Collections
{
    /// <summary>
    /// 分页描述符。
    /// </summary>
    public class PagingDescriptor : IPagingInfo
    {
        /// <summary>
        /// 构造一个 <see cref="PagingDescriptor"/>。
        /// </summary>
        /// <param name="total">给定的总条数。</param>
        public PagingDescriptor(int total)
        {
            Total = total;
        }


        /// <summary>
        /// 总条数。
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// 总页数。
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// 跳过的条数。
        /// </summary>
        public int Skip { get; private set; }

        /// <summary>
        /// 页大小或得到的条数。
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// 页索引。
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 是否通过跳数计算。
        /// </summary>
        public bool IsComputedBySkip { get; private set; }


        /// <summary>
        /// 通过索引计算。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="PagingDescriptor"/>。</returns>
        public PagingDescriptor ComputeByIndex(int index, int size)
        {
            if (index < 1) index = 1;
            if (size < 1) size = 1;

            Size = size;
            Index = index;

            // 计算跳过的条数
            if (Index > 1)
            {
                Skip = (Index - 1) * Size;
            }
            else
            {
                // 当前页索引小于等于1表示不跳过
                Skip = 0;
            }

            if (Total > 0)
            {
                // 计算总索引数
                Pages = Total / Size + (Total % Size > 0 ? 1 : 0);
            }

            IsComputedBySkip = false;

            return this;
        }

        /// <summary>
        /// 通过跳数计算。
        /// </summary>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="PagingDescriptor"/>。</returns>
        public PagingDescriptor ComputeBySkip(int skip, int take)
        {
            if (take < 1) take = 1;

            Index = ((int)Math.Round((double)skip / take)) + 1;
            Skip = skip;
            Size = take;

            if (Total > 0)
            {
                // 计算总页数
                Pages = Total / take + (Total % take > 0 ? 1 : 0);
            }

            IsComputedBySkip = true;

            return this;
        }

    }
}
