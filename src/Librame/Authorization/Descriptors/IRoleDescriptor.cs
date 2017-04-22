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
    /// 角色描述符接口。
    /// </summary>
    public interface IRoleDescriptor : IEntityAutomapping
    {
        /// <summary>
        /// 角色名称。
        /// </summary>
        string Name { get; }
    }
}
