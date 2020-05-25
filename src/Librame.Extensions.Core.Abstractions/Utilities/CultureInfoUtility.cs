#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Globalization;

namespace Librame.Extensions.Core.Utilities
{
    /// <summary>
    /// <see cref="CultureInfo"/> 实用工具。
    /// </summary>
    public static class CultureInfoUtility
    {
        /// <summary>
        /// 注册文化信息。
        /// </summary>
        /// <param name="cultureInfo">给定的 <see cref="CultureInfo"/>。</param>
        /// <returns>返回 <see cref="CultureInfo"/>。</returns>
        public static CultureInfo Register(CultureInfo cultureInfo)
        {
            cultureInfo.NotNull(nameof(cultureInfo));
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = cultureInfo;

            return cultureInfo;
        }

        /// <summary>
        /// 注册文化信息。
        /// </summary>
        /// <param name="cultureInfo">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="uiCultureInfo">给定的 UI <see cref="CultureInfo"/>。</param>
        public static void Register(CultureInfo cultureInfo, CultureInfo uiCultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo.NotNull(nameof(cultureInfo));
            CultureInfo.CurrentUICulture = uiCultureInfo.NotNull(nameof(uiCultureInfo));
        }

    }
}
