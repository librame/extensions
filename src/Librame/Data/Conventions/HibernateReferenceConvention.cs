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
    /// NHibernate 引用映射约定。
    /// </summary>
    public class HibernateReferenceConvention : IReferenceConvention
    {
        /// <summary>
        /// 应用约定。
        /// </summary>
        /// <param name="instance">给定的 <see cref="IManyToManyInstance"/>。</param>
        public virtual void Apply(IManyToOneInstance instance)
        {
            instance.Column(instance.Name + HibernateIdConvention.COLUMN_NAME);
            instance.LazyLoad();
        }

    }
}
