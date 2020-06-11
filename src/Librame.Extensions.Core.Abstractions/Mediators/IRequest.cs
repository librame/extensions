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
    [SuppressMessage("Design", "CA1040:避免使用空接口")]
    public interface IRequest<out TResponse> : IRequestIndication
    {
    }
}
