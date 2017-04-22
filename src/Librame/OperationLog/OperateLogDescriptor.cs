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
using System.ComponentModel;

namespace Librame.OperationLog
{
    /// <summary>
    /// 操作日志描述符。
    /// </summary>
    [Serializable]
    public class OperateLogDescriptor : IOperateLogDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="OperateLogDescriptor"/> 实例。
        /// </summary>
        /// <param name="message">给定的消息。</param>
        /// <param name="tableName">给定的数据表名。</param>
        /// <param name="operateUrl">给定的操作链接。</param>
        /// <param name="operateTime">给定的操作时间。</param>
        public OperateLogDescriptor(string message, string tableName, string operateUrl, DateTime operateTime)
        {
            Message = message;
            TableName = tableName;
            OperateUrl = operateUrl;
            OperateTime = operateTime;
        }


        /// <summary>
        /// 操作时间。
        /// </summary>
        [DisplayName("操作时间")]
        public virtual DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作链接。
        /// </summary>
        [DisplayName("操作链接")]
        public virtual string OperateUrl { get; set; }

        /// <summary>
        /// 数据表名。
        /// </summary>
        [DisplayName("数据表名")]
        public virtual string TableName { get; set; }

        /// <summary>
        /// 消息内容。
        /// </summary>
        [DisplayName("消息内容")]
        public virtual string Message { get; set; }

    }
}
