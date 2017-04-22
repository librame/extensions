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
    /// 操作日志适配器。
    /// </summary>
    public class DefaultOperationLogAdapter : AbstarctOperationLogAdapter, IOperationLogAdapter
    {
        /// <summary>
        /// 获取操作日志工厂。
        /// </summary>
        public virtual IOperateLogFactory Factory
        {
            get { return SingletonManager.Resolve<IOperateLogFactory>(key => new OperateLogFactory()); }
        }

    }
}
