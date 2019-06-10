#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Globalization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 构建器全局化。
    /// </summary>
    public class BuilderGlobalization
    {
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
