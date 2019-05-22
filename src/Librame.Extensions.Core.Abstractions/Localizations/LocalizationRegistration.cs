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
    /// 本地化注册。
    /// </summary>
    public class LocalizationRegistration
    {
        /// <summary>
        /// 注册文化信息。
        /// </summary>
        /// <param name="cultureName">给定的文化名称。</param>
        public static void Register(string cultureName)
        {
            Register(new CultureInfo(cultureName));
        }
        /// <summary>
        /// 注册文化信息。
        /// </summary>
        /// <param name="cultureInfo">给定的文化信息。</param>
        public static void Register(CultureInfo cultureInfo)
        {
            Register(cultureInfo, cultureInfo);
        }

        /// <summary>
        /// 注册文化信息。
        /// </summary>
        /// <param name="cultureName">给定的文化名称。</param>
        /// <param name="cultureUIName">给定的 UI 文化名称。</param>
        public static void Register(string cultureName, string cultureUIName)
        {
            Register(new CultureInfo(cultureName), new CultureInfo(cultureUIName));
        }
        /// <summary>
        /// 注册文化信息。
        /// </summary>
        /// <param name="cultureInfo">给定的文化信息。</param>
        /// <param name="cultureUIInfo">给定的 UI 文化信息。</param>
        public static void Register(CultureInfo cultureInfo, CultureInfo cultureUIInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo.NotNull(nameof(cultureInfo));
            CultureInfo.CurrentUICulture = cultureUIInfo.NotNull(nameof(cultureUIInfo));
        }

    }
}
