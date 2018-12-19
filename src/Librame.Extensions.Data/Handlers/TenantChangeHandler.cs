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
    using Builders;

    /// <summary>
    /// 租户变化处理程序。
    /// </summary>
    public class TenantChangeHandler : ITenantChangeHandler
    {
        /// <summary>
        /// 设置租户。
        /// </summary>
        public ITenant SetTenant { private get; set; }


        /// <summary>
        /// 处理实体。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="DataBuilderOptions"/>。</param>
        public void Process(EntityEntry entry, DataBuilderOptions builderOptions)
        {
            if (!builderOptions.AuditEnabled) return;

            if (entry.Entity is ITenantId entity && entity.IsNotDefault() && SetTenant.IsNotDefault())
            {
                // 获取租户标识属性
                var idProperty = SetTenant.GetType().GetProperty("Id");
                // 获取实体的关联租户标识属性
                var tenantIdProperty = entity.GetType().GetProperty("TenantId");

                // 更新当前实体的关联租户标识
                if (idProperty.IsNotDefault() && tenantIdProperty.IsNotDefault())
                    tenantIdProperty.SetValue(entity, idProperty.GetValue(SetTenant));
            }
        }
    }
}
