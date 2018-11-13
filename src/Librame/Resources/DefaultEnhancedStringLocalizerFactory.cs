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

namespace Librame.Resources
{
    using Extensions;

    /// <summary>
    /// 默认增强型字符串定位器工厂接口。
    /// </summary>
    public class DefaultEnhancedStringLocalizerFactory : ResourceManagerStringLocalizerFactory, IStringLocalizerFactory
    {
        /// <summary>
        /// 构造一个 <see cref="DefaultEnhancedStringLocalizerFactory"/> 实例。
        /// </summary>
        /// <param name="localizationOptions">给定的 <see cref="IOptions{LocalizationOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DefaultEnhancedStringLocalizerFactory(IOptions<LocalizationOptions> localizationOptions, ILoggerFactory loggerFactory)
            : base(localizationOptions, loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<DefaultEnhancedStringLocalizerFactory>();
        }


        /// <summary>
        /// 记录器。
        /// </summary>
        protected ILogger Logger { get; }


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

            if (typeInfo.Assembly.HasAttribute(out ResourcePrefixAttribute attribute) && attribute.UseEnhanced)
            {
                if (resourcesRelativePath.IsEmpty())
                    prefix = $"{baseNamespace}.{typeInfo.Name}"; // typeInfo.FullName;

                // resourcesRelativePath 已格式化为 Resources. 模式
                prefix = $"{baseNamespace}.{resourcesRelativePath}{typeInfo.Name}";

                Logger.LogInformation($"{typeInfo.FullName} resource prefix is {prefix} from {nameof(DefaultEnhancedStringLocalizerFactory)}({nameof(attribute.UseEnhanced)}={attribute.UseEnhanced})");
            }
            else
            {
                prefix = base.GetResourcePrefix(typeInfo, baseNamespace, resourcesRelativePath);

                Logger.LogInformation($"{typeInfo.FullName} resource prefix is {prefix} from {nameof(ResourceManagerStringLocalizerFactory)}");
            }

            return prefix;
        }

    }
}
