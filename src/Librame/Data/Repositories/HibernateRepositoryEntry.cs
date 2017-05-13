#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace Librame.Data.Repositories
{
    using Providers;
    using Utility;

    /// <summary>
    /// NHibernate 仓库入口准备。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class HibernateRepositoryEntry<T> : AbstractRepositoryEntry<T>
        where T : class
    {
        private readonly HibernateProvider _provider = null;
        private readonly ISessionFactory _sessionFactory = null;
        private readonly ISession _session = null;

        /// <summary>
        /// 构造一个 <see cref="HibernateRepository{T}"/> 实例。
        /// </summary>
        /// <param name="provider">给定的 <see cref="HibernateProvider"/>。</param>
        protected HibernateRepositoryEntry(HibernateProvider provider)
        {
            _provider = provider.NotNull(nameof(provider));
            _sessionFactory = _provider.BuildSessionFactory();
            _session = _sessionFactory.OpenSession();
        }

        /// <summary>
        /// 准备管道动作方法。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        protected virtual void ReadyProvider(Action<HibernateProvider, ISessionFactory, ISession> action)
        {
            action.NotNull(nameof(action));

            try
            {
                action.Invoke(_provider, _sessionFactory, _session);
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);

                //throw ex;
            }
        }

        /// <summary>
        /// 准备管道工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回值类型实例。</returns>
        protected virtual TValue ReadyProvider<TValue>(Func<HibernateProvider, ISessionFactory, ISession, TValue> factory)
        {
            factory.NotNull(nameof(factory));

            try
            {
                return factory.Invoke(_provider, _sessionFactory, _session);
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);

                return default(TValue);
                //throw ex;
            }
        }


        /// <summary>
        /// 准备查询动作。
        /// </summary>
        /// <param name="action">给定的查询动作。</param>
        public override void Ready(Action<IQueryable<T>> action)
        {
            action.NotNull(nameof(action));

            ReadyProvider((p, f, s) => action.Invoke(s.Query<T>().Cacheable()));
        }

        /// <summary>
        /// 准备查询工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回值实例。</returns>
        public override TValue Ready<TValue>(Func<IQueryable<T>, TValue> factory)
        {
            factory.NotNull(nameof(factory));

            return ReadyProvider((p, f, s) =>
            {
                return factory.Invoke(s.Query<T>().Cacheable());
            });
        }

    }
}
