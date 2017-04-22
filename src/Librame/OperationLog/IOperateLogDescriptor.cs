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
    using Data;

    /// <summary>
    /// 操作日志描述符接口。
    /// </summary>
    public interface IOperateLogDescriptor : IEntityAutomapping
    {
        /// <summary>
        /// 操作时间。
        /// </summary>
        DateTime OperateTime { get; }

        /// <summary>
        /// 操作链接。
        /// </summary>
        string OperateUrl { get; }

        /// <summary>
        /// 表名。
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// 消息内容。
        /// </summary>
        string Message { get; }
    }
}
