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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 程序集实用工具。
    /// </summary>
    public static class AssemblyUtility
    {
        private static readonly string[] _withoutSystemAssemblyPrefixes
            = new string[] { "microsoft", "mscorlib", "netstandard", "sos", "system", "window" };


        static AssemblyUtility()
        {
            CurrentDomainAssemblies = CurrentDomainAssemblies.EnsureSingleton(() =>
            {
                return AppDomain.CurrentDomain.GetAssemblies();
            });

            CurrentDomainAssembliesWithoutSystem = CurrentDomainAssembliesWithoutSystem.EnsureSingleton(() =>
            {
                var assemblies = CurrentDomainAssemblies.AsEnumerable();

                _withoutSystemAssemblyPrefixes.ForEach(prefix =>
                {
                    assemblies = assemblies.Where(assembly => !assembly.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
                });

                return assemblies.ToArray();
            });
        }


        /// <summary>
        /// 当前线程应用域的程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentDomainAssemblies { get; }

        /// <summary>
        /// 当前线程应用域除系统外的第三方程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentDomainAssembliesWithoutSystem { get; }
    }
}
