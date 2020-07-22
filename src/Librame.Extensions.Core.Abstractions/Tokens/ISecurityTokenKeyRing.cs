#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Core.Tokens
{
    /// <summary>
    /// 安全令牌钥匙环。
    /// </summary>
    public interface ISecurityTokenKeyRing
    {
        /// <summary>
        /// 获取令牌。
        /// </summary>
        /// <param name="index">给定的索引。</param>
        /// <returns>返回字符串。</returns>
        string this[string index] { get; }

        /// <summary>
        /// 当前索引。
        /// </summary>
        string CurrentIndex { get; }


        /// <summary>
        /// 获取所有索引集合。
        /// </summary>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        IEnumerable<string> GetAllIndexes();
    }
}
