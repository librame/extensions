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
    using Builders;

    /// <summary>
    /// 存储构建器选项接口。
    /// </summary>
    public interface IStorageBuilderOptions : IBuilderOptions
    {
        /// <summary>
        /// 缓冲区大小（默认使用 512）。
        /// </summary>
        int BufferSize { get; set; }
    }
}
