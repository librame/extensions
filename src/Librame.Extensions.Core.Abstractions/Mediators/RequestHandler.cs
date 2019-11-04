#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 表示要在中间件执行的下一个异步请求任务。
    /// </summary>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    /// <returns>返回一个包含 <typeparamref name="TResponse"/> 的异步操作。</returns>
    public delegate Task<TResponse> RequestHandler<TResponse>();
}
