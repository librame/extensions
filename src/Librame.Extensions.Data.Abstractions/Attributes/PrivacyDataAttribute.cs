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
    /// 用于表示隐私数据特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class PrivacyDataAttribute : Attribute
    {
    }
}
