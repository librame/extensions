#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 存储进度描述符。
    /// </summary>
    public class StorageProgressDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="StorageProgressDescriptor"/>。
        /// </summary>
        /// <param name="readLength">给定的读取长度。</param>
        /// <param name="readPosition">给定的读取位置。</param>
        /// <param name="writeLength">给定的写入长度。</param>
        /// <param name="writePosition">给定的写入位置。</param>
        /// <param name="currentCount">给定的当前读写数。</param>
        public StorageProgressDescriptor(long readLength, long readPosition,
            long writeLength, long writePosition, int currentCount)
        {
            ReadLength = readLength;
            ReadPosition = readPosition;

            WriteLength = writeLength;
            WritePosition = writePosition;

            CurrentCount = currentCount;
        }


        /// <summary>
        /// 读取长度。
        /// </summary>
        public long ReadLength { get; }

        /// <summary>
        /// 读取位置。
        /// </summary>
        public long ReadPosition { get; }


        /// <summary>
        /// 写入长度。
        /// </summary>
        public long WriteLength { get; }

        /// <summary>
        /// 写入位置。
        /// </summary>
        public long WritePosition { get; }


        /// <summary>
        /// 当前读写数。
        /// </summary>
        public int CurrentCount { get; }
    }
}
