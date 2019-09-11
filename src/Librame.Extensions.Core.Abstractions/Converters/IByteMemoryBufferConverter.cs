#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 字节型可读写的连续内存缓冲区至字符串转换器接口。
    /// </summary>
    public interface IByteMemoryBufferConverter : IByteMemoryBufferConverter<string>
    {
    }


    /// <summary>
    /// 字节型可读写的连续内存缓冲区转换器接口。
    /// </summary>
    /// <typeparam name="TTo">指定的转换类型。</typeparam>
    public interface IByteMemoryBufferConverter<TTo> : IConverter<IByteMemoryBuffer, TTo>
    {
    }
}
