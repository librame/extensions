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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Librame.Extensions.Core.Proxies
{
    internal class InvokeDependency<TInterface> : IInvokeDependency<TInterface>
        where TInterface : class
    {
        private readonly ConcurrentDictionary<InvokeDependencyKey, IEnumerable<Action<TInterface>>> _preActions;
        private readonly ConcurrentDictionary<InvokeDependencyKey, IEnumerable<Action<TInterface, object>>> _postActions;


        public InvokeDependency()
        {
            _preActions = new ConcurrentDictionary<InvokeDependencyKey, IEnumerable<Action<TInterface>>>();
            _postActions = new ConcurrentDictionary<InvokeDependencyKey, IEnumerable<Action<TInterface, object>>>();
        }


        public (IReadOnlyList<InvokeDependencyKey> PreActionKeys, IReadOnlyList<InvokeDependencyKey> PostActionKeys) FindKeys(MethodInfo methodInfo)
        {
            var approxKeys = InvokeDependencyKey.ParseApproximateKeys(methodInfo);
            IEnumerable<InvokeDependencyKey> preActionKeys = null;

            if (_preActions.Keys.Count > 0)
                preActionKeys = approxKeys.Intersect(_preActions.Keys);

            if (_postActions.Keys.Count > 0)
                return (preActionKeys?.AsReadOnlyList(), approxKeys.Intersect(_postActions.Keys)?.AsReadOnlyList());

            return (preActionKeys?.AsReadOnlyList(), null);
        }


        public void AddPreActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, params Action<TInterface>[] preActions)
            => AddPreActions(new InvokeDependencyKey(propertyExpression.AsPropertyName(), kind), preActions);

        public void AddPreActions(InvokeDependencyKey key, params Action<TInterface>[] preActions)
            => _preActions.AddOrUpdate(key, preActions, (key, value) => value.Concat(preActions));


        public void AddPostActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, params Action<TInterface, object>[] postActions)
            => AddPostActions(new InvokeDependencyKey(propertyExpression.AsPropertyName(), kind), postActions);

        public void AddPostActions(InvokeDependencyKey key, params Action<TInterface, object>[] postActions)
            => _postActions.AddOrUpdate(key, postActions, (key, value) => value.Concat(postActions));


        public bool TryGetPreActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, out IEnumerable<Action<TInterface>> preActions)
            => TryGetPreActions(new InvokeDependencyKey(propertyExpression.AsPropertyName(), kind), out preActions);

        public bool TryGetPreActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface>> preActions)
            => _preActions.TryGetValue(key, out preActions);


        public bool TryGetPostActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, out IEnumerable<Action<TInterface, object>> postActions)
            => TryGetPostActions(new InvokeDependencyKey(propertyExpression.AsPropertyName(), kind), out postActions);

        public bool TryGetPostActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface, object>> postActions)
            => _postActions.TryGetValue(key, out postActions);


        public bool TryRemovePreActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, out IEnumerable<Action<TInterface>> preActions)
            => TryRemovePreActions(new InvokeDependencyKey(propertyExpression.AsPropertyName(), kind), out preActions);

        public bool TryRemovePreActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface>> preActions)
            => _preActions.TryRemove(key, out preActions);


        public bool TryRemovePostActions<TProperty>(Expression<Func<TInterface, TProperty>> propertyExpression,
            InvokeDependencyKind kind, out IEnumerable<Action<TInterface, object>> postActions)
            => TryRemovePostActions(new InvokeDependencyKey(propertyExpression.AsPropertyName(), kind), out postActions);

        public bool TryRemovePostActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface, object>> postActions)
            => _postActions.TryRemove(key, out postActions);
    }
}
