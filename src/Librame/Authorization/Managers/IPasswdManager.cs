#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Authorization.Managers
{
    using Adaptation;

    /// <summary>
    /// 密码管理器接口。
    /// </summary>
    public interface IPasswdManager : IAdapterCollectionManager
    {
        /// <summary>
        /// 编码密码。
        /// </summary>
        /// <param name="raw">给定未处理的原始密码。</param>
        /// <returns>返回编码后的字符串。</returns>
        string Encode(string raw);

        
        /// <summary>
        /// 验证两个密码（左边密码编码，右边密码未编码）是否相等。
        /// </summary>
        /// <param name="left">给定左边的密码。</param>
        /// <param name="right">给定右边的密码。</param>
        /// <returns>返回字符串。</returns>
        bool EncodeRawEquals(string left, string right);

        /// <summary>
        /// 验证两个经过编码的密码是否相等。
        /// </summary>
        /// <param name="left">给定左边的密码。</param>
        /// <param name="right">给定右边的密码。</param>
        /// <returns>返回字符串。</returns>
        bool EncodeEquals(string left, string right);

        /// <summary>
        /// 验证两个原始密码（未编码）是否相等。
        /// </summary>
        /// <param name="left">给定左边的密码。</param>
        /// <param name="right">给定右边的密码。</param>
        /// <returns>返回字符串。</returns>
        bool RawEquals(string left, string right);
    }
}
