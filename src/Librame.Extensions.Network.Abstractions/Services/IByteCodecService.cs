#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 字节编解码服务接口。
    /// </summary>
    public interface IByteCodecService : INetworkService
    {
        /// <summary>
        /// 从字节数组解码字符串。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回原始字符串。</returns>
        string DecodeStringFromBytes(byte[] buffer, bool enableCodec);

        /// <summary>
        /// 解码字符串。
        /// </summary>
        /// <param name="encode">给定的编码字符串。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回原始字符串。</returns>
        string DecodeString(string encode, bool enableCodec);

        /// <summary>
        /// 解码字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回经过解码的字节数组。</returns>
        byte[] Decode(byte[] buffer);


        /// <summary>
        /// 编码字符串为字节数组。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回经过编码的字节数组。</returns>
        byte[] EncodeStringAsBytes(string str, bool enableCodec);

        /// <summary>
        /// 编码字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="enableCodec">启用编解码。</param>
        /// <returns>返回经过编码的字符串。</returns>
        string EncodeString(string str, bool enableCodec);

        /// <summary>
        /// 编码字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回经过编码的字节数组。</returns>
        byte[] Encode(byte[] buffer);
    }
}
