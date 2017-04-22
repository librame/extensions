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
    using Utility;

    /// <summary>
    /// 操作日志工厂。
    /// </summary>
    public class OperateLogFactory : AbstractInstanceFactory<IOperateLogDescriptor>, IOperateLogFactory
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractInstanceFactory{T}"/> 用户。
        /// </summary>
        public OperateLogFactory()
            : base()
        {
        }

    }
}
