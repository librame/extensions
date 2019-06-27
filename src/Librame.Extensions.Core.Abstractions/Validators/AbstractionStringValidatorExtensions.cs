#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象字符串验证器静态扩展。
    /// </summary>
    public static class AbstractionStringValidatorExtensions
    {
        /// <summary>
        /// 是否为数字。
        /// </summary>
        /// <param name="validator">给定的 <see cref="IStringValidator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this IStringValidator validator)
        {
            return validator.RawSource.IsDigit();
        }

    }
}
