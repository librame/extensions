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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Librame.Extensions.Core
{
    using Extensions;

    /// <summary>
    /// 表达式字符串定位器工厂。
    /// </summary>
    public class ExpressionStringLocalizerFactory : ResourceManagerStringLocalizerFactory, IStringLocalizerFactory
    {
        /// <summary>
        /// 构造一个 <see cref="ExpressionStringLocalizerFactory"/> 实例。
        /// </summary>
        /// <param name="localizationOptions">给定的 <see cref="IOptions{LocalizationOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public ExpressionStringLocalizerFactory(IOptions<LocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
            : base(localizationOptions, loggerFactory)
        {
            LoggerFactory = loggerFactory;
        }


        /// <summary>
        /// 日志工厂。
        /// </summary>
        protected ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger => LoggerFactory.CreateLogger<ExpressionStringLocalizerFactory>();


        /// <summary>
        /// 获取资源前缀。
        /// </summary>
        /// <param name="typeInfo">给定的 <see cref="TypeInfo"/>。</param>
        /// <param name="baseNamespace">给定的基础命名空间。</param>
        /// <param name="resourcesRelativePath">给定的资源相对路径。</param>
        /// <returns>返回字符串。</returns>
        protected override string GetResourcePrefix(TypeInfo typeInfo, string baseNamespace, string resourcesRelativePath)
        {
            var prefix = string.Empty;

            var resourceMapping = GetResourceMappingAttribute(typeInfo);
            if (resourceMapping.IsNotNull() && resourceMapping.Enabled)
            {
                if (resourceMapping.PrefixFactory.IsNull())
                {
                    resourceMapping.PrefixFactory = (_baseNamespace, _resourcesRelativePath, _typeInfo) =>
                    {
                        if (resourcesRelativePath.IsNullOrEmpty())
                            return $"{_baseNamespace}.{_typeInfo.Name}";

                        // _resourcesRelativePath 已格式化为点分隔符（如：Resources.）
                        return $"{_baseNamespace}.{_resourcesRelativePath}{_typeInfo.Name}";
                    };
                }

                prefix = resourceMapping.PrefixFactory.Invoke(baseNamespace, resourcesRelativePath, typeInfo);
                Logger.LogInformation($"Get resource prefix “{prefix}” by {nameof(ResourceMappingAttribute)}.PrefixFactory");
            }
            else
            {
                prefix = base.GetResourcePrefix(typeInfo, baseNamespace, resourcesRelativePath);
                Logger.LogInformation($"Get resource prefix “{prefix}” by {nameof(ResourceManagerStringLocalizerFactory)}");
            }

            return prefix;
        }


        /// <summary>
        /// 获取资源映射特性。
        /// </summary>
        /// <param name="typeInfo">给定的 <see cref="TypeInfo"/>。</param>
        /// <returns>返回 <see cref="ResourceMappingAttribute"/>。</returns>
        protected virtual ResourceMappingAttribute GetResourceMappingAttribute(TypeInfo typeInfo)
        {
            if (typeInfo.TryGetCustomAttribute(out ResourceMappingAttribute attribute))
                return attribute;

            if (typeInfo.Assembly.TryGetCustomAttribute(out attribute))
                return attribute;

            return null;
        }

        /// <summary>
        /// 获取根命名空间特性。
        /// </summary>
        /// <param name="assembly">给定的程序集。</param>
        /// <returns>返回 <see cref="RootNamespaceAttribute"/>。</returns>
        protected override RootNamespaceAttribute GetRootNamespaceAttribute(Assembly assembly)
        {
            if (assembly.TryGetCustomAttribute(out RootNamespaceAttribute rootNamespace))
                return rootNamespace;

            if (assembly.TryGetCustomAttribute(out AbstractionRootNamespaceAttribute abstractionRootNamespace))
                return new RootNamespaceAttribute(abstractionRootNamespace.RootNamespace);

            return null;
        }

    }
}
