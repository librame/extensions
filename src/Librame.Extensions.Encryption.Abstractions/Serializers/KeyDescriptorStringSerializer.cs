#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.Serializers
{
    using Core.Serializers;
    using Encryption.KeyGenerators;

    /// <summary>
    /// 密钥描述符字符串转换器。
    /// </summary>
    public class KeyDescriptorStringSerializer : AbstractStringSerializer<KeyDescriptor>
    {
        /// <summary>
        /// 反序列化字符串为类型。
        /// </summary>
        /// <param name="target">给定的字符串。</param>
        /// <returns>返回类型。</returns>
        public override KeyDescriptor Deserialize(string target)
            => new KeyDescriptor(target);

        /// <summary>
        /// 序列化类型为字符串。
        /// </summary>
        /// <param name="source">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public override string Serialize(KeyDescriptor source)
            => source?.ToString();
    }
}
