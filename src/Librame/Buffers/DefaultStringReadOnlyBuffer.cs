#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;

namespace Librame.Buffers
{
    using Extensions;

    /// <summary>
    /// 默认字符串只读缓冲区。
    /// </summary>
    public class DefaultStringReadOnlyBuffer : DefaultReadOnlyBuffer<char>, IStringReadOnlyBuffer
    {
        /// <summary>
        /// 记录器。
        /// </summary>
        protected ILogger<DefaultStringReadOnlyBuffer> Logger;


        /// <summary>
        /// 构造一个 <see cref="DefaultStringReadOnlyBuffer"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or white space.
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalReadOnlyBufferString}"/>（可选）。</param>
        public DefaultStringReadOnlyBuffer(string str, ILogger<DefaultStringReadOnlyBuffer> logger = null)
            : this(str.ToCharArray(), str, logger)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="DefaultStringReadOnlyBuffer"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or white space.
        /// </exception>
        /// <param name="memory">给定的只读存储器。</param>
        /// <param name="str">给定的字符串。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalReadOnlyBufferString}"/>（可选）。</param>
        internal DefaultStringReadOnlyBuffer(ReadOnlyMemory<char> memory, string str, ILogger<DefaultStringReadOnlyBuffer> logger = null)
        {
            RawString = str.NotEmpty(nameof(str));
            Logger = logger;

            Memory = memory;
            TryLogDebug($"Create string readonly buffer: {str}");
        }


        /// <summary>
        /// 原始字符串。
        /// </summary>
        public string RawString { get; }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IStringReadOnlyBuffer"/>。</returns>
        public virtual new IStringReadOnlyBuffer Copy()
        {
            return new DefaultStringReadOnlyBuffer(RawString, Logger);
        }


        /// <summary>
        /// 尝试记录调试消息。
        /// </summary>
        /// <param name="message">给定的消息。</param>
        /// <param name="args">给定的参数集合。</param>
        /// <returns>返回是否记录的布尔值。</returns>
        protected virtual bool TryLogDebug(string message, params object[] args)
        {
            if (Logger.IsNotDefault())
            {
                Logger.LogDebug(message, args);
                return true;
            }

            return false;
        }

    }
}
