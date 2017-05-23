#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Common.Logging;
using Librame.Authorization;
using Librame.Data;

namespace System.Web.Http
{
    /// <summary>
    /// API 控制器接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IApiController<T>
        where T : class
    {
        /// <summary>
        /// 数据仓库。
        /// </summary>
        IRepository<T> Repository { get; }

        /// <summary>
        /// 当前日志接口。
        /// </summary>
        ILog Logger { get; }

        /// <summary>
        /// 当前认证适配器接口。
        /// </summary>
        IAuthorizeAdapter Adapter { get; }

        /// <summary>
        /// 当前请求的 HTTP 上下文。
        /// </summary>
        HttpContextBase HttpContext { get; }
    }
}
