#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 请求接口。
    /// </summary>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    public interface IRequest<out TResponse> : IRequest
    {
    }


    /// <summary>
    /// 请求接口。
    /// </summary>
    public interface IRequest
    {
    }
}
