#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Localization;
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace Librame.Extensions.Core.Localizers
{
    /// <summary>
    /// 核心资源字典字符串定位器工厂。
    /// </summary>
    public class CoreResourceDictionaryStringLocalizerFactory : IDictionaryStringLocalizerFactory
    {
        private ConcurrentDictionary<string, DictionaryStringLocalizer> _localizers;


        /// <summary>
        /// 构造一个 <see cref="CoreResourceDictionaryStringLocalizerFactory"/>。
        /// </summary>
        public CoreResourceDictionaryStringLocalizerFactory()
        {
            _localizers = new ConcurrentDictionary<string, DictionaryStringLocalizer>();
        }


        /// <summary>
        /// 创建字符串定位器。
        /// </summary>
        /// <param name="resourceBaseType">给定的资源基础类型。</param>
        /// <returns>返回 <see cref="IStringLocalizer"/>。</returns>
        public IStringLocalizer Create(Type resourceBaseType)
        {
            var key = GenerateKey(resourceBaseType);

            return _localizers.GetOrAdd(key, k =>
            {
                var manager = new ResourceDictionaryManager(resourceBaseType);
                return new DictionaryStringLocalizer(manager);
            });
        }

        /// <summary>
        /// 创建字符串定位器。
        /// </summary>
        /// <param name="baseTypeName">给定的资源基础名。</param>
        /// <param name="assemblyLocation">给定的资源程序集定位。</param>
        /// <returns>返回 <see cref="IStringLocalizer"/>。</returns>
        public IStringLocalizer Create(string baseTypeName, string assemblyLocation)
        {
            var key = GenerateKey(baseTypeName, assemblyLocation);

            return _localizers.GetOrAdd(key, k =>
            {
                var baseType = Type.GetType(baseTypeName);

                var assemblyName = new AssemblyName(assemblyLocation);
                var assembly = Assembly.Load(assemblyName);

                var manager = new ResourceDictionaryManager(baseType, assembly);
                return new DictionaryStringLocalizer(manager);
            });
        }


        /// <summary>
        /// 生成键名。
        /// </summary>
        /// <param name="resourceBaseType">给定的资源基础类型。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "resourceBaseType")]
        public static string GenerateKey(Type resourceBaseType)
        {
            resourceBaseType.NotNull(nameof(resourceBaseType));
            return GenerateKey(resourceBaseType.FullName, resourceBaseType.Assembly.Location);
        }

        /// <summary>
        /// 生成键名。
        /// </summary>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="location">给定的定位。</param>
        /// <returns>返回字符串。</returns>
        public static string GenerateKey(string baseName, string location)
            => $"B={baseName},L={location},C={CultureInfo.CurrentUICulture.Name}";
    }
}
