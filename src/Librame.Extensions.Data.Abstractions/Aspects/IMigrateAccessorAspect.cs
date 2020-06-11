#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Aspects
{
    /// <summary>
    /// 迁移访问器截面接口（通常用于后置保存变化操作）。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public interface IMigrateAccessorAspect<TGenId, TCreatedBy> : IAccessorAspect<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 需要保存更改。
        /// </summary>
        bool RequiredSaveChanges { get; set; }
    }
}
