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

namespace Librame.Localizations
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
            Logger = loggerFactory.CreateLogger<ExpressionStringLocalizerFactory>();
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

            var reusableAttribute = GetReusableResourceAttribute(typeInfo);
            if (reusableAttribute.IsNotDefault() && reusableAttribute.Enabled)
            {
                if (reusableAttribute.PrefixFactory.IsDefault())
                {
                    reusableAttribute.PrefixFactory = (_baseNamespace, _resourcesRelativePath, _typeInfo) =>
                    {
                        if (resourcesRelativePath.IsEmpty())
                            return $"{_baseNamespace}.{_typeInfo.Name}";
                        else
                            return $"{_baseNamespace}.{_resourcesRelativePath}{_typeInfo.Name}"; // 已格式化为点分隔符（如：Resources.）
                    };
                }

                prefix = reusableAttribute.PrefixFactory.Invoke(baseNamespace, resourcesRelativePath, typeInfo);

                Logger.LogInformation($"The resource prefix: {prefix} ({nameof(ReusableResourceAttribute)}.{nameof(reusableAttribute.Enabled)}={reusableAttribute.Enabled})");
            }
            else
            {
                prefix = base.GetResourcePrefix(typeInfo, baseNamespace, resourcesRelativePath);

                Logger.LogInformation($"The resource prefix: {prefix} (from {nameof(ResourceManagerStringLocalizerFactory)})");
            }

            return prefix;
        }


        /// <summary>
        /// 获取可重用资源特性。
        /// </summary>
        /// <param name="typeInfo">给定的 <see cref="TypeInfo"/>。</param>
        /// <returns>返回 <see cref="ReusableResourceAttribute"/>。</returns>
        protected virtual ReusableResourceAttribute GetReusableResourceAttribute(TypeInfo typeInfo)
        {
            if (typeInfo.TryAsAttribute(out ReusableResourceAttribute typeAttribute))
                return typeAttribute;

            if (typeInfo.Assembly.TryAsAttribute(out ReusableResourceAttribute assemblyAttribute))
                return assemblyAttribute;

            return null;
        }

    }
}
