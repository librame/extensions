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
    /// 帐户描述符接口。
    /// </summary>
    public interface IAccountDescriptor : IEntityAutomapping
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 密码。
        /// </summary>
        string Passwd { get; }

        /// <summary>
        /// 状态。
        /// </summary>
        AccountStatus Status { get; }
    }
}
