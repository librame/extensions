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
    using Converters;

    /// <summary>
    /// 明文算法转换器接口。
    /// </summary>
    public interface IPlaintextAlgorithmConverter : IAlgorithmConverter<string>, IEncodingConverter
    {
    }
}
