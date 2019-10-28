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
    /// <summary>
    /// 核心资源管理器字符串定位器工厂。
    /// </summary>
    public class CoreResourceManagerStringLocalizerFactory : ResourceManagerStringLocalizerFactory
    {
        /// <summary>
        /// 构造一个 <see cref="CoreResourceManagerStringLocalizerFactory"/>。
        /// </summary>
        /// <param name="localizationOptions">给定的 <see cref="IOptions{LocalizationOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        public CoreResourceManagerStringLocalizerFactory(IOptions<LocalizationOptions> localizationOptions, ILoggerFactory loggerFactory,
            IOptions<CoreBuilderOptions> builderOptions)
            : base(localizationOptions, loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger<CoreResourceManagerStringLocalizerFactory>();

            BuilderOptions = builderOptions.Value;
        }


        /// <summary>
        /// 日志工厂。
        /// </summary>
        protected ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        protected CoreBuilderOptions BuilderOptions { get; }


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
                if (resourceMapping.Factory.IsNull())
                    resourceMapping.Factory = BuilderOptions.ResourceMappingFactory;

                prefix = resourceMapping.Factory.Invoke(new ResourceMappingDescriptor(typeInfo, baseNamespace, resourcesRelativePath));
                Logger.LogTrace($"Get resource prefix “{prefix}” by {nameof(CoreResourceManagerStringLocalizerFactory)}");
            }
            else
            {
                prefix = base.GetResourcePrefix(typeInfo, baseNamespace, resourcesRelativePath);
                Logger.LogTrace($"Get resource prefix “{prefix}” by {nameof(ResourceManagerStringLocalizerFactory)}");
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
