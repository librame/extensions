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
        /// GUID 型标识生成器。
        /// </summary>
        public IIdentificationGenerator<Guid> GuidIdentificationGenerator { get; set; }

        /// <summary>
        /// 长整型标识生成器。
        /// </summary>
        public IIdentificationGenerator<long> LongIdentificationGenerator { get; set; }

        /// <summary>
        /// 字符串标识生成器。
        /// </summary>
        public IIdentificationGenerator<string> StringIdentificationGenerator { get; set; }
    }
}
