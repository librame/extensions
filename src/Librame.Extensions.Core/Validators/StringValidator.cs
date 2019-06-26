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
    /// 字符串验证器。
    /// </summary>
    public class StringValidator : AbstractValidator<string>, IStringValidator
    {
        /// <summary>
        /// 构建一个 <see cref="StringValidator"/> 实例。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        public StringValidator(string str)
            : base(str)
        {
        }
    }
}
