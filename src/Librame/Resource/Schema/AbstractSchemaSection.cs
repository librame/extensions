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
using System.Runtime.InteropServices;

namespace Librame.Resource.Schema
{
    /// <summary>
    /// 抽象结构部分。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public abstract class AbstractSchemaSection
    {
        /// <summary>
        /// 部分名称。
        /// </summary>
        protected abstract string SectionName { get; }
    }
}
