#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Librame.Extensions.Data
//{
//    using Core;

//    /// <summary>
//    /// 抽象访问器。
//    /// </summary>
//    public abstract class AbstractAccessor : AbstractDisposable, IAccessor
//    {
//        /// <summary>
//        /// 当前租户。
//        /// </summary>
//        public ITenant CurrentTenant { get; set; }


//        /// <summary>
//        /// 异步查询指定标识的实体。
//        /// </summary>
//        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
//        /// <param name="id">给定的标识。</param>
//        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
//        /// <returns>返回一个包含实体的异步操作。</returns>
//        public abstract Task<TEntity> QueryByIdAsync<TEntity>(object id,
//            CancellationToken cancellationToken = default)
//            where TEntity : class;


//        /// <summary>
//        /// 执行 SQL 命令。
//        /// </summary>
//        /// <param name="sql">给定的 SQL 语句。</param>
//        /// <param name="parameters">给定的参数集合。</param>
//        /// <returns>返回受影响的行数。</returns>
//        public abstract int ExecuteSqlCommand(string sql, params object[] parameters);

//        /// <summary>
//        /// 异步执行 SQL 命令。
//        /// </summary>
//        /// <param name="sql">给定的 SQL 语句。</param>
//        /// <param name="parameters">给定的参数集合。</param>
//        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
//        /// <returns>返回一个包含受影响行数的异步操作。</returns>
//        public abstract Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);


//        /// <summary>
//        /// 重载保存更改。
//        /// </summary>
//        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
//        /// <returns>返回受影响的行数。</returns>
//        public abstract int SaveChanges(bool acceptAllChangesOnSuccess);

//        /// <summary>
//        /// 重载异步保存更改。
//        /// </summary>
//        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
//        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
//        /// <returns>返回一个包含受影响行数的异步操作。</returns>
//        public abstract Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

//        /// <summary>
//        /// 尝试改变数据库连接。
//        /// </summary>
//        /// <param name="connectionStringFactory">给定改变数据库连接的工厂方法。</param>
//        /// <returns>返回是否切换的布尔值。</returns>
//        public abstract bool TryChangeDbConnection(Func<ITenant, string> connectionStringFactory);
//    }
//}
