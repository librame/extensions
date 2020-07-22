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
    using Tokens;

    /// <summary>
    /// 安全标识符字符串转换器。
    /// </summary>
    public class SecurityTokenStringSerializer : AbstractStringSerializer<SecurityToken>
    {
        /// <summary>
        /// 构造一个 <see cref="SecurityTokenStringSerializer"/>。
        /// </summary>
        public SecurityTokenStringSerializer()
            : base(f => f, r => new SecurityToken(r))
        {
        }

    }
}
