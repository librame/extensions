#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Utility
{
    /// <summary>
    /// URI 请求方法。
    /// </summary>
    public enum UriRequestMethod
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// GET。
        /// </summary>
        [Description("GET")]
        Get = 1,

        /// <summary>
        /// POST。
        /// </summary>
        [Description("POST")]
        Post = 2
    }
}
