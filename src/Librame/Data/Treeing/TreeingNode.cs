#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Data.Descriptors;
using Librame.Utility;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// 树形节点。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TId">指定的主键类型。</typeparam>
    public class TreeingNode<T, TId> : IEnumerable<TreeingNode<T, TId>>, IParentIdDescriptor<TId>
        where T : IParentIdDescriptor<TId>
        where TId : struct
    {
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
        /// 子节点列表。
        /// </summary>
        public IList<TreeingNode<T, TId>> Childs { get; set; }


        /// <summary>
        /// 构造一个 <see cref="TreeingNode{T, TId}"/> 实例。
        /// </summary>
        public TreeingNode()
            : this(default(T))
        {
        }
        /// <summary>
        /// 构造一个 <see cref="TreeingNode{T, TId}"/> 实例。
        /// </summary>
        /// <param name="item">给定的项。</param>
        /// <param name="childs">给定的子节点列表。</param>
        public TreeingNode(T item, IList<TreeingNode<T, TId>> childs = null)
        {
            Item = item;
            Childs = childs ?? new List<TreeingNode<T, TId>>();

            // 绑定编号、父编号属性信息
            if (ReferenceEquals(IdProperty, null))
            {
                var properties = typeof(T).GetProperties();

                var id = nameof(Id);
                var parentId = nameof(ParentId);

                IdProperty = properties.FirstOrDefault(pi => pi.Name == id);
                ParentIdProperty = properties.FirstOrDefault(pi => pi.Name == parentId);
            }
        }


        /// <summary>
        /// 获取当前节点项的编号。
        /// </summary>
        public virtual TId Id
        {
            get { return (TId)IdProperty.GetValue(Item, null); }
        }

        /// <summary>
        /// 获取当前节点项的父编号。
        /// </summary>
        public virtual TId ParentId
        {
            get { return (TId)ParentIdProperty.GetValue(Item, null); }
        }


        /// <summary>
        /// 是否包含子孙节点。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public virtual bool ContainsChilds()
        {
            return (Childs.Count > 0);
        }


        /// <summary>
        /// 是否包含指定编号的子节点。
        /// </summary>
        /// <param name="childId">给定的子节点信号。</param>
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

            return (!ReferenceEquals(child, null));
        }


        /// <summary>
        /// 查找指定编号的子节点。
        /// </summary>
        /// <param name="childId">给定的子节点编号。</param>
        /// <returns>返回当前子节点。</returns>
        public virtual TreeingNode<T, TId> GetChild(int childId)
        {
            // 是否包含子孙节点
            if (ContainsChilds())
            {
                foreach (var c in Childs)
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
        public virtual IList<TreeingNode<T, TId>> GetChilds(int parentId)
        {
            // 是否包含子孙节点
            if (ContainsChilds())
            {
                var allChilds = new List<TreeingNode<T, TId>>();

                foreach (var c in Childs)
                {
                    // 断定当前子节点编号
                    if (c.ParentId.Equals(parentId) && c.ContainsChilds())
                    {
                        // 添加子节点的子孙节点集合
                        allChilds.AddRange(c.Childs);
                    }
                }

                return allChilds;
            }

            return null;
        }


        #region IEnumerable<T> Members

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>返回枚举数。</returns>
        public IEnumerator<TreeingNode<T, TId>> GetEnumerator()
        {
            if (ReferenceEquals(Childs, null) || Childs.Count < 1) return null;

            return Childs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


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
                return (Id.Equals(node.Id) && ParentId.Equals(node.ParentId));
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
            if (Childs.Count < 1) return string.Empty;

            if (ReferenceEquals(toStringFactory, null))
                toStringFactory = node => this.AsString();
            
            var sb = new StringBuilder();
            foreach (var child in Childs)
            {
                // 循环转换子节点
                if (!ReferenceEquals(child.Item, null))
                    sb.Append(toStringFactory(child.Item));

                // 循环转换可能存在的子孙节点
                sb.Append(child.ToString(toStringFactory));
            }

            return sb.ToString();
        }

    }
}
