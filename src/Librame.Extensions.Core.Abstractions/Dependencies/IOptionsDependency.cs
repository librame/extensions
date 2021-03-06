﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Dependencies
{
    using Serializers;

    /// <summary>
    /// 选项依赖接口。
    /// </summary>
    public interface IOptionsDependency : IDependency
    {
        /// <summary>
        /// 选项类型。
        /// </summary>
        SerializableString<Type> OptionsType { get; }
    }
}
