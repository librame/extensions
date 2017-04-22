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
    using Data.Descriptors;

    /// <summary>
    /// 操作日志管道接口。
    /// </summary>
    public interface IOperateLogProvider : IAdapterManagerReference
    {
        /// <summary>
        /// 添加操作日志。
        /// </summary>
        /// <param name="message">给定的消息。</param>
        /// <param name="tableName">给定的数据表名。</param>
        /// <param name="operateUrl">给定的操作链接。</param>
        /// <param name="operateTime">给定的操作时间。</param>
        /// <returns>返回 <see cref="IOperateLogDescriptor"/>。</returns>
        IOperateLogDescriptor AddOperateLog(string message, string tableName, string operateUrl, DateTime operateTime);

        /// <summary>
        /// 添加操作日志。
        /// </summary>
        /// <param name="operateLog">给定的操作日志。</param>
        /// <returns>返回 <see cref="IOperateLogDescriptor"/>。</returns>
        IOperateLogDescriptor AddOperateLog(IOperateLogDescriptor operateLog);
    }
}
