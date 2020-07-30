#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Options
{
    using Identifiers;

    /// <summary>
    /// 标识符选项。
    /// </summary>
    public class IdentifierOptions
    {
        /// <summary>
        /// GUID 型标识符生成器。
        /// </summary>
        public IIdentityGenerator<Guid> GuidIdentifierGenerator { get; set; }

        /// <summary>
        /// 长整型标识符生成器。
        /// </summary>
        public IIdentityGenerator<long> LongIdentifierGenerator { get; set; }

        /// <summary>
        /// 字符串标识符生成器。
        /// </summary>
        public IIdentityGenerator<string> StringIdentifierGenerator { get; set; }
    }
}
