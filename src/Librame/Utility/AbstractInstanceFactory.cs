#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Web;

namespace Librame.Utility
{
    /// <summary>
    /// 抽象实例工厂。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public abstract class AbstractInstanceFactory<T> : KeyBuilder, IInstanceFactory<T>
    {
        /// <summary>
        /// 默认实例名。
        /// </summary>
        private readonly string _defaultName = BuildKey<T>();


        /// <summary>
        /// 构造一个 <see cref="AbstractInstanceFactory{T}"/> 实例。
        /// </summary>
        public AbstractInstanceFactory()
            : this(null)
        {
        }
        /// <summary>
        /// 构造一个 <see cref="AbstractInstanceFactory{T}"/> 实例。
        /// </summary>
        /// <param name="defaultName">给定的默认实例名。</param>
        protected AbstractInstanceFactory(string defaultName)
        {
            if (!string.IsNullOrEmpty(defaultName))
                _defaultName = defaultName;
        }


        #region Get

        /// <summary>
        /// 获取默认实例名的实例。
        /// </summary>
        /// <returns>返回类型实例。</returns>
        public virtual T GetInstance()
        {
            return GetInstance(_defaultName);
        }

        /// <summary>
        /// 获取实例。
        /// </summary>
        /// <param name="name">给定的实例名。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T GetInstance(string name)
        {
            name.NotNull(nameof(name));

            var instance = default(T);
            
            if (PathUtility.IsWebEnvironment)
                instance = GetInstanceByWeb(name);

            if (ReferenceEquals(instance, null))
                instance = GetInstanceByThread(name);

            return instance;
        }

        /// <summary>
        /// 从 Thread 环境中获取实例。
        /// </summary>
        /// <param name="name">给定的实例名。</param>
        /// <returns>返回类型实例。</returns>
        protected virtual T GetInstanceByThread(string name)
        {
            return default(T);
        }

        /// <summary>
        /// 从 Web 环境中的缓存对象获取实例。
        /// </summary>
        /// <param name="name">给定的实例名。</param>
        /// <returns>返回类型实例。</returns>
        protected virtual T GetInstanceByWeb(string name)
        {
            return (T)HttpContext.Current?.Cache[name];
        }

        #endregion


        #region Set

        /// <summary>
        /// 设置默认实例名的实例。
        /// </summary>
        /// <param name="instance">给定的类型实例。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T SetInstance(T instance)
        {
            return SetInstance(_defaultName, instance);
        }

        /// <summary>
        /// 设置实例。
        /// </summary>
        /// <param name="name">给定的实例名。</param>
        /// <param name="instance">给定的类型实例。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T SetInstance(string name, T instance)
        {
            name.NotNull(nameof(name));

            SetInstanceByThread(name, instance);

            if (PathUtility.IsWebEnvironment)
                SetInstanceByWeb(name, instance);

            return instance;
        }

        /// <summary>
        /// 向 Thread 环境中设置实例。
        /// </summary>
        /// <param name="name">给定的实例名。</param>
        /// <param name="instance">给定的类型实例。</param>
        /// <returns>返回类型实例。</returns>
        protected virtual T SetInstanceByThread(string name, T instance)
        {
            return instance;
        }

        /// <summary>
        /// 向 Web 环境中的缓存对象设置实例。
        /// </summary>
        /// <param name="name">给定的实例名。</param>
        /// <param name="instance">给定的类型实例。</param>
        /// <returns>返回类型实例。</returns>
        protected virtual T SetInstanceByWeb(string name, T instance)
        {
            var context = HttpContext.Current;

            if (!ReferenceEquals(context, null))
                context.Cache[name] = instance;

            return instance;
        }

        #endregion

    }
}
