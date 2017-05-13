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
using System.Linq;

namespace Librame.Data.Repositories
{
    /// <summary>
    /// 仓库入口接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IRepositoryEntry<T>
        where T : class
    {
        /// <summary>
        /// 实体绑定接口。
        /// </summary>
        IEntityBinding<T> Binding { get; }


        /// <summary>
        /// 准备查询动作。
        /// </summary>
        /// <param name="action">给定的查询动作。</param>
        void Ready(Action<IQueryable<T>> action);

        /// <summary>
        /// 准备查询工厂方法。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="factory">给定的工厂方法。</param>
        /// <returns>返回值实例。</returns>
        TValue Ready<TValue>(Func<IQueryable<T>, TValue> factory);
    }
}
