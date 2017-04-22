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
    using Algorithm;
    using Utility;

    /// <summary>
    /// 密码管理器接口。
    /// </summary>
    public class PasswdManager : LibrameBase<PasswdManager>, IPasswdManager
    {
        private readonly IAlgorithmAdapter _algo = null;

        /// <summary>
        /// 构造一个 <see cref="PasswdManager"/> 实例。
        /// </summary>
        public PasswdManager()
        {
            _algo = LibrameArchitecture.AdapterManager.AlgorithmAdapter;
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
            var encode = _algo.Hash.ToSha384(raw);

            // AES (防撞库)
            encode = _algo.StandardAes.Encrypt(encode);

            return encode;
        }


        /// <summary>
        /// 验证两个密码是否相等。
        /// </summary>
        /// <param name="encode">给定经过编码的密码。</param>
        /// <param name="raw">给定未处理用于对比的原始密码。</param>
        /// <returns>返回字符串。</returns>
        public virtual bool RawEquals(string encode, string raw)
        {
            if (string.IsNullOrEmpty(encode) || string.IsNullOrEmpty(raw))
                return false;

            // 先编码
            var compareEncode = Encode(raw);

            return Equals(encode, compareEncode);
        }

        /// <summary>
        /// 验证两个密码是否相等。
        /// </summary>
        /// <param name="encode">给定经过编码的密码。</param>
        /// <param name="compareEncode">给定经过编码用于对比的密码。</param>
        /// <returns>返回字符串。</returns>
        public virtual bool Equals(string encode, string compareEncode)
        {
            if (string.IsNullOrEmpty(encode) || string.IsNullOrEmpty(compareEncode))
                return false;

            try
            {
                // 先解码
                var decode = _algo.StandardAes.Decrypt(encode);
                var compareDecode = _algo.StandardAes.Decrypt(compareEncode);

                return (decode == compareDecode);
            }
            catch (Exception ex)
            {
                Log.Error(ex.AsOrInnerMessage(), ex);

                return false;
            }
        }

    }
}
