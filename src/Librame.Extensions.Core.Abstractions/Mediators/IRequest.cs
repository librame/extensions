#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Mediators
{
    /// <summary>
    /// 请求接口。
    /// </summary>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    public interface IRequest<out TResponse> : IRequest
    {
    }


    /// <summary>
    /// 请求接口。
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    public interface IRequest
    {
    }
}
