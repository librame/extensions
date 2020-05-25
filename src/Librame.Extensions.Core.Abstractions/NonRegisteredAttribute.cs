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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 未注册特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NonRegisteredAttribute : Attribute
    {
    }
}
