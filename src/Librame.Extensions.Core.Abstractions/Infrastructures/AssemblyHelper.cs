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
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 程序集助手。
    /// </summary>
    public static class AssemblyHelper
    {
        static AssemblyHelper()
        {
            CurrentDomainAssemblies = CurrentDomainAssemblies.EnsureSingleton(() =>
            {
                return AppDomain.CurrentDomain.GetAssemblies();
            });

            CurrentDomainAssembliesWithoutSystem = CurrentDomainAssembliesWithoutSystem.EnsureSingleton(() =>
            {
                return CurrentDomainAssemblies.SkipWhile(assembly =>
                {
                    var name = assembly.GetName().Name;
                    return !name.StartsWith("Microsoft.") || !name.StartsWith("System.");
                })
                .ToArray();
            });
        }


        /// <summary>
        /// 当前线程应用域的程序集数组。
        /// </summary>
        public static Assembly[] CurrentDomainAssemblies { get; }

        /// <summary>
        /// 当前线程应用域除系统外的第三方程序集数组。
        /// </summary>
        public static Assembly[] CurrentDomainAssembliesWithoutSystem { get; }
    }
}
