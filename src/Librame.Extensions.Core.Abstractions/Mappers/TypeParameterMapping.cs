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

namespace Librame.Extensions.Core.Mappers
{
    /// <summary>
    /// 类型参数映射。
    /// </summary>
    public class TypeParameterMapping : IEquatable<TypeParameterMapping>
    {
        /// <summary>
        /// 构造一个 <see cref="TypeParameterMapping"/>。
        /// </summary>
        /// <param name="parameterType">给定的定义形参类型。</param>
        /// <param name="argumentType">给定的调用实参类型（可选）。</param>
        public TypeParameterMapping(Type parameterType, Type argumentType = null)
        {
            ParameterType = parameterType.NotNull(nameof(parameterType));
            ArgumentType = argumentType;
        }


        /// <summary>
        /// 定义形参类型。
        /// </summary>
        public Type ParameterType { get; }

        /// <summary>
        /// 调用实参类型。
        /// </summary>
        public Type ArgumentType { get; set; }


        /// <summary>
        /// 定义形参名称。
        /// </summary>
        public string ParameterName
            => ParameterType.Name;

        /// <summary>
        /// 调用实参名称。
        /// </summary>
        public string ArgumentName
            => ArgumentType?.Name;


        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="other">给定的 <see cref="TypeParameterMapping"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(TypeParameterMapping other)
            => ParameterType == other?.ParameterType && ArgumentType == other.ArgumentType;

        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => Equals(obj as TypeParameterMapping);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
        {
            if (ArgumentType.IsNull())
                return ParameterType.GetHashCode();
            
            return ParameterType.GetHashCode() ^ ArgumentType.GetHashCode();
        }

    }
}
