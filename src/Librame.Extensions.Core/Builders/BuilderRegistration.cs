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
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 构建器全局化。
    /// </summary>
    public static class BuilderGlobalization
    {
        static BuilderGlobalization()
        {
            Assemblies = Assemblies.EnsureSingleton(() => AppDomain.CurrentDomain.GetAssemblies());
        }


        /// <summary>
        /// 程序集数组。
        /// </summary>
        public static Assembly[] Assemblies { get; }


        /// <summary>
        /// 注册类型集合。
        /// </summary>
        /// <param name="action">给定的注册动作。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <param name="filterAssemblies">给定的程序集过滤工厂方法（可选；默认过滤系统程序集）。</param>
        public static void RegisterTypes(Action<Type> action,
            Func<IEnumerable<Type>, IEnumerable<Type>> filterTypes = null,
            Func<Assembly[], Assembly[]> filterAssemblies = null)
        {
            action.NotNull(nameof(action));

            // 默认过滤系统程序集
            filterAssemblies = filterAssemblies.EnsureSingleton(() =>
            {
                return _assemblies => _assemblies.SkipWhile(assembly =>
                {
                    var name = assembly.GetName().Name;
                    return !name.StartsWith("Microsoft.") || !name.StartsWith("System.");
                })
                .ToArray();
            });

            var types = filterAssemblies.Invoke(Assemblies).SelectMany(a => a.ExportedTypes);
            types = filterTypes?.Invoke(types);

            foreach (var type in types)
                action.Invoke(type);
        }


        /// <summary>
        /// 注册文化信息集合。
        /// </summary>
        /// <param name="cultureName">给定的文化名称。</param>
        /// <param name="cultureUIName">给定的 UI 文化名称（可选；默认为文化名称）。</param>
        public static void RegisterCultureInfos(string cultureName, string cultureUIName = null)
        {
            RegisterCultureInfos(new CultureInfo(cultureName),
                cultureUIName.IsNullOrEmpty() ? null : new CultureInfo(cultureUIName));
        }

        /// <summary>
        /// 注册文化信息集合。
        /// </summary>
        /// <param name="cultureInfo">给定的文化信息。</param>
        /// <param name="cultureUIInfo">给定的 UI 文化信息（可选；默认为文化信息）。</param>
        public static void RegisterCultureInfos(CultureInfo cultureInfo, CultureInfo cultureUIInfo = null)
        {
            CultureInfo.DefaultThreadCurrentCulture
                = CultureInfo.CurrentCulture
                = cultureInfo.NotNull(nameof(cultureInfo));

            CultureInfo.DefaultThreadCurrentUICulture
                = CultureInfo.CurrentUICulture
                = cultureUIInfo ?? cultureInfo;
        }

    }
}
