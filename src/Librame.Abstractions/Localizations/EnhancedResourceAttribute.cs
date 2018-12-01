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

namespace Microsoft.Extensions.Localization
{
    /// <summary>
    /// 增强型资源特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class EnhancedResourceAttribute : Attribute
    {
        /// <summary>
        /// 构造一个 <see cref="EnhancedResourceAttribute"/> 实例。
        /// </summary>
        /// <param name="enabled">启用增强模式（可选；默认启用）。</param>
        public EnhancedResourceAttribute(bool enabled = true)
            : base()
        {
            Enabled = enabled;
        }


        /// <summary>
        /// 启用增强模式。
        /// </summary>
        public bool Enabled { get; }
    }
}
