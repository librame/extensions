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
    public interface IBaseStore : IBaseStore<BaseAudit, BaseAuditProperty, BaseTenant>
    {
    }


    /// <summary>
    /// 基础存储接口。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface IBaseStore<TAudit, TAuditProperty, TTenant> : IAuditStore<TAudit, TAuditProperty>, ITenantStore<TTenant>
        where TAudit : class
        where TAuditProperty : class
        where TTenant : class
    {
    }
}
