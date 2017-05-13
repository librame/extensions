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
    public interface ICryptogramManager : IAdapterManagerReference
    {
        /// <summary>
        /// 加密实例。
        /// </summary>
        /// <typeparam name="TValue">指定的实例类型。</typeparam>
        /// <param name="value">给定的实例。</param>
        /// <returns>返回字符串。</returns>
        string Encrypt<TValue>(TValue value);

        /// <summary>
        /// 解密实例。
        /// </summary>
        /// <typeparam name="TValue">指定的实例类型。</typeparam>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <returns>返回实例。</returns>
        TValue Decrypt<TValue>(string encrypt);
    }
}
