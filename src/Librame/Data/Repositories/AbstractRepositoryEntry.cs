#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Linq;

namespace Librame.Data.Repositories
{
    using Utility;

    /// <summary>
    /// 抽象仓库入口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public abstract class AbstractRepositoryEntry<T> : LibrameBase<EntityRepositoryEntry<T>>, IRepositoryEntry<T>
        where T : class
    {
        /// <summary>
        /// 实体绑定接口。
        /// </summary>
        public IEntityBinding<T> Binding
            => SingletonManager.Regist<IEntityBinding<T>>(key => new EntityBinding<T>());


        /// <summary>
        /// 准备查询动作。
        /// </summary>
        /// <param name="action">给定的查询动作。</param>
        public abstract void Ready(Action<IQueryable<T>> action);

        /// <summary>
        /// 准备查询工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回值实例。</returns>
        public abstract TValue Ready<TValue>(Func<IQueryable<T>, TValue> factory);


        #region IRepositoryEntry<T>

        void IRepositoryEntry<T>.Ready(Action<IQueryable<T>> action)
        {
            Ready(action);
        }

        TValue IRepositoryEntry<T>.Ready<TValue>(Func<IQueryable<T>, TValue> factory)
        {
            return Ready(factory);
        }

        #endregion

    }
}
