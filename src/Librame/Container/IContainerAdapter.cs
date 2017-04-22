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
using System.Collections.Generic;

namespace Librame.Container
{
    /// <summary>
    /// 容器适配器接口。
    /// </summary>
    public interface IContainerAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 当前容器对象。
        /// </summary>
        object CurrentContainer { get; }


        /// <summary>
        /// 建立容器。
        /// </summary>
        /// <returns>返回容器对象。</returns>
        object BuildContainer();


        /// <summary>
        /// 增强实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="existing">给定已存在的类型实例。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        T BuildUp<T>(T existing, params object[] parameters);
        /// <summary>
        /// 增强实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="existing">给定已存在的类型实例。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        T BuildUp<T>(T existing, string name, params object[] parameters);

        /// <summary>
        /// 增强实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="existing">给定已存在的类型实例。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        object BuildUp(Type t, object existing, params object[] parameters);
        /// <summary>
        /// 增强实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="existing">给定已存在的类型实例。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        object BuildUp(Type t, object existing, string name, params object[] parameters);


        /// <summary>
        /// 是否已注册。
        /// </summary>
        /// <param name="typeToCheck">给定的检查类型。</param>
        /// <returns>返回布尔值。</returns>
        bool IsRegistered(Type typeToCheck);
        /// <summary>
        /// 是否已注册。
        /// </summary>
        /// <param name="typeToCheck">给定的检查类型。</param>
        /// <param name="nameToCheck">给定的检查键名。</param>
        /// <returns>返回布尔值。</returns>
        bool IsRegistered(Type typeToCheck, string nameToCheck);

        /// <summary>
        /// 是否已注册。
        /// </summary>
        /// <typeparam name="T">指定的检查类型。</typeparam>
        /// <returns>返回布尔值。</returns>
        bool IsRegistered<T>();
        /// <summary>
        /// 是否已注册。
        /// </summary>
        /// <typeparam name="T">指定的检查类型。</typeparam>
        /// <param name="nameToCheck">给定的检查键名。</param>
        /// <returns>返回布尔值。</returns>
        bool IsRegistered<T>(string nameToCheck);


        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter RegisterInstance(Type t, object instance);
        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter RegisterInstance(Type t, string name, object instance);

        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <typeparam name="TInterface">指定的注册接口类型。</typeparam>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter RegisterInstance<TInterface>(TInterface instance);
        /// <summary>
        /// 注册实例。
        /// </summary>
        /// <typeparam name="TInterface">指定的注册接口类型。</typeparam>
        /// <param name="name">给定的键名。</param>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter RegisterInstance<TInterface>(string name, TInterface instance);


        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <typeparam name="T">指定的注册类型。</typeparam>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter Register<T>(params object[] parameters);
        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <typeparam name="T">指定的注册类型。</typeparam>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter Register<T>(string name, params object[] parameters);

        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <param name="t">给定的注册类型。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter Register(Type t, params object[] parameters);
        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <param name="t">给定的注册类型。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter Register(Type t, string name, params object[] parameters);


        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <typeparam name="TFrom">指定的来源类型。</typeparam>
        /// <typeparam name="TTo">指定的目标类型。</typeparam>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter Register<TFrom, TTo>(params object[] parameters) where TTo : TFrom;
        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <typeparam name="TFrom">指定的来源类型。</typeparam>
        /// <typeparam name="TTo">指定的目标类型。</typeparam>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter Register<TFrom, TTo>(string name, params object[] parameters) where TTo : TFrom;

        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <param name="from">给定的来源类型。</param>
        /// <param name="to">给定的目标类型。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter Register(Type from, Type to, params object[] parameters);
        /// <summary>
        /// 注册类型。
        /// </summary>
        /// <param name="from">给定的来源类型。</param>
        /// <param name="to">给定的目标类型。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的注入成员数组。</param>
        /// <returns>返回 <see cref="IContainerAdapter"/>。</returns>
        IContainerAdapter Register(Type from, Type to, string name, params object[] parameters);


        /// <summary>
        /// 解析类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        T Resolve<T>(params object[] parameters);
        /// <summary>
        /// 解析类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        T Resolve<T>(string name, params object[] parameters);

        /// <summary>
        /// 解析类型实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        object Resolve(Type t, params object[] parameters);
        /// <summary>
        /// 解析类型实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="name">给定的键名。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例。</returns>
        object Resolve(Type t, string name, params object[] parameters);


        /// <summary>
        /// 解析类型多次注册实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例集合。</returns>
        IEnumerable<T> ResolveAll<T>(params object[] parameters);
        /// <summary>
        /// 解析类型多次注册实例。
        /// </summary>
        /// <param name="t">给定的类型。</param>
        /// <param name="parameters">给定的解析器覆盖数组。</param>
        /// <returns>返回类型实例集合。</returns>
        IEnumerable<object> ResolveAll(Type t, params object[] parameters);

    }
}
