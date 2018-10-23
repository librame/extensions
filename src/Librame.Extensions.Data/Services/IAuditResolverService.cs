#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    using Services;

    /// <summary>
    /// 审计解析器服务接口。
    /// </summary>
    public interface IAuditResolverService : IService
    {
        /// <summary>
        /// 创建者名工厂方法。
        /// </summary>
        Func<IEnumerable<PropertyEntry>, string> CreatorNameFactory { get; set; }

        /// <summary>
        /// 创建时间属性工厂方法。
        /// </summary>
        Func<IEnumerable<PropertyEntry>, PropertyEntry> CreateTimePropertyFactory { get; set; }

        /// <summary>
        /// 更新者名工厂方法。
        /// </summary>
        Func<IEnumerable<PropertyEntry>, string> UpdatorNameFactory { get; set; }

        /// <summary>
        /// 更新时间属性工厂方法。
        /// </summary>
        Func<IEnumerable<PropertyEntry>, PropertyEntry> UpdateTimePropertyFactory { get; set; }


        /// <summary>
        /// 获取列表。
        /// </summary>
        /// <param name="changeTracker">给定的 <see cref="ChangeTracker"/>。</param>
        /// <param name="entityStates">给定的用于审计的实体状态集合（可选；默认审计添加、修改、删除状态）。</param>
        /// <returns>返回 <see cref="IList{AuditEntry}"/>。</returns>
        IList<Audit> GetAudits(ChangeTracker changeTracker, params EntityState[] entityStates);
    }
}
