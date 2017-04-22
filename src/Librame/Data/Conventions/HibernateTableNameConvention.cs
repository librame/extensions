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
    using Utility;

    /// <summary>
    /// NHibernate 表名映射约定。
    /// </summary>
    public class HibernateTableNameConvention : IClassConvention
    {
        /// <summary>
        /// 应用约定。
        /// </summary>
        /// <param name="instance">给定的 <see cref="IClassInstance"/>。</param>
        public virtual void Apply(IClassInstance instance)
        {
            // 变成复数形式
            instance.Table(WordHelper.AsPluralize(instance.EntityType.Name));
        }

    }
}
