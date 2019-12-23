#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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
    public interface IDictionaryStringLocalizer<TResource> : IStringLocalizer<TResource>
    {
    }
}
