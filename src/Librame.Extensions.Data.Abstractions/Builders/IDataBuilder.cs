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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 数据构建器接口。
    /// </summary>
    public interface IDataBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 访问器类型。
        /// </summary>
        Type AccessorType { get; }
    }
}
