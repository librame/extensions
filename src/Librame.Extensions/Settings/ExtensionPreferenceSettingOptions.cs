#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Security.Cryptography;

namespace Librame.Extensions
{
    /// <summary>
    /// 扩展偏好设置选项。
    /// </summary>
    public class ExtensionPreferenceSettingOptions : ISettingOptions
    {
        /// <summary>
        /// HMAC 键控短密钥。
        /// </summary>
        public string HmacShortKey { get; set; }

        /// <summary>
        /// HMAC 键控长密钥。
        /// </summary>
        public string HmacLongKey { get; set; }

        /// <summary>
        /// AES 密钥。
        /// </summary>
        public string AesKey { get; set; }

        /// <summary>
        /// AES 向量。
        /// </summary>
        public string AesVector { get; set; }

        /// <summary>
        /// DES 密钥。
        /// </summary>
        public string DesKey { get; set; }

        /// <summary>
        /// DES 向量。
        /// </summary>
        public string DesVector { get; set; }

        /// <summary>
        /// TripleDES 密钥。
        /// </summary>
        public string TripleDesKey { get; set; }

        /// <summary>
        /// TripleDES 向量。
        /// </summary>
        public string TripleDesVector { get; set; }


        /// <summary>
        /// 获取默认选项。
        /// </summary>
        /// <returns>返回 <see cref="ExtensionPreferenceSettingOptions"/>。</returns>
        internal static ExtensionPreferenceSettingOptions GetDefaultOptions()
        {
            var options = new ExtensionPreferenceSettingOptions();

            var hmac = AlgorithmExtensions.HmacAlgorithms.GetOrAdd(typeof(HMACSHA256).Name,
                key => HMAC.Create(key));
            options.HmacShortKey = hmac.Key.AsBase64String();

            hmac = AlgorithmExtensions.HmacAlgorithms.GetOrAdd(typeof(HMACSHA512).Name,
                key => HMAC.Create(key));
            options.HmacLongKey = hmac.Key.AsBase64String();

            var sym = AlgorithmExtensions.SymmetricAlgorithms.GetOrAdd(typeof(Aes).Name,
                key => SymmetricAlgorithm.Create(key));
            options.AesKey = sym.Key.AsBase64String();
            options.AesVector = sym.IV.AsBase64String();

            sym = AlgorithmExtensions.SymmetricAlgorithms.GetOrAdd(typeof(DES).Name,
                key => SymmetricAlgorithm.Create(key));
            options.DesKey = sym.Key.AsBase64String();
            options.DesVector = sym.IV.AsBase64String();

            sym = AlgorithmExtensions.SymmetricAlgorithms.GetOrAdd(typeof(TripleDES).Name,
                key => SymmetricAlgorithm.Create(key));
            options.TripleDesKey = sym.Key.AsBase64String();
            options.TripleDesVector = sym.IV.AsBase64String();

            return options;
        }

    }
}
