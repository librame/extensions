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
using System.Linq.Expressions;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 调用依赖接口。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    public interface IInvokeDependency<TInterface> : IDisposable
        where TInterface : class
    {
        /// <summary>
        /// 查询键名集合。
        /// </summary>
        /// <param name="methodInfo">给定的 <see cref="MethodInfo"/>。</param>
        /// <returns>返回包含前置与后置 <see cref="IReadOnlyList{DependencyActionKey}"/> 的元组。</returns>
        (IReadOnlyList<InvokeDependencyKey> PreActionKeys, IReadOnlyList<InvokeDependencyKey> PostActionKeys) FindKeys(MethodInfo methodInfo);


        #region AddActions

        /// <summary>
        /// 添加前置动作集合。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="kind">给定的 <see cref="InvokeDependencyKind"/>。</param>
        /// <param name="preActions">给定的前置动作数组。</param>
        void AddPreActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, params Action<TInterface>[] preActions);

        /// <summary>
        /// 添加前置动作集合。
        /// </summary>
        /// <param name="key">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <param name="preActions">给定的前置动作数组。</param>
        void AddPreActions(InvokeDependencyKey key, params Action<TInterface>[] preActions);


        /// <summary>
        /// 添加后置动作集合。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="kind">给定的 <see cref="InvokeDependencyKind"/>。</param>
        /// <param name="postActions">给定的后置动作数组。</param>
        void AddPostActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, params Action<TInterface, object>[] postActions);

        /// <summary>
        /// 添加后置动作集合。
        /// </summary>
        /// <param name="key">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <param name="postActions">给定的后置动作数组。</param>
        void AddPostActions(InvokeDependencyKey key, params Action<TInterface, object>[] postActions);

        #endregion


        #region TryGetActions

        /// <summary>
        /// 尝试获取前置动作集合。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="kind">给定的 <see cref="InvokeDependencyKind"/>。</param>
        /// <param name="preActions">输出得到的前置动作数组。</param>
        /// <returns>返回布尔值。</returns>
        bool TryGetPreActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, out IEnumerable<Action<TInterface>> preActions);

        /// <summary>
        /// 尝试获取前置动作集合。
        /// </summary>
        /// <param name="key">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <param name="preActions">输出得到的前置动作数组。</param>
        /// <returns>返回布尔值。</returns>
        bool TryGetPreActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface>> preActions);


        /// <summary>
        /// 尝试获取后置动作集合。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="kind">给定的 <see cref="InvokeDependencyKind"/>。</param>
        /// <param name="postActions">输出得到的后置动作集合。</param>
        /// <returns>返回布尔值。</returns>
        bool TryGetPostActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, out IEnumerable<Action<TInterface, object>> postActions);

        /// <summary>
        /// 尝试获取后置动作集合。
        /// </summary>
        /// <param name="key">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <param name="postActions">输出得到的后置动作集合。</param>
        /// <returns>返回布尔值。</returns>
        bool TryGetPostActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface, object>> postActions);

        #endregion


        #region TryRemoveActions

        /// <summary>
        /// 尝试移除前置动作集合。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="kind">给定的 <see cref="InvokeDependencyKind"/>。</param>
        /// <param name="preActions">输出移除的前置动作集合。</param>
        /// <returns>返回布尔值。</returns>
        bool TryRemovePreActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, out IEnumerable<Action<TInterface>> preActions);

        /// <summary>
        /// 尝试移除前置动作集合。
        /// </summary>
        /// <param name="key">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <param name="preActions">输出移除的前置动作集合。</param>
        /// <returns>返回布尔值。</returns>
        bool TryRemovePreActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface>> preActions);


        /// <summary>
        /// 尝试移除后置动作集合。
        /// </summary>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="kind">给定的 <see cref="InvokeDependencyKind"/>。</param>
        /// <param name="postActions">输出移除的后置动作集合。</param>
        /// <returns>返回布尔值。</returns>
        bool TryRemovePostActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, out IEnumerable<Action<TInterface, object>> postActions);

        /// <summary>
        /// 尝试移除后置动作集合。
        /// </summary>
        /// <param name="key">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <param name="postActions">输出移除的后置动作集合。</param>
        /// <returns>返回布尔值。</returns>
        bool TryRemovePostActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface, object>> postActions);

        #endregion

    }
}
