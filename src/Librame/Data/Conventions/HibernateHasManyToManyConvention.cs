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
    /// NHibernate 多对多映射约定。
    /// </summary>
    public class HibernateHasManyToManyConvention : IHasManyToManyConvention
    {
        /// <summary>  
        /// 应用约定。  
        /// </summary>  
        /// <param name="instance">给定的 <see cref="IManyToManyCollectionInstance"/>。</param>
        public virtual void Apply(IManyToManyCollectionInstance instance)
        {
            // 指定表名的生成规则，将表名按字符排序较小的一方表名在前方，表名较大的放到后面  
            instance.Table(instance.EntityType.Name.ToString() + instance.ChildType.Name.ToString());

            // 关联列的一方的命名方式  
            instance.Key.Column(instance.EntityType.Name.ToString() + HibernateIdConvention.COLUMN_NAME);

            // 关系列的另一方的命名方式  
            instance.Relationship.Column(instance.ChildType.Name.ToString() + HibernateIdConvention.COLUMN_NAME);
        }

    }

}
