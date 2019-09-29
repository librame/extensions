﻿#region License

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
    /// <summary>
    /// 分表特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class,
        AllowMultiple = false, Inherited = false)]
    public class ShardingTableAttribute : Attribute
    {
    }
}