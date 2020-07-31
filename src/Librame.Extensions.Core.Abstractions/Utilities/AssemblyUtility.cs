#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
        /// <summary>
        /// 当前程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentAssemblies
            => ExtensionSettings.Preference.CurrentAssemblies;

        /// <summary>
        /// 当前包含第三方服务的程序集列表。
        /// </summary>
        public static IReadOnlyList<Assembly> CurrentThirdPartyAssemblies
            => InitializeThirdPartyAssemblies();

        /// <summary>
        /// 当前第三方服务程序集集合所包含的所有公共类型列表。
        /// </summary>
        /// <remarks>使用</remarks>
        public static IReadOnlyList<Type> CurrentThirdPartyExportedTypes
            => InitializeThirdPartyExportedTypes();


        /// <summary>
        /// 创建当前第三方服务程序集集合所包含的所有公共类型实例列表。
        /// </summary>
        /// <typeparam name="TExported">指定的公共类型。</typeparam>
        /// <param name="filterNonRegistered">过滤已标记 <see cref="NonRegisteredAttribute"/> 的类型（可选；默认启用）。</param>
        /// <returns>返回 <see cref="IReadOnlyList{TExported}"/>。</returns>
        public static List<TExported> CreateCurrentThirdPartyExportedInstances<TExported>
            (bool filterNonRegistered = true)
        {
            return ExtensionSettings.Preference.RunLocker(() =>
            {
                var exportedType = typeof(TExported);

                Func<Type, bool> predicate = null;
                if (filterNonRegistered)
                {
                    predicate = t => exportedType.IsAssignableFromTargetType(t)
                        && t.IsConcreteType()
                        && !t.IsDefined<NonRegisteredAttribute>();
                }
                else
                {
                    predicate = t => exportedType.IsAssignableFromTargetType(t)
                        && t.IsConcreteType();
                }

                var exporteds = CurrentThirdPartyExportedTypes
                    .Where(predicate)
                    .Select(t => t.EnsureCreate<TExported>())
                    .ToList();

                if (exportedType.IsImplementedInterfaceType<ISortable>())
                    exporteds.Sort();

                return exporteds;
            });
        }


        private static IReadOnlyList<Type> InitializeThirdPartyExportedTypes()
        {
            return ExtensionSettings.Preference.RunLocker(() =>
            {
                return CurrentThirdPartyAssemblies.ExportedTypes();
            });
        }

        private static IReadOnlyList<Assembly> InitializeThirdPartyAssemblies()
        {
            // 使用包含第三方服务程序集前缀集合而不是除系统程序集前缀集合，
            // 否则新增包引用程序集可能会导致枚举类型时抛出找不到 FileNotFound 异常
            var thirdPartyAssemblyPrefixes = CoreSettings.Preference.ThirdPartyAssemblyPrefixes;

            return ExtensionSettings.Preference.RunLocker(() =>
            {
                return ExtensionSettings.Preference.CurrentAssemblies
                    .Where(ContainsThirdPartyAssembly).AsReadOnlyList();
            });

            // ContainsThirdPartyAssembly
            bool ContainsThirdPartyAssembly(Assembly assembly)
            {
                foreach (var prefix in thirdPartyAssemblyPrefixes)
                {
                    if (assembly.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        return true;
                }

                return false;
            }
        }

    }
}
