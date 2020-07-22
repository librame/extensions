#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 抽象评论用户计数。
    /// </summary>
    public abstract class AbstractCommentUserCount : AbstractCommentUserCount<long>
    {
        /// <summary>
        /// 累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数。</param>
        /// <returns>返回长整数。</returns>
        public override long DegressiveCount(long count)
            => --count;

        /// <summary>
        /// 异步累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含长整数的异步操作。</returns>
        public override ValueTask<long> DegressiveCountAsync(long count,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelValueAsync(() => --count);


        /// <summary>
        /// 累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数。</param>
        /// <returns>返回长整数。</returns>
        public override long ProgressiveCount(long count)
            => ++count;

        /// <summary>
        /// 异步累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含长整数的异步操作。</returns>
        public override ValueTask<long> ProgressiveCountAsync(long count,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelValueAsync(() => ++count);
    }


    /// <summary>
    /// 抽象评论用户计数。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public abstract class AbstractCommentUserCount<TValue> : AbstractUserCount<TValue>, ICommentUserCount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 评论条数。
        /// </summary>
        [Display(Name = nameof(CommentCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue CommentCount { get; set; }

        /// <summary>
        /// 评论人数。
        /// </summary>
        [Display(Name = nameof(CommenterCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue CommenterCount { get; set; }


        #region Degressive

        /// <summary>
        /// 累减评论条数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue DegressiveCommentCount()
            => CommentCount = DegressiveCount(CommentCount);

        /// <summary>
        /// 异步累减评论条数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> DegressiveCommentCountAsync(CancellationToken cancellationToken = default)
            => CommentCount = await DegressiveCountAsync(CommentCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累减评论人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue DegressiveCommenterCount()
            => CommenterCount = DegressiveCount(CommenterCount);

        /// <summary>
        /// 异步累减评论人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> DegressiveCommenterCountAsync(CancellationToken cancellationToken = default)
            => CommenterCount = await DegressiveCountAsync(CommenterCount, cancellationToken).ConfigureAwait();

        #endregion


        #region Progressive

        /// <summary>
        /// 累加评论条数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ProgressiveCommentCount()
            => CommentCount = ProgressiveCount(CommentCount);

        /// <summary>
        /// 异步累加评论条数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveCommentCountAsync(CancellationToken cancellationToken = default)
            => CommentCount = await ProgressiveCountAsync(CommentCount, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 累加评论人数。
        /// </summary>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ProgressiveCommenterCount()
            => CommenterCount = ProgressiveCount(CommenterCount);

        /// <summary>
        /// 异步累加评论人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveCommenterCountAsync(CancellationToken cancellationToken = default)
            => CommenterCount = await ProgressiveCountAsync(CommenterCount, cancellationToken).ConfigureAwait();

        #endregion

    }
}
