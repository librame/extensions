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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 可选配置静态扩展。
    /// </summary>
    public static class OptionalExtensions
    {
        /// <summary>
        /// 配置可选。
        /// </summary>
        /// <typeparam name="TOptional">指定的可选配置类型。</typeparam>
        /// <param name="configureAction">给定的配置动作。</param>
        /// <returns>返回 <typeparamref name="TOptional"/>。</returns>
        public static TOptional Configure<TOptional>(this Action<TOptional> configureAction)
            where TOptional : class, IOptional
            => configureAction.Configure<TOptional, TOptional>(out _);

        /// <summary>
        /// 配置可选。
        /// </summary>
        /// <typeparam name="TOptional">指定的可选配置类型。</typeparam>
        /// <param name="configureAction">给定的配置动作。</param>
        /// <param name="optional">输出 <typeparamref name="TOptional"/>。</param>
        /// <returns>返回 <typeparamref name="TOptional"/>。</returns>
        public static TOptional Configure<TOptional>(this Action<TOptional> configureAction, out TOptional optional)
            where TOptional : class, IOptional
            => configureAction.Configure<TOptional, TOptional>(out optional);


        /// <summary>
        /// 配置可选。
        /// </summary>
        /// <typeparam name="TOptional">指定的可选配置类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <param name="configureAction">给定的配置动作。</param>
        /// <returns>返回 <typeparamref name="TOptional"/>。</returns>
        public static TOptional Configure<TOptional, TImplementation>(this Action<TOptional> configureAction)
            where TOptional : class, IOptional
            where TImplementation : TOptional
            => configureAction.Configure<TOptional, TImplementation>(out _);

        /// <summary>
        /// 配置可选。
        /// </summary>
        /// <typeparam name="TOptional">指定的可选配置类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <param name="configureAction">给定的配置动作。</param>
        /// <param name="optional">输出 <typeparamref name="TOptional"/>。</param>
        /// <returns>返回 <typeparamref name="TOptional"/>。</returns>
        public static TOptional Configure<TOptional, TImplementation>(this Action<TOptional> configureAction, out TOptional optional)
            where TOptional : class, IOptional
            where TImplementation : TOptional
        {
            optional = typeof(TImplementation).EnsureCreate<TOptional>();
            configureAction?.Invoke(optional);

            return optional;
        }

    }
}
