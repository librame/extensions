#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration.Edm;

namespace Librame.Data.Conventions
{
    using Utility;

    ///// <summary>
    ///// EntityFramework 表名映射约定。
    ///// </summary>
    //public class EntityTableNameConvention : PluralizingTableNameConvention
    //{
    //    /// <summary>
    //    /// 应用约定。
    //    /// </summary>
    //    /// <param name="item">给定的实体类型。</param>
    //    /// <param name="model">给定的数据模型。</param>
    //    public override void Apply(EntityType item, DbModel model)
    //    {
    //        item.GuardNull(nameof(item));
    //        model.GuardNull(nameof(model));
            
    //        var entitySet = model.StoreModel.GetEntitySet(item);
    //        entitySet.Table = WordHelper.Pluralize(item.Name);
    //    }

    //}
}
