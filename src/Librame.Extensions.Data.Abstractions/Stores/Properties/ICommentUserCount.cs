#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 评论用户计数接口。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public interface ICommentUserCount<TValue> : IUserCount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 评论次数。
        /// </summary>
        TValue CommentCount { get; set; }

        /// <summary>
        /// 评论人数。
        /// </summary>
        TValue CommenterCount { get; set; }


        /// <summary>
        /// 异步累加评论次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveCommentCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步累加评论人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveCommenterCountAsync(CancellationToken cancellationToken = default);
    }
}
