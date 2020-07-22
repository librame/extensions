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

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// 标识生成器工厂接口。
    /// </summary>
    public interface IIdentityGeneratorFactory
    {
        /// <summary>
        /// 获取标识生成器。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <returns>返回 <see cref="IIdentityGenerator{TId}"/>。</returns>
        IIdentityGenerator<TId> GetGenerator<TId>();

        /// <summary>
        /// 获取对象标识生成器。
        /// </summary>
        /// <param name="idType">给定的标识类型。</param>
        /// <returns>返回 <see cref="IObjectIdentityGenerator"/>。</returns>
        IObjectIdentityGenerator GetGenerator(Type idType);
    }
}
