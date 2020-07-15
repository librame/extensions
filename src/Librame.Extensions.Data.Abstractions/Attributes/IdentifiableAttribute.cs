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
    /// 可标识特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class IdentifiableAttribute : Attribute
    {
    }
}
