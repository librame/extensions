#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Librame.Container
{
    using Utility;

    /// <summary>
    /// 默认容器适配器。
    /// </summary>
    public class DefaultContainerAdapter : AbstractContainerAdapter, IContainerAdapter
    {
        /// <summary>
        /// 当前 Unity 容器。
        /// </summary>
        protected readonly IUnityContainer CurrentUnityContainer = null;

        /// <summary>
        /// 构造一个 <see cref="DefaultContainerAdapter"/> 默认实例。
        /// </summary>
        public DefaultContainerAdapter()
        {
            InitializeAdapter();

            if (ReferenceEquals(CurrentUnityContainer, null))
                CurrentUnityContainer = (IUnityContainer)BuildContainer();
        }

        /// <summary>
        /// 初始化适配器。
        /// </summary>
        protected virtual void InitializeAdapter()
        {
            // 导出配置文件
            ExportConfigDirectory("Unity.config");
        }


        /// <summary>
        /// 当前容器对象。
        /// </summary>
        public virtual object CurrentContainer
        {
            get { return CurrentUnityContainer; }
        }


        /// <summary>
        /// 建立容器。
        /// </summary>
        /// <returns>返回容器对象。</returns>
        public virtual object BuildContainer()
        {
            // 初始创建容器实例
            var container = CreateContainer();

            // 配置容器
            container = (IUnityContainer)ConfigureContainer(container);

            // 纳入服务定位器统一管理
            var serviceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
            
            return container;
        }

        /// <summary>
        /// 创建容器。
        /// </summary>
        /// <returns>返回 <see cref="IUnityContainer"/>。</returns>
        protected virtual IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        /// <summary>
        /// 配置容器。
        /// </summary>
        /// <param name="container">给定的容器对象。</param>
        /// <returns>返回配置后的容器对象。</returns>
        protected virtual object ConfigureContainer(object container)
        {
            var configFileMap = GetContainerConfigFileMap();

            // 如果配置文件不存在，则直接返回
            if (!File.Exists(configFileMap.ExeConfigFilename))
                return container;

            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            try
            {
                var section = (UnityConfigurationSection)config.GetSection("unity");

                return section.Configure((IUnityContainer)container, typeof(LibrameArchitecture).Name);
            }
            catch (InvalidOperationException ioe)
            {
                throw ioe;
            }
        }

        /// <summary>
        /// 获取容器配置文件映射。
        /// </summary>
        /// <returns>返回 <see cref="ExeConfigurationFileMap"/>。</returns>
        protected virtual ExeConfigurationFileMap GetContainerConfigFileMap()
        {
            var configFilename = AdapterConfigDirectory.AppendPath("Unity.config");

            return new ExeConfigurationFileMap() { ExeConfigFilename = configFilename };
        }


        /// <summary>
        /// 尝试执行。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回值。</returns>
        protected TValue TryProcess<TValue>(Func<IUnityContainer, TValue> factory)
        {
            factory.NotNull(nameof(factory));

            try
            {
                return factory.Invoke(CurrentUnityContainer);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);

                throw ex;
            }
        }


        /// <summary>
        /// 增强实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="existing">给定已存在的类型实例。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T BuildUp<T>(T existing, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return CurrentUnityContainer.BuildUp(existing, resolverOverrides);
            });
        }
        /// <summary>
        /// 增强实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="existing">给定已存在的类型实例。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T BuildUp<T>(T existing, string name, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return CurrentUnityContainer.BuildUp(existing, name, resolverOverrides);
            });
        }

        /// <summary>
        /// 增强实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="existing">给定已存在的类型实例。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        public virtual object BuildUp(Type t, object existing, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return CurrentUnityContainer.BuildUp(t, existing, resolverOverrides);
            });
        }
        /// <summary>
        /// 增强实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="existing">给定已存在的类型实例。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        public virtual object BuildUp(Type t, object existing, string name, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return CurrentUnityContainer.BuildUp(t, existing, name, resolverOverrides);
            });
        }


        /// <summary>
        /// 是否已注册。
        /// </summary>
        /// <param name="typeToCheck">给定的检查类型。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsRegistered(Type typeToCheck)
        {
            return TryProcess(c => c.IsRegistered(typeToCheck));
        }
        /// <summary>
        /// 是否已注册。
        /// </summary>
        /// <param name="typeToCheck">给定的检查类型。</param>
        /// <param name="nameToCheck">给定的检查键名。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsRegistered(Type typeToCheck, string nameToCheck)
        {
            return TryProcess(c => c.IsRegistered(typeToCheck, nameToCheck));
        }

        /// <summary>
        /// 是否已注册。
        /// </summary>
        /// <typeparam name="T">指定的检查类型。</typeparam>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsRegistered<T>()
        {
            return TryProcess(c => c.IsRegistered<T>());
        }
        /// <summary>
        /// 是否已注册。
        /// </summary>
        /// <typeparam name="T">指定的检查类型。</typeparam>
        /// <param name="nameToCheck">给定的检查键名。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsRegistered<T>(string nameToCheck)
        {
            return TryProcess(c => c.IsRegistered<T>(nameToCheck));
        }


        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter RegisterInstance(Type t, object instance)
        {
            return TryProcess(c =>
            {
                c.RegisterInstance(t, instance);

                return this;
            });
        }
        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter RegisterInstance(Type t, string name, object instance)
        {
            return TryProcess(c =>
            {
                c.RegisterInstance(t, name, instance);

                return this;
            });
        }

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <typeparam name="TInterface">指定的注册接口类型。</typeparam>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter RegisterInstance<TInterface>(TInterface instance)
        {
            return TryProcess(c =>
            {
                c.RegisterInstance(instance);

                return this;
            });
        }
        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <typeparam name="TInterface">指定的注册接口类型。</typeparam>
        /// <param name="name">给定的键名。</param>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter RegisterInstance<TInterface>(string name, TInterface instance)
        {
            return TryProcess(c =>
            {
                c.RegisterInstance(name, instance);

                return this;
            });
        }


        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <typeparam name="T">指定的注册类型。</typeparam>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter Register<T>(params object[] parameters)
        {
            return TryProcess(c =>
            {
                var injectionMembers = ArrayUtility.As<object, InjectionMember>(parameters);

                c.RegisterType<T>(injectionMembers);

                return this;
            });
        }
        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <typeparam name="T">指定的注册类型。</typeparam>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter Register<T>(string name, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var injectionMembers = ArrayUtility.As<object, InjectionMember>(parameters);

                c.RegisterType<T>(name, injectionMembers);

                return this;
            });
        }

        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <param name="t">给定的注册类型。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter Register(Type t, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var injectionMembers = ArrayUtility.As<object, InjectionMember>(parameters);

                c.RegisterType(t, injectionMembers);

                return this;
            });
        }
        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <param name="t">给定的注册类型。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter Register(Type t, string name, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var injectionMembers = ArrayUtility.As<object, InjectionMember>(parameters);

                c.RegisterType(t, name, injectionMembers);

                return this;
            });
        }


        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <typeparam name="TFrom">指定的来源类型。</typeparam>
        /// <typeparam name="TTo">指定的目标类型。</typeparam>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter Register<TFrom, TTo>(params object[] parameters)
            where TTo : TFrom
        {
            return TryProcess(c =>
            {
                var injectionMembers = ArrayUtility.As<object, InjectionMember>(parameters);

                c.RegisterType<TFrom, TTo>(injectionMembers);

                return this;
            });
        }
        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <typeparam name="TFrom">指定的来源类型。</typeparam>
        /// <typeparam name="TTo">指定的目标类型。</typeparam>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter Register<TFrom, TTo>(string name, params object[] parameters)
            where TTo : TFrom
        {
            return TryProcess(c =>
            {
                var injectionMembers = ArrayUtility.As<object, InjectionMember>(parameters);

                c.RegisterType<TFrom, TTo>(name, injectionMembers);

                return this;
            });
        }

        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <param name="from">给定的来源类型。</param>
        /// <param name="to">给定的目标类型。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter Register(Type from, Type to, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var injectionMembers = ArrayUtility.As<object, InjectionMember>(parameters);

                c.RegisterType(from, to, injectionMembers);

                return this;
            });
        }
        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <param name="from">给定的来源类型。</param>
        /// <param name="to">给定的目标类型。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        public virtual IContainerAdapter Register(Type from, Type to, string name, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var injectionMembers = ArrayUtility.As<object, InjectionMember>(parameters);

                c.RegisterType(from, to, name, injectionMembers);

                return this;
            });
        }


        /// <summary>
        /// 解析类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T Resolve<T>(params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return c.Resolve<T>(resolverOverrides);
            });
        }
        /// <summary>
        /// 解析类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        public virtual T Resolve<T>(string name, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return c.Resolve<T>(name, resolverOverrides);
            });
        }

        /// <summary>
        /// 解析类型实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        public virtual object Resolve(Type t, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return c.Resolve(t, resolverOverrides);
            });
        }
        /// <summary>
        /// 解析类型实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        public virtual object Resolve(Type t, string name, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return c.Resolve(t, name, resolverOverrides);
            });
        }


        /// <summary>
        /// 解析类型多次注册实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例集合。</returns>
        public virtual IEnumerable<T> ResolveAll<T>(params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return c.ResolveAll<T>(resolverOverrides);
            });
        }

        /// <summary>
        /// 解析类型多次注册实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例集合。</returns>
        public virtual IEnumerable<object> ResolveAll(Type t, params object[] parameters)
        {
            return TryProcess(c =>
            {
                var resolverOverrides = ArrayUtility.As<object, ResolverOverride>(parameters);

                return c.ResolveAll(t, resolverOverrides);
            });
        }


        /// <summary>
        /// 释放容器适配器资源。
        /// </summary>
        public override void Dispose()
        {
            CurrentUnityContainer.Dispose();
        }

    }
}
