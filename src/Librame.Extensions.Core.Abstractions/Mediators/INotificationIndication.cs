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
    /// 通知标示接口（用于标记、约束实例）。
    /// </summary>
    [SuppressMessage("Design", "CA1040:避免使用空接口")]
    public interface INotificationIndication : IIndication
    {
    }
}
