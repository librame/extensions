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

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 类型字符串序列化器。
    /// </summary>
    public class TypeStringSerializer : AbstractStringSerializer<Type>
    {
        /// <summary>
        /// 反序列化字符串为类型。
        /// </summary>
        /// <param name="target">给定的字符串。</param>
        /// <returns>返回类型。</returns>
        public override Type Deserialize(string target)
            => Type.GetType(target, true);

        /// <summary>
        /// 序列化类型为字符串。
        /// </summary>
        /// <param name="source">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public override string Serialize(Type source)
            => source.GetAssemblyQualifiedNameWithoutVersion();
    }
}
