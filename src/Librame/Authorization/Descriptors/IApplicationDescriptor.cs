#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Authorization.Descriptors
{
    using Data;

    /// <summary>
    /// 应用描述符接口。
    /// </summary>
    public interface IApplicationDescriptor : IEntityAutomapping
    {
        /// <summary>
        /// 授权编号。
        /// </summary>
        string AuthId { get; }
    }
}
