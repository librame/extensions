#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption
{
    using Buffers;
    using Converters;

    /// <summary>
    /// 算法转换器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public interface IAlgorithmConverter<TSource> : IConverter<TSource, IBuffer<byte>>
    {
    }
}
