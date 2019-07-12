#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 全局唯一标识符。
    /// </summary>
    public class GuIdentifier : AbstractIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="GuIdentifier"/> 实例。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/> 。</param>
        public GuIdentifier(Guid guid)
            : this(GenerateBytes(guid))
        {
        }

        private GuIdentifier(byte[] buffer)
            : base(buffer)
        {
        }


        private static byte[] GenerateBytes(Guid guid)
        {
            return guid.ToByteArray();
        }


        /// <summary>
        /// 显式转换为 <see cref="GuIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定的全局唯一标识符。</param>
        public static explicit operator GuIdentifier(string identifier)
            => new GuIdentifier(identifier.FromBase64String());


        /// <summary>
        /// 只读空实例。
        /// </summary>
        public static readonly GuIdentifier Empty
            = new GuIdentifier(Guid.Empty);

        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <returns>返回 <see cref="GuIdentifier"/>。</returns>
        public static GuIdentifier New()
            => new GuIdentifier(Guid.NewGuid());
    }
}
