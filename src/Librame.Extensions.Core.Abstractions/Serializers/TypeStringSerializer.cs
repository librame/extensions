#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
        /// 构造一个 <see cref="TypeStringSerializer"/>。
        /// </summary>
        public TypeStringSerializer()
            : base(f => f.GetAssemblyQualifiedNameWithoutVersion(),
                  r => Type.GetType(r, true))
        {
        }

    }
}
