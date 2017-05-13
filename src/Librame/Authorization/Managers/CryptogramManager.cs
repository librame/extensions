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
    public class CryptogramManager : AbstractAdapterManagerReference, ICryptogramManager
    {
        /// <summary>
        /// 构造一个密文管理器实例。
        /// </summary>
        /// <param name="adapters">给定的适配器器管理器。</param>
        public CryptogramManager(IAdapterManager adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 加密实例。
        /// </summary>
        /// <typeparam name="TValue">指定的实例类型。</typeparam>
        /// <param name="value">给定的实例。</param>
        /// <returns>返回字符串。</returns>
        public virtual string Encrypt<TValue>(TValue value)
        {
            value.NotNull(nameof(value));

            // 转换为 JSON
            var json = value.AsJson();

            // 默认使用标准对称加密算法
            var sa = Adapters.Algorithm.StandardAes;
            return sa.Encrypt(json);
        }

        /// <summary>
        /// 解密实例。
        /// </summary>
        /// <typeparam name="TValue">指定的实例类型。</typeparam>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <returns>返回实例。</returns>
        public virtual TValue Decrypt<TValue>(string encrypt)
        {
            encrypt.NotNullOrEmpty(nameof(encrypt));

            // 默认使用标准对称加密算法
            var sa = Adapters.Algorithm.StandardAes;
            var json = sa.Decrypt(encrypt);

            // 还原票根
            return json.FromJson<TValue>();
        }

    }
}
