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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Librame.Extensions.Core.Utilities
{
    /// <summary>
    /// 程序集实用工具。
    /// </summary>
    public static class AssemblyUtility
    {
        private static readonly string[] _systemAssemblyPrefixes
            = new string[]
            {
                "anonymously",
                "microsoft",
                "mscorlib",
                "netstandard",
                "nuget",
                "runtime",
                "sos",
                "system",
                "testhost",
                "window"
            };


        /// <summary>
        /// 当前程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentAssemblies { get; }
            = LoadContextAssemblies();

        /// <summary>
        /// 当前除系统外的第三方程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentAssembliesWithoutSystem { get; }
            = CurrentAssemblies.Where(NotSystemAssembly).AsReadOnlyList();


        private static bool NotSystemAssembly(Assembly assembly)
        {
            foreach (var prefix in _systemAssemblyPrefixes)
            {
                if (assembly.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static IReadOnlyList<Assembly> LoadContextAssemblies()
        {
            #if !NET48
                var context = AssemblyLoadContext.Default;

                var assemblies = (IEnumerable<Assembly>)context.GetType()
                    .GetProperty("Assemblies")?.GetValue(context);

                if (assemblies.IsNull())
                    throw new InvalidOperationException("Invalid load assemblies.");

                return assemblies.AsReadOnlyList();
            #else
                return AppDomain.CurrentDomain.GetAssemblies();
            #endif
        }
    }
}
