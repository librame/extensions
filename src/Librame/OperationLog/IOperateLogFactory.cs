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
    /// 操作日志工厂接口。
    /// </summary>
    public interface IOperateLogFactory : IInstanceFactory<IOperateLogDescriptor>
    {
    }
}
