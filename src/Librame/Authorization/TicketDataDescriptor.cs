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

namespace Librame.Authorization
{
    using Descriptors;
    using Utility;

    /// <summary>
    /// 票根数据描述符。
    /// </summary>
    [Serializable]
    public class TicketDataDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="TicketDataDescriptor"/> 实例。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="account">给定的帐户。</param>
        public TicketDataDescriptor(string token, IAccountDescriptor account)
        {
            Token = token;
            Account = account.NotNull(nameof(account));
        }


        /// <summary>
        /// 获取令牌。
        /// </summary>
        public string Token { get; }
        
        /// <summary>
        /// 获取帐户。
        /// </summary>
        public IAccountDescriptor Account { get; }


        /// <summary>
        /// 序列化票根数据描述符实例。
        /// </summary>
        /// <param name="descriptor">给定的票根数据描述符。</param>
        /// <returns>返回字符串。</returns>
        public static string Serialize(TicketDataDescriptor descriptor)
        {
            return Serializer.SerializeBase64(descriptor);
        }

        /// <summary>
        /// 反序列化票根数据描述符实例。
        /// </summary>
        /// <param name="descriptor">给定序列化的票根数据描述符字符串形式。</param>
        /// <returns>返回票根数据描述符实例。</returns>
        public static TicketDataDescriptor Deserialize(string descriptor)
        {
            return Serializer.DeserializeBase64<TicketDataDescriptor>(descriptor);
        }

    }
}
