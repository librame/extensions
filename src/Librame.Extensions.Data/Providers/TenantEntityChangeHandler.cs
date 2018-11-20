#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 租户实体变化处理程序。
    /// </summary>
    public class TenantEntityChangeHandler : IEntityChangeHandler
    {
        private string _tenantId = string.Empty;


        public Tenant Tenant { get; } = new Tenant();
        

        public void Process(EntityEntry entry)
        {
            if (entry.Entity is ITenantId)
            {
                var property = entry.Entity?.GetType().GetProperty("TenantId");

                if (property.IsNotDefault())
                    property.SetValue(entry.Entity, _tenantId);
            }
        }

    }
}
