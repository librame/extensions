#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// 树状节点。
    /// </summary>
    /// <typeparam name="T">指定的树状元素类型。</typeparam>
    /// <typeparam name="TId">指定的树状元素标识类型。</typeparam>
    [NotMapped]
    public class TreeingNode<T, TId> : IParentId<TId>
        where T : IParentId<TId>
        where TId : IEquatable<TId>
    {
        private IList<TreeingNode<T, TId>> _children = null;


        /// <summary>
        /// 编号属性。
        /// </summary>
        protected readonly PropertyInfo IdProperty = null;

        /// <summary>
        /// 父编号属性。
        /// </summary>
        protected readonly PropertyInfo ParentIdProperty = null;


        /// <summary>
        /// 节点项。
        /// </summary>
        public T Item { get; }

        /// <summary>
        /// 深度等级。
        /// </summary>
        public int DepthLevel { get; }


        /// <summary>
        /// 构造一个 <see cref="TreeingNode{T, TId}"/> 实例。
        /// </summary>
        public TreeingNode()
            : this(default)
        {
        }
        /// <summary>
        /// 构造一个 <see cref="TreeingNode{T, TId}"/> 实例。
        /// </summary>
        /// <param name="item">给定的项。</param>
        /// <param name="depthLevel">给定的深度等级。</param>
        /// <param name="children">给定的子节点列表。</param>
        public TreeingNode(T item, int depthLevel = 0, IList<TreeingNode<T, TId>> children = null)
        {
            Item = item;
            DepthLevel = depthLevel;

            _children = children ?? new List<TreeingNode<T, TId>>();

            // 绑定编号、父编号属性信息
            if (ReferenceEquals(IdProperty, null))
            {
                var properties = typeof(T).GetRuntimeProperties();

                var id = nameof(Id);
                var parentId = nameof(ParentId);

                IdProperty = properties.FirstOrDefault(pi => pi.Name == id);
                ParentIdProperty = properties.FirstOrDefault(pi => pi.Name == parentId);
            }
        }


        /// <summary>
        /// 子节点列表。
        /// </summary>
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
        /// 获取当前节点项的编号。
        /// </summary>
        public virtual TId Id
        {
            get { return (TId)IdProperty.GetValue(Item, null); }
            set { IdProperty.SetValue(Item, value); }
        }

        /// <summary>
        /// 获取当前节点项的父编号。
        /// </summary>
        public virtual TId ParentId
        {
            get { return (TId)ParentIdProperty.GetValue(Item, null); }
            set { ParentIdProperty.SetValue(Item, value); }
        }


        /// <summary>
        /// 是否包含子孙节点。
        /// </summary>
        public bool HasChildren => (Children.Count > 0);


        /// <summary>
        /// 是否包含指定编号的子节点。
        /// </summary>
        /// <param name="childId">给定的子节点编号。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool ContainsChild(int childId)
        {
            TreeingNode<T, TId> child = null;

            return ContainsChild(childId, out child);
        }
        /// <summary>
        /// 是否包含指定编号的子节点。
        /// </summary>
        /// <param name="childId">给定的子节点信号。</param>
        /// <param name="child">输出当前子节点。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool ContainsChild(int childId, out TreeingNode<T, TId> child)
        {
            child = GetChild(childId);

            return (child.IsNotDefault());
        }


        /// <summary>
        /// 查找指定编号的子节点。
        /// </summary>
        /// <param name="childId">给定的子节点编号。</param>
        /// <returns>返回当前子节点。</returns>
        public virtual TreeingNode<T, TId> GetChild(int childId)
        {
            if (HasChildren)
            {
                foreach (var c in Children)
                {
                    // 断定当前子节点编号
                    if (c.Id.Equals(childId))
                        return c;
                }
            }

            return null;
        }


        /// <summary>
        /// 查询指定父编号的子孙节点列表。
        /// </summary>
        /// <param name="parentId">给定的父编号。</param>
        /// <returns>返回树形节点列表。</returns>
        public virtual IList<TreeingNode<T, TId>> GetChildren(int parentId)
        {
            if (HasChildren)
            {
                var allChilds = new List<TreeingNode<T, TId>>();

                foreach (var c in Children)
                {
                    // 断定当前子节点编号
                    if (c.ParentId.Equals(parentId) && c.HasChildren)
                    {
                        // 添加子节点的子孙节点集合
                        allChilds.AddRange(c.Children);
                    }
                }

                return allChilds;
            }

            return null;
        }


        /// <summary>
        /// 已重载。
        /// </summary>
        /// <param name="obj">给定的 <see cref="TreeingNode{T, TId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is TreeingNode<T, TId>)
            {
                var node = obj as TreeingNode<T, TId>;
                return Id.Equals(node.Id) && ParentId.Equals(node.ParentId);
            }

            return false;
        }


        /// <summary>
        /// 已重载。
        /// </summary>
        /// <returns>返回此实例的哈希代码。</returns>
        public override int GetHashCode()
        {
            return ParentId.GetHashCode() ^ ParentId.GetHashCode();
        }


        /// <summary>
        /// 已重载。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return ToString(null);
        }
        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <param name="toStringFactory">给定的转换方法。</param>
        /// <returns>返回字符串。</returns>
        public virtual string ToString(Func<T, string> toStringFactory)
        {
            if (toStringFactory.IsDefault())
                toStringFactory = obj => obj.ToString(); // obj.AsPairsString();

            var sb = new StringBuilder();

            sb.Append(toStringFactory.Invoke(Item));

            if (Children.Count < 1)
                return sb.ToString();

            sb.Append(";");

            int i = 0;
            foreach (var child in Children)
            {
                // 链式转换可能存在的子孙节点
                sb.Append(child.ToString(toStringFactory));

                if (i != Children.Count - 1)
                    sb.Append(";");
            }

            return sb.ToString();
        }

    }
}
