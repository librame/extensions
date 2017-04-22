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
    /// NHibernate 标识映射约定。
    /// </summary>
    public class HibernateIdConvention : IIdConvention
    {
        /// <summary>
        /// 列名。
        /// </summary>
        internal const string COLUMN_NAME = "Id";

        /// <summary>  
        /// 应用约定。
        /// </summary>  
        /// <param name="instance">给定的 <see cref="IIdentityInstance"/>。</param>
        public virtual void Apply(IIdentityInstance instance)
        {
            instance.Column(COLUMN_NAME);
            instance.GeneratedBy.Native();
        }

    }
}
