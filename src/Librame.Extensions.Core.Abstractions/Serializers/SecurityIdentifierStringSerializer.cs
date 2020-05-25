#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Serializers
{
    using Identifiers;

    /// <summary>
    /// 安全标识符字符串转换器。
    /// </summary>
    public class SecurityIdentifierStringSerializer : AbstractStringSerializer<SecurityIdentifier>
    {
        /// <summary>
        /// 构造一个 <see cref="SecurityIdentifierStringSerializer"/>。
        /// </summary>
        public SecurityIdentifierStringSerializer()
            : base(f => f, r => new SecurityIdentifier(r))
        {
        }

    }
}
