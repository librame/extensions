#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using System;

namespace Microsoft.Extensions.Localization
{
    /// <summary>
    /// 因原 RootNamespaceAttribute 不在 Microsoft.Extensions.Localization.Abstraction 包，所以定义个抽象副本。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly,
        AllowMultiple = false, Inherited = false)]
    public class AbstractionRootNamespaceAttribute : Attribute
    {
        /// <summary>
        /// 构建一个 <see cref="AbstractionRootNamespaceAttribute"/>。
        /// </summary>
        /// <param name="rootNamespace">给定的程序集根命名空间。</param>
        public AbstractionRootNamespaceAttribute(string rootNamespace)
        {
            RootNamespace = rootNamespace.NotEmpty(nameof(rootNamespace));
        }

        /// <summary>
        /// 程序集根命名空间。
        /// </summary>
        public string RootNamespace { get; }
    }
}
