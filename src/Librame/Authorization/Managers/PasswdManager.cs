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
    /// 密码管理器。
    /// </summary>
    public class PasswdManager : AbstractAdapterCollectionManager, IPasswdManager
    {
        /// <summary>
        /// 构造一个密码管理器实例。
        /// </summary>
        /// <param name="adapters">给定的适配器器管理器。</param>
        public PasswdManager(IAdapterCollection adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 编码密码。
        /// </summary>
        /// <param name="raw">给定未处理的原始密码。</param>
        /// <returns>返回编码后的字符串。</returns>
        public virtual string Encode(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                return string.Empty;

            // SHA384
            var encode = Adapters.Algorithm.Hash.ToSha384(raw);

            // AES (防撞库)
            encode = Adapters.Algorithm.StandardAes.Encrypt(encode);

            return encode;
        }

        
        /// <summary>
        /// 验证两个密码（左边密码编码，右边密码未编码）是否相等。
        /// </summary>
        /// <param name="left">给定左边的密码。</param>
        /// <param name="right">给定右边的密码。</param>
        /// <returns>返回字符串。</returns>
        public virtual bool EncodeRawEquals(string left, string right)
        {
            if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
                return false;

            return EncodeEquals(left, Encode(right));
        }

        /// <summary>
        /// 验证两个密码是否相等。
        /// </summary>
        /// <param name="left">给定左边的密码。</param>
        /// <param name="right">给定右边的密码。</param>
        /// <returns>返回字符串。</returns>
        public virtual bool EncodeEquals(string left, string right)
        {
            if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
                return false;

            try
            {
                // 先解码
                var decodeLeft = Adapters.Algorithm.StandardAes.Decrypt(left);
                var decodeRight = Adapters.Algorithm.StandardAes.Decrypt(right);

                return (decodeLeft == decodeRight);
            }
            catch (Exception ex)
            {
                Log.Warn(ex.InnerMessage(), ex);

                // 尝试直接对比
                return RawEquals(left, right);
            }
        }

        /// <summary>
        /// 验证两个原始密码（未编码）是否相等。
        /// </summary>
        /// <param name="left">给定左边的密码。</param>
        /// <param name="right">给定右边的密码。</param>
        /// <returns>返回字符串。</returns>
        public virtual bool RawEquals(string left, string right)
        {
            if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
                return false;

            return (left == right);
        }

    }
}
