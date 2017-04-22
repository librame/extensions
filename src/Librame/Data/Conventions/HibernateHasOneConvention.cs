#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Librame.Data.Conventions
{
    /// <summary>
    /// Hibernate 一对一映射约定。
    /// </summary>
    public class HibernateHasOneConvention : IHasOneConvention
    {
        /// <summary>  
        /// 应用约定。  
        /// </summary>  
        /// <param name="instance">给定的 <see cref="IOneToOneInstance"/>。</param>
        public virtual void Apply(IOneToOneInstance instance)
        {
            // 指定外键列的列名规则  
            instance.ForeignKey(instance.EntityType.Name + HibernateIdConvention.COLUMN_NAME);

            // 指定主表对从表的操作  
            instance.Cascade.SaveUpdate();

            instance.LazyLoad();
        }

    }
}
