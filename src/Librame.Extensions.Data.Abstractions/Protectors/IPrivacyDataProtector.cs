#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Protectors
{
    /// <summary>
    /// 隐私数据保护器接口。
    /// </summary>
    public interface IPrivacyDataProtector
    {
        /// <summary>
        /// 保护数据。
        /// </summary>
        /// <param name="data">给定的数据。</param>
        /// <returns>返回字符串。</returns>
        string Protect(string data);

        /// <summary>
        /// 解除保护。
        /// </summary>
        /// <param name="data">给定的数据。</param>
        /// <returns>返回字符串。</returns>
        string Unprotect(string data);
    }
}
