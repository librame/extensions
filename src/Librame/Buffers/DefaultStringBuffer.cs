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
    /// 默认字符串缓冲区。
    /// </summary>
    public class DefaultStringBuffer : DefaultBuffer<char>, IStringBuffer
    {
        /// <summary>
        /// 记录器。
        /// </summary>
        protected ILogger<DefaultStringBuffer> Logger;


        /// <summary>
        /// 构造一个 <see cref="DefaultStringBuffer"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or white space.
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalBufferString}"/>（可选）。</param>
        public DefaultStringBuffer(string str, ILogger<DefaultStringBuffer> logger = null)
            : this(str.ToCharArray(), str, logger)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="DefaultStringBuffer"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or white space.
        /// </exception>
        /// <param name="memory">给定的字符存储器。</param>
        /// <param name="str">给定的字符串。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalBufferString}"/>（可选）。</param>
        internal DefaultStringBuffer(Memory<char> memory, string str, ILogger<DefaultStringBuffer> logger = null)
        {
            RawString = str.NotWhiteSpace(nameof(str));
            Logger = logger;

            ChangeMemory(memory);
            TryLogDebug($"Create string buffer: {str}");
        }


        /// <summary>
        /// 原始字符串。
        /// </summary>
        public string RawString { get; }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IStringBuffer"/>。</returns>
        public virtual new IStringBuffer Copy()
        {
            return new DefaultStringBuffer(Memory, RawString, Logger);
        }

        /// <summary>
        /// 创建只读副本。
        /// </summary>
        /// <returns>返回 <see cref="IStringBuffer"/>。</returns>
        IStringReadOnlyBuffer IStringReadOnlyBuffer.Copy()
        {
            return Copy();
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
