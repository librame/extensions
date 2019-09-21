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
using System.Collections.Concurrent;
using System.Globalization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象资源字典。
    /// </summary>
    public abstract class AbstractResourceDictionary : ConcurrentDictionary<string, object>, IResourceDictionary
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractResourceDictionary"/>。
        /// </summary>
        public AbstractResourceDictionary()
            : base()
        {
        }


        /// <summary>
        /// 获取资源字典。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <param name="cultureName">给定的 <see cref="CultureInfo"/>。</param>
        /// <returns>返回 <see cref="IResourceDictionary"/>。</returns>
        public static IResourceDictionary GetResourceDictionary<TResource>(string cultureName)
            where TResource : IResource
            => GetResourceDictionary<TResource>(new CultureInfo(cultureName));

        /// <summary>
        /// 获取资源字典。
        /// </summary>
        /// <typeparam name="TResource">指定的资源类型。</typeparam>
        /// <param name="cultureInfo">给定的 <see cref="CultureInfo"/>。</param>
        /// <returns>返回 <see cref="IResourceDictionary"/>。</returns>
        public static IResourceDictionary GetResourceDictionary<TResource>(CultureInfo cultureInfo)
            where TResource : IResource
            => GetResourceDictionaryCore(cultureInfo, typeof(TResource));


        /// <summary>
        /// 获取资源字典。
        /// </summary>
        /// <param name="cultureName">给定的文化名称。</param>
        /// <param name="resourceType">给定的资源类型。</param>
        /// <returns>返回 <see cref="IResourceDictionary"/>。</returns>
        public static IResourceDictionary GetResourceDictionary(string cultureName, Type resourceType)
            => GetResourceDictionary(new CultureInfo(cultureName), resourceType);

        /// <summary>
        /// 获取资源字典。
        /// </summary>
        /// <param name="cultureInfo">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="resourceType">给定的资源类型。</param>
        /// <returns>返回 <see cref="IResourceDictionary"/>。</returns>
        public static IResourceDictionary GetResourceDictionary(CultureInfo cultureInfo, Type resourceType)
        {
            resourceType.AssignableToBase(typeof(IResource));
            return GetResourceDictionaryCore(cultureInfo, resourceType);
        }

        private static IResourceDictionary GetResourceDictionaryCore(CultureInfo cultureInfo, Type resourceType)
        {
            cultureInfo.NotNull(nameof(cultureInfo));

            var suffixName = cultureInfo.Name.Replace('-', '_');
            var cultureType = Type.GetType($"{resourceType.FullName}_{suffixName}, {resourceType.Assembly}");
            return (IResourceDictionary)cultureType.EnsureCreateObject();
        }
    }
}
