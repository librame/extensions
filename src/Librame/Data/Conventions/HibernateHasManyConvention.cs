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
    /// NHibernate 一对多映射约定。
    /// </summary>
    public class HibernateHasManyConvention : IHasManyConvention
    {
        /// <summary>  
        /// 应用约定。  
        /// </summary>  
        /// <param name="instance">给定的 <see cref="IOneToManyCollectionInstance"/>。</param>
        public virtual void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + HibernateIdConvention.COLUMN_NAME);

            instance.Cascade.AllDeleteOrphan();

            instance.LazyLoad();
        }

    }
}
