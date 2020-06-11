#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Localization;

namespace Librame.Extensions.Core.Localizers
{
    /// <summary>
    /// 字典字符串定位器接口。
    /// </summary>
    /// <typeparam name="TResource">指定的资源类型。</typeparam>
    public interface IDictionaryStringLocalizer<out TResource> : IStringLocalizer<TResource>
    {
        /// <summary>
        /// 带有资源。
        /// </summary>
        /// <typeparam name="TNewResource">指定的新资源类型。</typeparam>
        /// <returns>返回 <see cref="IDictionaryStringLocalizer{TResource}"/>。</returns>
        IDictionaryStringLocalizer<TNewResource> WithResource<TNewResource>();
    }
}
