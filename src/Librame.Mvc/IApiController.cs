#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Data;
using System.Net.Http;

namespace System.Web.Http
{
    /// <summary>
    /// API 控制器接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TId">指定的主键类型。</typeparam>
    public interface IApiController<T, TId>
        where T : class
        where TId : struct
    {
        /// <summary>
        /// 数据仓库。
        /// </summary>
        IRepository<T> Repository { get; }

        
        /// <summary>
        /// 获取指定主键的类型实例。
        /// </summary>
        /// <example>
        /// GET api/values/5
        /// </example>
        /// <param name="id">给定的主键。</param>
        /// <returns>返回类型实例。</returns>
        HttpResponseMessage Get(TId id);
    }
}
