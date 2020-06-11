#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core.Proxies
{
    /// <summary>
    /// 抽象依赖代理。
    /// </summary>
    /// <typeparam name="TInterface">指定的接口类型。</typeparam>
    public abstract class AbstractDependencyProxy<TInterface> : DispatchProxy, IDependencyProxy<TInterface>
        where TInterface : class
    {
        private TInterface _source;


        /// <summary>
        /// 构造一个抽象代理。
        /// </summary>
        /// <param name="source">给定的源实例（可选）。</param>
        /// <param name="dependency">给定的 <see cref="IInvokeDependency{TSource}"/>（可选）。</param>
        /// <param name="validation"></param>
        protected AbstractDependencyProxy(TInterface source = null,
            IInvokeDependency<TInterface> dependency = null,
            IInvokeValidation validation = null)
        {
            if (source.IsNotNull())
                Source = source;

            Dependency = dependency ?? new InvokeDependency<TInterface>();
            Validation = validation ?? new InvokeValidation();
        }


        /// <summary>
        /// 调用依赖。
        /// </summary>
        /// <value>返回 <see cref="IInvokeDependency{TInterface}"/>。</value>
        public IInvokeDependency<TInterface> Dependency { get; }

        /// <summary>
        /// 调用验证。
        /// </summary>
        /// <value>返回 <see cref="IInvokeValidation"/>。</value>
        public IInvokeValidation Validation { get; }

        /// <summary>
        /// 源实例。
        /// </summary>
        public TInterface Source
        {
            get => _source.NotNull(nameof(Source));
            set => _source = value.NotNull(nameof(value));
        }


        /// <summary>
        /// 调用方法。
        /// </summary>
        /// <param name="targetMethod">给定的 <see cref="MethodInfo"/>。</param>
        /// <param name="args">给定的参数数组。</param>
        /// <returns>返回调用结果。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var (preActionKeys, postActionKeys) = Dependency.FindKeys(targetMethod);

            if (preActionKeys.IsNotEmpty())
            {
                foreach (var key in preActionKeys)
                {
                    if (!Dependency.TryGetPreActions(key, out IEnumerable<Action<TInterface>> preActions))
                        continue;

                    preActions.ForEach(action => action.Invoke(Source));
                }
            }

            var keyMember = ExtractInvokeDependencyKeyMember(targetMethod, preActionKeys, postActionKeys);
            if (keyMember.IsNotNull())
            {
                if (!Validation.Validate(keyMember, args, out string errorMessage))
                    throw new ValidationException(errorMessage);
            }

            var result = targetMethod.Invoke(Source, args);

            if (postActionKeys.IsNotEmpty())
            {
                foreach (var key in postActionKeys)
                {
                    if (!Dependency.TryGetPostActions(key, out IEnumerable<Action<TInterface, object>> postActions))
                        continue;

                    postActions.ForEach(action => action.Invoke(Source, result));
                }
            }

            return result;
        }

        /// <summary>
        /// 提取调用依赖键成员信息。
        /// </summary>
        /// <param name="targetMethod">给定的 <see cref="MethodInfo"/>。</param>
        /// <param name="preActionKeys">给定的前置 <see cref="IReadOnlyList{InvokeDependencyKey}"/>。</param>
        /// <param name="postActionKeys">给定的后置 <see cref="IReadOnlyList{InvokeDependencyKey}"/>。</param>
        /// <returns>返回 <see cref="MemberInfo"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual MemberInfo ExtractInvokeDependencyKeyMember(MethodInfo targetMethod,
            IReadOnlyList<InvokeDependencyKey> preActionKeys,
            IReadOnlyList<InvokeDependencyKey> postActionKeys)
        {
            var keyName = ExtractInvokeDependencyKeyName(preActionKeys, postActionKeys);
            if (keyName.IsEmpty())
                return null;

            return targetMethod.DeclaringType.GetMembers().SingleOrDefault(member => member.Name == keyName);
        }

        /// <summary>
        /// 提取调用依赖键名。
        /// </summary>
        /// <param name="preActionKeys">给定的前置 <see cref="IReadOnlyList{InvokeDependencyKey}"/>。</param>
        /// <param name="postActionKeys">给定的后置 <see cref="IReadOnlyList{InvokeDependencyKey}"/>。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string ExtractInvokeDependencyKeyName(IReadOnlyList<InvokeDependencyKey> preActionKeys,
            IReadOnlyList<InvokeDependencyKey> postActionKeys)
        {
            var hasPreActionKeys = preActionKeys.IsNotEmpty();
            var hasPostActionKeys = postActionKeys.IsNotEmpty();

            if (!hasPreActionKeys && !hasPostActionKeys)
                return null;

            var names = new List<string>();

            if (hasPreActionKeys)
                names.AddRange(preActionKeys.Select(key => key.Name));

            if (hasPostActionKeys)
                names.AddRange(postActionKeys.Select(key => key.Name));

            return names.Distinct().FirstOrDefault();
        }

    }
}
