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
    /// 租户变化处理程序。
    /// </summary>
    public class TenantChangeHandler : ITenantChangeHandler
    {
        /// <summary>
        /// 设置租户。
        /// </summary>
        public Tenant SetTenant { private get; set; }


        /// <summary>
        /// 处理实体。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="DataBuilderOptions"/>。</param>
        public void Process(EntityEntry entry, DataBuilderOptions builderOptions)
        {
            if (!builderOptions.AuditEnabled) return;

            if (entry.Entity is ITenantId && SetTenant.IsNotDefault())
            {
                var property = entry.Entity?.GetType().GetProperty("TenantId");

                if (property.IsNotDefault())
                    property.SetValue(entry.Entity, SetTenant.Id);
            }
        }

    }
}
