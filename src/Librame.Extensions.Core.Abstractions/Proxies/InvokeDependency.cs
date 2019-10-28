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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    class InvokeDependency<TInterface> : AbstractDisposable, IInvokeDependency<TInterface>
        where TInterface : class
    {
        private readonly ConcurrentDictionary<InvokeDependencyKey, IEnumerable<Action<TInterface>>> _preActions = null;
        private readonly ConcurrentDictionary<InvokeDependencyKey, IEnumerable<Action<TInterface, object>>> _postActions = null;


        public InvokeDependency()
        {
            _preActions = new ConcurrentDictionary<InvokeDependencyKey, IEnumerable<Action<TInterface>>>();
            _postActions = new ConcurrentDictionary<InvokeDependencyKey, IEnumerable<Action<TInterface, object>>>();
        }


        protected override void DisposeCore()
        {
            _preActions.Clear();
            _postActions.Clear();
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


        public void AddPreActions(InvokeDependencyKey key, params Action<TInterface>[] preActions)
            => _preActions.AddOrUpdate(key, preActions, (key, value) => value.Concat(preActions));

        public void AddPostActions(InvokeDependencyKey key, params Action<TInterface, object>[] postActions)
            => _postActions.AddOrUpdate(key, postActions, (key, value) => value.Concat(postActions));


        public bool TryGetPreActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface>> preActions)
            => _preActions.TryGetValue(key, out preActions);

        public bool TryGetPostActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface, object>> postActions)
            => _postActions.TryGetValue(key, out postActions);


        public bool TryRemovePreActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface>> preActions)
            => _preActions.TryRemove(key, out preActions);

        public bool TryRemovePostActions(InvokeDependencyKey key, out IEnumerable<Action<TInterface, object>> postActions)
            => _postActions.TryRemove(key, out postActions);
    }
}
