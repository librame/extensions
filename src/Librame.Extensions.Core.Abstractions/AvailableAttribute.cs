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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 可用的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class AvailableAttribute : Attribute
    {
        /// <summary>
        /// 构造一个 <see cref="AvailableAttribute"/>。
        /// </summary>
        /// <param name="enabled">是否启用此特性。</param>
        public AvailableAttribute(bool enabled)
            : base()
        {
            Enabled = enabled;
        }


        /// <summary>
        /// 启用特性。
        /// </summary>
        public bool Enabled { get; set; }
    }
}
