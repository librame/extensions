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
    using Utility;

    /// <summary>
    /// 密文管理器。
    /// </summary>
    public class CiphertextManager : AbstractAdapterCollectionManager, ICiphertextManager
    {
        /// <summary>
        /// 构造一个密文管理器实例。
        /// </summary>
        /// <param name="adapters">给定的适配器器管理器。</param>
        public CiphertextManager(IAdapterCollection adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 编码。
        /// </summary>
        /// <param name="str">给定要编码的字符串。</param>
        /// <returns>返回编码字符串。</returns>
        public virtual string Encode(string str)
        {
            str.NotEmpty(nameof(str));
            
            // 默认使用标准对称加密算法
            var sa = Adapters.Algorithm.StandardAes;
            return sa.Encrypt(str);
        }

        /// <summary>
        /// 解码。
        /// </summary>
        /// <param name="encode">给定要解码的字符串。</param>
        /// <returns>返回原始字符串。</returns>
        public virtual string Decode(string encode)
        {
            encode.NotEmpty(nameof(encode));

            // 默认使用标准对称加密算法
            var sa = Adapters.Algorithm.StandardAes;
            return sa.Decrypt(encode);
        }

    }
}
