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
                "newtonsoft",
                "netstandard",
                "nuget",
                "proxybuilder",
                "runtime",
                "sos",
                "system",
                "testhost",
                "window",
                "xunit"
            };


        /// <summary>
        /// 当前程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentAssemblies { get; }
            = AppDomain.CurrentDomain.GetAssemblies();

        /// <summary>
        /// 当前除系统外的第三方程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentAssembliesWithoutSystem { get; }
            = CurrentAssemblies.Where(NotSystemAssembly).AsReadOnlyList();


        /// <summary>
        /// 当前除系统外的第三方程序集包含的所有公共类型列表。
        /// </summary>
        public static IReadOnlyList<Type> CurrentExportedTypesWithoutSystem { get; }
            = CurrentAssembliesWithoutSystem.ExportedTypes();


        /// <summary>
        /// 创建当前除系统外的第三方程序集包含的所有公共类型的实例列表。
        /// </summary>
        /// <typeparam name="TExported">指定的公共类型。</typeparam>
        /// <returns>返回 <see cref="IReadOnlyList{TExported}"/>。</returns>
        public static IReadOnlyList<TExported> CreateInstancesByCurrentExportedTypesWithoutSystem<TExported>()
        {
            var baseType = typeof(TExported);

            var exporteds = CurrentExportedTypesWithoutSystem
                .Where(t => baseType.IsAssignableFromTargetType(t) && t.IsConcreteType())
                .Select(t => t.EnsureCreate<TExported>())
                .ToList();

            if (baseType.IsImplementedInterface<ISortable>())
                exporteds.Sort();

            return exporteds;
        }


        private static bool NotSystemAssembly(Assembly assembly)
        {
            foreach (var prefix in _systemAssemblyPrefixes)
            {
                if (assembly.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

    }
}
