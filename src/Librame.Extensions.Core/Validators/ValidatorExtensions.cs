#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions
{
    using Core;

    /// <summary>
    /// 转换器静态扩展。
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// 转换为字符串验证器。
        /// </summary>
        /// <param name="str">给定要验证的字符串。</param>
        /// <returns>返回 <see cref="IStringValidator"/>。</returns>
        public static IStringValidator AsStringConverter(this string str)
        {
            return new StringValidator(str);
        }

    }
}
