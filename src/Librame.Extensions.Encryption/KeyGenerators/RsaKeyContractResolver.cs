#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Librame.Extensions.Encryption
{
    /// <summary>
    /// RSA 密钥合约解析器。
    /// </summary>
    internal class RsaKeyContractResolver : DefaultContractResolver
    {

        /// <summary>
        /// 创建属性。
        /// </summary>
        /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
        /// <param name="memberSerialization">给定的 <see cref="MemberSerialization"/>。</param>
        /// <returns>返回 <see cref="JsonProperty"/>。</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.Ignored = false;

            return property;
        }

    }
}
