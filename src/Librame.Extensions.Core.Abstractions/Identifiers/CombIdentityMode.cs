#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// COMB 标识方式。
    /// </summary>
    public enum CombIdentityMode
    {
        /// <summary>
        /// 作为二进制。
        /// </summary>
        AsBinary = 1,

        /// <summary>
        /// 作为字符串。
        /// </summary>
        AsString = 2,

        /// <summary>
        /// 位于末尾。
        /// </summary>
        AtEnd = 3
    }
}
