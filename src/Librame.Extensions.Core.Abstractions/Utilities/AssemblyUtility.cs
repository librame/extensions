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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core.Utilities
{
    /// <summary>
    /// <see cref="Assembly"/> 实用工具。
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
        public static IReadOnlyList<Assembly> CurrentAssemblies
            => InitializeAssemblies();

        /// <summary>
        /// 当前除系统外的第三方程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentAssembliesWithoutSystem
            => InitializeAssembliesWithoutSystem();


        /// <summary>
        /// 当前除系统外的第三方程序集包含的所有公共类型列表。
        /// </summary>
        public static IReadOnlyList<Type> CurrentExportedTypesWithoutSystem { get; }
            = CurrentAssembliesWithoutSystem.ExportedTypes();


        /// <summary>
        /// 创建当前除系统外的第三方程序集包含的所有公共类型的实例列表。
        /// </summary>
        /// <typeparam name="TExported">指定的公共类型。</typeparam>
        /// <param name="filterNonRegisteredAttribute">过滤已标记 <see cref="NonRegisteredAttribute"/> 的类型（可选；默认启用）。</param>
        /// <returns>返回 <see cref="IReadOnlyList{TExported}"/>。</returns>
        public static List<TExported> CurrentExportedInstancesWithoutSystem<TExported>
            (bool filterNonRegisteredAttribute = true)
        {
            var baseType = typeof(TExported);

            Func<Type, bool> predicate = null;
            if (filterNonRegisteredAttribute)
            {
                predicate = t => baseType.IsAssignableFromTargetType(t)
                    && t.IsConcreteType()
                    && !t.IsDefined<NonRegisteredAttribute>();
            }
            else
            {
                predicate = t => baseType.IsAssignableFromTargetType(t)
                    && t.IsConcreteType();
            }

            var exporteds = CurrentExportedTypesWithoutSystem
                .Where(predicate)
                .Select(t => t.EnsureCreate<TExported>())
                .ToList();

            if (baseType.IsImplementedInterface<ISortable>())
                exporteds.Sort();

            return exporteds;
        }


        private static Assembly[] InitializeAssemblies()
        {
            return ExtensionSettings.Current.RunLockerResult(() =>
            {
                return AppDomain.CurrentDomain.GetAssemblies();
            });
        }

        private static Assembly[] InitializeAssembliesWithoutSystem()
        {
            return ExtensionSettings.Current.RunLockerResult(() =>
            {
                return CurrentAssemblies.Where(NotSystemAssembly).ToArray();
            });

            bool NotSystemAssembly(Assembly assembly)
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
}
