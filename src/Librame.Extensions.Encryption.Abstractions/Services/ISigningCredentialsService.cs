#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.IdentityModel.Tokens;

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;

    /// <summary>
    /// 签名证书服务接口。
    /// </summary>
    public interface ISigningCredentialsService : IService
    {
        /// <summary>
        /// 获取全局签名证书。
        /// </summary>
        /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
        SigningCredentials GetGlobalSigningCredentials();

        /// <summary>
        /// 获取签名证书。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
        SigningCredentials GetSigningCredentials(string key);
    }
}
