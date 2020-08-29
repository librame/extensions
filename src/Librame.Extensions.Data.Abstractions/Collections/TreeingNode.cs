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
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Collections
{
    using Core.Identifiers;

    /// <summary>
    /// 树形节点。
    /// </summary>
    /// <typeparam name="T">指定的树形元素类型。</typeparam>
    /// <typeparam name="TId">指定的树形元素标识类型。</typeparam>
    [NotMapped]
    public class TreeingNode<T, TId> : IParentIdentifier<TId>, IEquatable<TreeingNode<T, TId>>
        where T : IParentIdentifier<TId>
        where TId : IEquatable<TId>
    {
        private IList<TreeingNode<T, TId>> _children;


        /// <summary>
        /// 构造一个 <see cref="TreeingNode{T, TId}"/>。
        /// </summary>
        /// <param name="item">给定的项。</param>
        /// <param name="depthLevel">给定的深度等级。</param>
        /// <param name="children">给定的子节点列表。</param>
        public TreeingNode(T item, int depthLevel = 0, IList<TreeingNode<T, TId>> children = null)
        {
            Item = item;
            DepthLevel = depthLevel;

            _children = children ?? new List<TreeingNode<T, TId>>();
        }


        /// <summary>
        /// 节点项。
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// 深度等级。
        /// </summary>
        public int DepthLevel { get; }


        /// <summary>
        /// 子节点列表。
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public IList<TreeingNode<T, TId>> Children
        {
            get
            {
                return _children;
            }
            set
            {
                if (value.IsEmpty())
                    value = new List<TreeingNode<T, TId>>();

                _children = value;
            }
        }


        /// <summary>
        /// 获取节点项的标识。
        /// </summary>
        public TId Id
        {
            get { return Item.Id; }
            set { Item.Id = value; }
        }

        /// <summary>
        /// 获取节点项的父标识。
        /// </summary>
        public TId ParentId
        {
            get { return Item.ParentId; }
            set { Item.ParentId = value; }
        }


        /// <summary>
        /// 标识类型。
        /// </summary>
        public Type IdType
            => Item.IdType;


        /// <summary>
        /// 获取对象标识。
        /// </summary>
        /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
        public virtual object GetObjectId()
            => Item.GetObjectId();

        /// <summary>
        /// 异步获取对象标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectIdAsync(CancellationToken cancellationToken)
            => Item.GetObjectIdAsync(cancellationToken);


        /// <summary>
        /// 获取对象标识。
        /// </summary>
        /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
        public virtual object GetObjectParentId()
            => Item.GetObjectParentId();

        /// <summary>
        /// 异步获取对象标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectParentIdAsync(CancellationToken cancellationToken)
            => Item.GetObjectParentIdAsync(cancellationToken);


        /// <summary>
        /// 设置对象标识。
        /// </summary>
        /// <param name="newId">给定的新对象标识。</param>
        /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
        public virtual object SetObjectId(object newId)
            => Item.SetObjectId(newId);

        /// <summary>
        /// 异步设置对象标识。
        /// </summary>
        /// <param name="newId">给定的新对象标识。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectIdAsync(object newId,
            CancellationToken cancellationToken = default)
            => Item.SetObjectIdAsync(newId, cancellationToken);


        /// <summary>
        /// 设置对象标识。
        /// </summary>
        /// <param name="newParentId">给定的新对象标识。</param>
        /// <returns>返回标识（兼容各种引用与值类型标识）。</returns>
        public virtual object SetObjectParentId(object newParentId)
            => Item.SetObjectParentId(newParentId);

        /// <summary>
        /// 异步设置对象标识。
        /// </summary>
        /// <param name="newParentId">给定的新对象标识。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectParentIdAsync(object newParentId,
            CancellationToken cancellationToken = default)
            => Item.SetObjectParentIdAsync(newParentId, cancellationToken);


        /// <summary>
        /// 是否包含指定标识的子节点。
        /// </summary>
        /// <param name="childId">给定的子节点编号。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool ContainsChild(TId childId)
            => ContainsChild(childId, out _);

        /// <summary>
        /// 是否包含指定标识的子节点。
        /// </summary>
        /// <param name="childId">给定的子节点信号。</param>
        /// <param name="child">输出当前子节点。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool ContainsChild(TId childId, out TreeingNode<T, TId> child)
        {
            child = GetChild(childId);
            return child.IsNotNull();
        }


        /// <summary>
        /// 查找指定编号的子节点。
        /// </summary>
        /// <param name="childId">给定的子节点编号。</param>
        /// <returns>返回当前子节点。</returns>
        public virtual TreeingNode<T, TId> GetChild(TId childId)
        {
            if (Children.IsEmpty())
                return null;

            return Children.FirstOrDefault(c => Id.Equals(childId));
        }

        /// <summary>
        /// 查询指定父编号的子孙节点列表。
        /// </summary>
        /// <param name="parentId">给定的父编号。</param>
        /// <returns>返回树形节点列表。</returns>
        public virtual IList<TreeingNode<T, TId>> GetParentChildren(TId parentId)
        {
            if (Children.IsEmpty())
                return null;

            return Children.Where(p => p.ParentId.Equals(parentId)).ToList();
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="TreeingNode{T, TId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual bool Equals(TreeingNode<T, TId> other)
        {
            other.NotNull(nameof(other));
            return Id.Equals(other.Id) && ParentId.Equals(other.ParentId);
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的 <see cref="TreeingNode{T, TId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is TreeingNode<T, TId> other && Equals(other);


        /// <summary>
        /// 已重载。
        /// </summary>
        /// <returns>返回此实例的哈希代码。</returns>
        public override int GetHashCode()
            => Id.GetHashCode() ^ ParentId.GetHashCode();


        /// <summary>
        /// 已重载。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => ToString(node => node.Item.ToString());

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <param name="toStringFactory">给定的转换方法。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual string ToString(Func<TreeingNode<T, TId>, string> toStringFactory)
        {
            if (Children.IsEmpty())
                return string.Empty;

            toStringFactory.NotNull(nameof(toStringFactory));

            var sb = new StringBuilder();

            // Current Node
            sb.Append(toStringFactory.Invoke(this));
            sb.Append(';');

            // Children Nodes
            int i = 0;
            foreach (var child in Children)
            {
                // 链式转换可能存在的子孙节点
                sb.Append(child.ToString(toStringFactory));

                if (i != Children.Count - 1)
                    sb.Append(';');

                i++;
            }

            return sb.ToString();
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="TreeingNode{T, TId}"/>。</param>
        /// <param name="b">给定的 <see cref="TreeingNode{T, TId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(TreeingNode<T, TId> a, TreeingNode<T, TId> b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="TreeingNode{T, TId}"/>。</param>
        /// <param name="b">给定的 <see cref="TreeingNode{T, TId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(TreeingNode<T, TId> a, TreeingNode<T, TId> b)
            => !(a?.Equals(b)).Value;

    }
}
