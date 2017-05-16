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

namespace Librame.OperationLog
{
    using Adaptation;

    /// <summary>
    /// 操作日志管道基类。
    /// </summary>
    public class OperateLogProviderBase : AbstractAdapterCollectionManager, IOperateLogProvider
    {
        /// <summary>
        /// 构造一个 <see cref="OperateLogProviderBase"/> 实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public OperateLogProviderBase(IAdapterCollection adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 添加操作日志。
        /// </summary>
        /// <param name="message">给定的消息。</param>
        /// <param name="tableName">给定的数据表名。</param>
        /// <param name="operateUrl">给定的操作链接。</param>
        /// <param name="operateTime">给定的操作时间。</param>
        /// <returns>返回 <see cref="IOperateLogDescriptor"/>。</returns>
        public virtual IOperateLogDescriptor AddOperateLog(string message, string tableName, string operateUrl, DateTime operateTime)
        {
            return AddOperateLog(new OperateLogDescriptor(message, tableName, operateUrl, operateTime));
        }

        /// <summary>
        /// 添加操作日志。
        /// </summary>
        /// <param name="operateLog">给定的操作日志。</param>
        /// <returns>返回 <see cref="IOperateLogDescriptor"/>。</returns>
        public virtual IOperateLogDescriptor AddOperateLog(IOperateLogDescriptor operateLog)
        {
            return operateLog;
        }

    }
}
