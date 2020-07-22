#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Tokens
{
    /// <summary>
    /// 安全令牌保护器。
    /// </summary>
    public interface ISecurityTokenProtector
    {
        /// <summary>
        /// 钥匙环。
        /// </summary>
        ISecurityTokenKeyRing KeyRing { get; }


        /// <summary>
        /// 保护数据。
        /// </summary>
        /// <param name="index">给定可在 <see cref="ISecurityTokenKeyRing"/> 中查询的索引。</param>
        /// <param name="data">给定的数据。</param>
        /// <returns>返回字符串。</returns>
        string Protect(string index, string data);

        /// <summary>
        /// 解除保护。
        /// </summary>
        /// <param name="index">给定可在 <see cref="ISecurityTokenKeyRing"/> 中查询的索引。</param>
        /// <param name="data">给定的数据。</param>
        /// <returns>返回字符串。</returns>
        string Unprotect(string index, string data);
    }
}
