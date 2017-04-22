#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Algorithm.Symmetries
{
    /// <summary>
    /// 对称算法接口。
    /// </summary>
    public interface ISymmetryAlgorithm
    {
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="Adaptation.AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回加密字符串。</returns>
        string Encrypt(string str, string guid = null);
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回加密字符串。</returns>
        string Encrypt(string str, byte[] key);


        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="Adaptation.AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回原始字符串。</returns>
        string Decrypt(string encrypt, string guid = null);
        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回原始字符串。</returns>
        string Decrypt(string encrypt, byte[] key);


        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="Adaptation.AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] GenerateKey(string guid = null);

        /// <summary>
        /// 生成向量。
        /// </summary>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回用于当前算法的向量字节数组。</returns>
        byte[] GenerateIV(byte[] key);
    }
}
