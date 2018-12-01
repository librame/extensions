#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Builders
{
    /// <summary>
    /// 存储构建器选项。
    /// </summary>
    public class StorageBuilderOptions : IBuilderOptions
    {
        /// <summary>
        /// 缓冲区大小（默认使用 512）。
        /// </summary>
        public int BufferSize { get; set; } = 512;
    }
}
