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
using System.Text;

namespace Librame.Extensions.Core.Builders
{
    using Serializers;

    /// <summary>
    /// 核心构建器选项。
    /// </summary>
    public class CoreBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// 字符编码（默认为 UTF8）。
        /// </summary>
        public SerializableString<Encoding> Encoding { get; }
            = new SerializableString<Encoding>(System.Text.Encoding.UTF8);

        /// <summary>
        /// 解决时钟回流的偏移量（默认为 1）。
        /// </summary>
        public int ClockRefluxOffset { get; set; }
            = 1;

        /// <summary>
        /// 是 UTC 时钟。
        /// </summary>
        public bool IsUtcClock { get; set; }

        /// <summary>
        /// 线程数（默认为当前 CPU 线程数）。
        /// </summary>
        public int ThreadsCount { get; set; }
            = Environment.ProcessorCount;
    }
}
