#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 算法标识符选项。
    /// </summary>
    public class AlgorithmIdentifierOptions
    {
        /// <summary>
        /// 散列字符串。
        /// </summary>
        public string HashString { get; set; }
            = "E7HAAAEAAAD/AAABcAkBAAwCACBrTGliAHJhbWUuRXh0AGVuc2lvbnMuAENvcmUuQWJzIHRyYWN0AUQsIAhWZXIBbD02LjEALjg4MC4yMDIALCBDdWx0dXIgZT1uZXUAVGwsACBQdWJsaWNLAGV5VG9rZW49AGUyZjdlNTIwAGVkZjg4Yzc1CgUBeTEVcFVuaXEAdWVBbGdvcmkAdGhtSWRlbnRAaWZpZXIDADUKAQcOCUNvbnZlchB0ZXIRBglUeXCEZU4AVAEEAS0VX3hIZXgGLgYcAYGBAQYBATIgQjY4ODVGAEJDQkZFNkZEADQyQUZDMEE2ADBGQzE5REJF0EFCCQQBFQUAAqsyHgWBGysZAdABTAs=";


        /// <summary>
        /// 从散列字符串还原。
        /// </summary>
        /// <returns>返回 <see cref="IAlgorithmIdentifier"/>。</returns>
        public IAlgorithmIdentifier FromHashString()
        {
            return (IAlgorithmIdentifier)HashString
                .FromBase64String()
                .RtlDecompress()
                .DeserializeBinary();
        }

        /// <summary>
        /// 设置散列字符串。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IAlgorithmIdentifier"/>。</param>
        /// <returns>返回散列字符串。</returns>
        public string SetHashString(IAlgorithmIdentifier identifier)
        {
            HashString = identifier
                .SerializeBinary()
                .RtlCompress()
                .SerializeBinary()
                .AsBase64String();

            return HashString;
        }
    }
}
