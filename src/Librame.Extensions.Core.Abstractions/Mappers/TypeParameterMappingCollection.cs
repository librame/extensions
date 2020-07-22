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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Librame.Extensions.Core.Mappers
{
    /// <summary>
    /// <see cref="TypeParameterMapping"/> 集合。
    /// </summary>
    [Serializable]
    public class TypeParameterMappingCollection : Dictionary<string, TypeParameterMapping>
    {
        /// <summary>
        /// 构造一个 <see cref="TypeParameterMappingCollection"/>。
        /// </summary>
        public TypeParameterMappingCollection()
            : base()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="TypeParameterMappingCollection"/>。
        /// </summary>
        /// <param name="dictionary">给定的类型参数映射键值对字典。</param>
        public TypeParameterMappingCollection(IDictionary<string, TypeParameterMapping> dictionary)
            : base(dictionary)
        {
        }

#if !NET48
        /// <summary>
        /// 构造一个 <see cref="TypeParameterMappingCollection"/>。
        /// </summary>
        /// <param name="collection">给定的类型参数映射键值对集合。</param>
        public TypeParameterMappingCollection(IEnumerable<KeyValuePair<string, TypeParameterMapping>> collection)
            : base(collection)
        {
        }
#endif

        /// <summary>
        /// 构造一个 <see cref="TypeParameterMappingCollection"/>。
        /// </summary>
        /// <param name="serializationInfo">给定的 <see cref="SerializationInfo"/>。</param>
        /// <param name="streamingContext">给定的 <see cref="StreamingContext"/>。</param>
        protected TypeParameterMappingCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }


        /// <summary>
        /// 获取指定索引处的类型参数映射。
        /// </summary>
        /// <param name="index">给定的索引。</param>
        /// <returns>返回 <see cref="TypeParameterMapping"/>。</returns>
        public TypeParameterMapping this[int index]
            => this.ElementAt(index).Value;


        /// <summary>
        /// 尝试从值集合中查找类型定义并添加类型参数映射。
        /// </summary>
        /// <param name="typeDefinition">给定要查找的类型定义。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual bool TryFindTypeDefinitionFromValuesAndAddMapping(Type typeDefinition)
        {
            typeDefinition.NotNull(nameof(typeDefinition));

            if (typeDefinition.IsInterface)
            {
                foreach (var value in Values)
                {
                    if (value.ArgumentType.IsImplementedInterfaceType(typeDefinition, out var resultType))
                    {
                        AddMapping(resultType);
                        return true;
                    }
                }
            }
            else
            {
                foreach (var value in Values)
                {
                    if (value.ArgumentType.IsImplementedBaseType(typeDefinition, out var resultType))
                    {
                        AddMapping(resultType);
                        return true;
                    }
                }
            }

            return false;

            // AddMapping
            void AddMapping(Type resultType)
            {
                var parameterTypes = typeDefinition.GetTypeInfo().GenericTypeParameters;

                for (var i = 0; i < parameterTypes.Length; i++)
                {
                    Add(parameterTypes[i].Name,
                        new TypeParameterMapping(parameterTypes[i], resultType.GenericTypeArguments[i]));
                }
            }
        }

    }
}
