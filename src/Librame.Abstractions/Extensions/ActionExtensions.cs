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

namespace Librame.Extensions
{
    /// <summary>
    /// 动作静态扩展。
    /// </summary>
    public static class ActionExtensions
    {

        /// <summary>
        /// 重新配置实例。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="action">给定的配置动作。</param>
        /// <returns>返回源实例。</returns>
        public static TSource Newly<TSource>(this Action<TSource> action)
            where TSource : class, new()
        {
            var source = new TSource();
            action?.Invoke(source);

            return source;
        }

    }
}
