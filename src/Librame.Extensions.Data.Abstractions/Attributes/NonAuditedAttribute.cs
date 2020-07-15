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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 非审计特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
    public sealed class NonAuditedAttribute : Attribute
    {
    }
}
