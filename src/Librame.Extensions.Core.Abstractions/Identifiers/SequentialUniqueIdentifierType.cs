#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// 有序唯一标识符类型。
    /// </summary>
    public enum SequentialUniqueIdentifierType
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
