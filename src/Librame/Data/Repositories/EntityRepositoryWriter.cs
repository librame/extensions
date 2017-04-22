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
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Librame.Data.Repositories
{
    using Providers;

    /// <summary>
    /// EntityFramework 仓库写入器。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public abstract class EntityRepositoryWriter<T> : EntityRepositoryReadiness<T>, IRepositoryWriter<T>
        where T : class
    {
        /// <summary>
        /// 构造一个 <see cref="HibernateRepository{T}"/> 实例。
        /// </summary>
        /// <param name="provider">给定的 <see cref="EntityProvider"/>。</param>
        public EntityRepositoryWriter(EntityProvider provider)
            : base(provider)
        {
        }
        
        /// <summary>
        /// 移除被占用的实体。
        /// </summary>
        /// <param name="entity">给定的实体。</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool RemoveHolding(T entity)
        {
            return ReadyProvider((p, s) => RemoveHolding(p, entity));
        }
        private bool RemoveHolding(IObjectContextAdapter contextAdapter, T entity)
        {
            var objContext = contextAdapter.ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);
            if (exists)
            {
                // 移除此对象
                objContext.Detach(foundEntity);
            }

            return exists;
        }


        /// <summary>
        /// 保存。
        /// </summary>
        public virtual void Save()
        {
            ReadyProvider((p, s) => p.SaveChanges());
        }


        /// <summary>
        /// 创建类型实例。
        /// </summary>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T Create(T entity, bool syncDatabase = true)
        {
            if (ReferenceEquals(entity, null))
                return default(T);

            // Auto SaveChanges
            return ReadyProvider((p, s) =>
            {
                entity = s.Add(entity);

                if (syncDatabase)
                    p.SaveChanges();

                return entity;
            });
        }


        /// <summary>
        /// 更新类型实例。
        /// </summary>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T Update(T entity, bool syncDatabase = true)
        {
            if (ReferenceEquals(entity, null))
                return default(T);

            return ReadyProvider((p, s) =>
            {
                // 移除被占用的实体
                RemoveHolding(p, entity);

                // 附加实体
                s.Attach(entity);
                p.Entry(entity).State = EntityState.Modified;

                if (syncDatabase)
                    p.SaveChanges();

                return entity;
            });
        }

        /// <summary>
        /// 通过更新获取单条符合指定查询表达式的类型实例。
        /// </summary>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询第一条数据）。</param>
        /// <param name="updateAction">给定更新此类型实例的方法。</param>
        /// <returns>返回对象。</returns>
        public virtual T GetByUpdate(Expression<Func<T, bool>> predicate = null, Action<T> updateAction = null)
        {
            return ReadyProvider((p, s) =>
            {
                var entity = default(T);

                // 支持筛选
                if (!ReferenceEquals(predicate, null))
                    entity = s.FirstOrDefault(predicate);
                else
                    entity = s.FirstOrDefault();

                if (!ReferenceEquals(updateAction, null))
                {
                    // 调用更新方法
                    updateAction.Invoke(entity);

                    // 同步数据库
                    p.SaveChanges();
                }

                return entity;
            });
        }


        /// <summary>
        /// 删除类型实例。
        /// </summary>
        /// <param name="entity">给定的类型实例。</param>
        /// <param name="syncDatabase">是否同步到数据库（可选；默认同步）。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T Delete(T entity, bool syncDatabase = true)
        {
            if (ReferenceEquals(entity, null))
                return default(T);

            return ReadyProvider((p, s) =>
            {
                s.Remove(entity);

                if (syncDatabase)
                    p.SaveChanges();

                return entity;
            });
        }


        #region IRepositoryWriter<T>

        void IRepositoryWriter<T>.Save()
        {
            Save();
        }


        T IRepositoryWriter<T>.Create(T entity, bool syncDatabase)
        {
            return Create(entity, syncDatabase);
        }


        T IRepositoryWriter<T>.Update(T entity, bool syncDatabase)
        {
            return Update(entity, syncDatabase);
        }

        T IRepositoryWriter<T>.GetByUpdate(Expression<Func<T, bool>> predicate, Action<T> updateAction)
        {
            return GetByUpdate(predicate, updateAction);
        }


        T IRepositoryWriter<T>.Delete(T entity, bool syncDatabase)
        {
            return Delete(entity, syncDatabase);
        }

        #endregion

    }
}
