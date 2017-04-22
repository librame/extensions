#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Practices.Unity.InterceptionExtension;

namespace Librame.OperationLog
{
    using Utility;

    /// <summary>
    /// 操作日志访问处理程序。
    /// </summary>
    public class OperateLogCallHandler : ICallHandler
    {
        private int _order = 0;


        /// <summary>
        /// 获取操作日志描述符。
        /// </summary>
        public IOperateLogDescriptor Descriptor { get; }

        /// <summary>
        /// 获取操作日志管道。
        /// </summary>
        public IOperateLogProvider Provider { get; }

        
        /// <summary>
        /// 构造一个 <see cref="OperateLogCallHandler"/> 实例。
        /// </summary>
        /// <param name="descriptor">给定的操作日志描述符。</param>
        /// <param name="provider">给定的操作日志管道。</param>
        public OperateLogCallHandler(IOperateLogDescriptor descriptor,
            IOperateLogProvider provider)
        {
            Descriptor = descriptor.NotNull(nameof(descriptor));
            Provider = provider.NotNull(nameof(provider));
        }


        /// <summary>
        /// 调用拦截方法。
        /// </summary>
        /// <param name="input">给定要调用拦截目标时的输入信息。</param>
        /// <param name="getNext">给定要通过行为链来获取下一个拦截行为的委托。</param>
        /// <returns>返回从拦截目标获得的返回信息。</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            input.GuardNull(nameof(input));
            getNext.GuardNull(nameof(getNext));

            var result = getNext()(input, getNext);
            if (result.Exception == null)
            {
                // 写入数据库
                Provider.AddOperateLog(Descriptor);
            }
            
            return result;
        }


        /// <summary>
        /// 表示某个处理程序将被执行。
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

    }
}
