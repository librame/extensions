#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Data.Repositories
{
    using Providers;

    /// <summary>
    /// EntityFramework 仓库。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class EntityRepository<T> : EntityRepositoryReader<T>, IRepository<T>
        where T : class
    {
        /// <summary>
        /// 构造一个 <see cref="EntityRepository{T}"/> 实例。
        /// </summary>
        /// <param name="dataSettings">给定的数据首选项。</param>
        public EntityRepository(DataSettings dataSettings)
            : this(DataHelper.BuildProvider<EntityProvider>(dataSettings,
                settings => settings.EntityProviderTypeString))
        {
        }
        /// <summary>
        /// 构造一个 <see cref="HibernateRepository{T}"/> 实例。
        /// </summary>
        /// <param name="provider">给定的 <see cref="EntityProvider"/>。</param>
        public EntityRepository(EntityProvider provider)
            : base(provider)
        {
        }

    }
}
