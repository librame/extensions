#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Algorithm
{
    using Utility;

    /// <summary>
    /// 抽象字节编解码器。
    /// </summary>
    public abstract class AbstractByteCodec : LibrameBase<AbstractByteCodec>, IByteCodec
    {
        /// <summary>
        /// 获取 <see cref="AlgorithmSettings"/>。
        /// </summary>
        public AlgorithmSettings AlgoSettings { get; }

        /// <summary>
        /// 构造一个 <see cref="AbstractByteCodec"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgoSettings"/>。</param>
        public AbstractByteCodec(AlgorithmSettings algoSettings)
        {
            AlgoSettings = algoSettings.NotNull(nameof(algoSettings));
        }


        /// <summary>
        /// 将字符串编码为字节序列。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字节数组。</returns>
        protected virtual byte[] GetBytes(string str)
        {
            return AlgoSettings.AdapterSettings.Encoding.GetBytes(str);
        }

        /// <summary>
        /// 将字节序列解码为字符串。
        /// </summary>
        /// <param name="buffer">给定的字节序列。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string GetString(byte[] buffer)
        {
            return AlgoSettings.AdapterSettings.Encoding.GetString(buffer, 0, buffer.Length);
        }


        /// <summary>
        /// 编码为 BASE64。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回 BASE64 字符串。</returns>
        protected virtual string EncodeBase64(byte[] buffer)
        {
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 解码 BASE64。
        /// </summary>
        /// <param name="base64">给定的 BASE64 字符串。</param>
        /// <returns>返回字节数组。</returns>
        protected virtual byte[] DecodeBase64(string base64)
        {
            return Convert.FromBase64String(base64);
        }


        /// <summary>
        /// 编码为 BIT。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回 16 进制字符串。</returns>
        protected virtual string EncodeBit(byte[] buffer)
        {
            return ByteUtility.AsBit(buffer);
        }

        /// <summary>
        /// 解码 BIT。
        /// </summary>
        /// <param name="bit">给定的 16 进制字符串。</param>
        /// <returns>返回字节数组。</returns>
        protected virtual byte[] DecodeBit(string bit)
        {
            return ByteUtility.FromBit(bit);
        }

    }
}
