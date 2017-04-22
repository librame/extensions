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
using System.Data.Entity;
using System.Linq;

namespace Librame.Data.Repositories
{
    using Providers;
    using Utility;

    /// <summary>
    /// EntityFramework 仓库准备。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class EntityRepositoryReadiness<T> : LibrameBase<EntityRepositoryReadiness<T>>, IRepositoryReadiness<T>
        where T : class
    {
        private readonly EntityProvider _provider = null;
        private readonly DbSet<T> _dbSet = null;

        /// <summary>
        /// 构造一个 <see cref="HibernateRepository{T}"/> 实例。
        /// </summary>
        /// <param name="provider">给定的 <see cref="EntityProvider"/>。</param>
        protected EntityRepositoryReadiness(EntityProvider provider)
        {
            _provider = provider.NotNull(nameof(provider));
            _dbSet = _provider.Set<T>();
        }

        /// <summary>
        /// 准备管道动作方法。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        protected virtual void ReadyProvider(Action<EntityProvider, DbSet<T>> action)
        {
            action.GuardNull(nameof(action));

            try
            {
                action.Invoke(_provider, _dbSet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.AsOrInnerMessage(), ex);

                throw ex; // 抛出致命异常
            }
        }

        /// <summary>
        /// 准备管道工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回值类型实例。</returns>
        protected virtual TValue ReadyProvider<TValue>(Func<EntityProvider, DbSet<T>, TValue> factory)
        {
            factory.GuardNull(nameof(factory));

            try
            {
                return factory.Invoke(_provider, _dbSet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.AsOrInnerMessage(), ex);

                //return default(TValue);
                throw ex; // 抛出致命异常
            }
        }


        /// <summary>
        /// 准备查询工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回值实例。</returns>
        public virtual TValue Ready<TValue>(Func<IQueryable<T>, TValue> factory)
        {
            factory.GuardNull(nameof(factory));

            return ReadyProvider((p, s) =>
            {
                return factory.Invoke(s);
            });
        }


        #region IRepositoryReadiness<T>

        TValue IRepositoryReadiness<T>.Ready<TValue>(Func<IQueryable<T>, TValue> factory)
        {
            return Ready(factory);
        }

        #endregion

    }
}
