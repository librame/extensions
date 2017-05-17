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
        /// 编号。
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 应用编号。
        /// </summary>
        int AppId { get; }

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


        /// <summary>
        /// 重置密码。
        /// </summary>
        /// <param name="newPasswd">给定要重置的新密码。</param>
        void ResetPasswd(string newPasswd = null);
    }
}
