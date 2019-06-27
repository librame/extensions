#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 基础存储接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IBaseStore<TAccessor> : IBaseStore<TAccessor, BaseAudit, BaseTenant>
        where TAccessor : IAccessor
    {
    }


    /// <summary>
    /// 基础存储接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface IBaseStore<TAccessor, TAudit, TTenant> : IAuditStore<TAccessor, TAudit>, ITenantStore<TAccessor, TTenant>
        where TAudit : class
        where TTenant : class
        where TAccessor : IAccessor
    {
    }
}
