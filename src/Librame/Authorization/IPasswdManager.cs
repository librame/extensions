#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Authorization
{
    /// <summary>
    /// 密码管理器接口。
    /// </summary>
    public interface IPasswdManager
    {
        /// <summary>
        /// 编码密码。
        /// </summary>
        /// <param name="raw">给定未处理的原始密码。</param>
        /// <returns>返回编码后的字符串。</returns>
        string Encode(string raw);


        /// <summary>
        /// 验证两个密码是否相等。
        /// </summary>
        /// <param name="encode">给定经过编码的密码。</param>
        /// <param name="raw">给定未处理用于对比的原始密码。</param>
        /// <returns>返回字符串。</returns>
        bool RawEquals(string encode, string raw);

        /// <summary>
        /// 验证两个密码是否相等。
        /// </summary>
        /// <param name="encode">给定经过编码的密码。</param>
        /// <param name="compareEncode">给定经过编码用于对比的密码。</param>
        /// <returns>返回字符串。</returns>
        bool Equals(string encode, string compareEncode);
    }
}
