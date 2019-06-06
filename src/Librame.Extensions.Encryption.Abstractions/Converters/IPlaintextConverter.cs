﻿#region License

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
    using Core;

    /// <summary>
    /// 明文转换器接口。
    /// </summary>
    public interface IPlaintextConverter : IAlgorithmConverter<string>, IEncodingConverter
    {
    }
}