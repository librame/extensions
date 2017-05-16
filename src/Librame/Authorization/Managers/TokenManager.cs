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

namespace Librame.Authorization.Managers
{
    using Adaptation;
    using Utility;

    /// <summary>
    /// 令牌管理器。
    /// </summary>
    public class TokenManager : AbstractAdapterCollectionManager, ITokenManager
    {
        /// <summary>
        /// 构造一个令牌管理器实例。
        /// </summary>
        /// <param name="adapters">给定的适配器器管理器。</param>
        public TokenManager(IAdapterCollection adapters)
            : base(adapters)
        {
        }

        /// <summary>
        /// 编码令牌。
        /// </summary>
        /// <param name="hex">给定的十六进制字符串。</param>
        /// <returns>返回编码后的字符串。</returns>
        protected virtual string Encode(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return string.Empty;

            // SHA384
            var encode = Adapters.Algorithm.Hash.ToSha384(hex);

            //// AES (防撞库)
            //encode = Adapters.Algorithm.StandardAes.Encrypt(encode);

            return encode;
        }


        /// <summary>
        /// 生成令牌。
        /// </summary>
        /// <returns>返回令牌字符串。</returns>
        public virtual string Generate()
        {
            return Generate(Guid.NewGuid());
        }

        /// <summary>
        /// 生成令牌。
        /// </summary>
        /// <param name="guid">给定的 GUID。</param>
        /// <returns>返回令牌字符串。</returns>
        public virtual string Generate(Guid guid)
        {
            var hex = GuidUtility.AsHex(guid);

            return Encode(hex);
        }

    }
}
