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
    /// 密文管理器接口。
    /// </summary>
    public interface ICiphertextManager : IAdapterCollectionManager
    {
        /// <summary>
        /// 编码。
        /// </summary>
        /// <param name="str">给定要编码的字符串。</param>
        /// <returns>返回编码字符串。</returns>
        string Encode(string str);

        /// <summary>
        /// 解码。
        /// </summary>
        /// <param name="encode">给定要解码的字符串。</param>
        /// <returns>返回原始字符串。</returns>
        string Decode(string encode);
    }
}
