#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
using System;

namespace Librame.Extensions.Core.Dependencies
{
    using Serializers;

    /// <summary>
    /// 抽象依赖。
    /// </summary>
    public abstract class AbstractDependency : IDependency
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDependency"/>。
        /// </summary>
        /// <param name="name">给定的依赖名称。</param>
        protected AbstractDependency(string name)
        {
            Name = name.NotEmpty(nameof(name));
        }


        /// <summary>
        /// 依赖名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 依赖类型。
        /// </summary>
        public SerializableObject<Type> Type
            => SerializableHelper.CreateType(GetType());

        /// <summary>
        /// 依赖配置。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public IConfiguration Configuration { get; set; }


        /// <summary>
        /// 建立名称。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回字符串。</returns>
        public static string BuildName<T>()
            where T : class
            => BuildName<T>(out _);

        /// <summary>
        /// 建立名称。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="type">输出类型。</param>
        /// <returns>返回字符串。</returns>
        public static string BuildName<T>(out Type type)
            where T : class
        {
            type = typeof(T);
            return BuildName(type);
        }

        /// <summary>
        /// 建立名称。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string BuildName(Type type)
            => type.NotNull(nameof(type)).Name;
    }
}
