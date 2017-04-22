#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.OperationLog
{
    using Adaptation;

    /// <summary>
    /// 操作日志适配器接口。
    /// </summary>
    public interface IOperationLogAdapter : IAdapter
    {
        /// <summary>
        /// 获取操作日志工厂。
        /// </summary>
        IOperateLogFactory Factory { get; }
    }
}
