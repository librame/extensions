#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Common.Logging;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Librame.Container.Interception
{
    using Utility;

    /// <summary>
    /// 异常日志拦截行为。
    /// </summary>
    public class ExceptionLogInterceptionBehavior : IInterceptionBehavior
    {
        /// <summary>
        /// 获取当前行为需要拦截的对象类型接口。
        /// </summary>
        /// <returns>返回所有需要拦截的对象类型接口。</returns>
        public virtual IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }


        /// <summary>
        /// 获取声明指定方法成员的类的日志。
        /// </summary>
        /// <param name="method">给定的方法和构造函数信息。</param>
        /// <returns>返回 <see cref="ILog"/>。</returns>
        protected virtual ILog GetLogger(MethodBase method)
        {
            return LibrameArchitecture.Logging.GetLogger(method.DeclaringType);
        }

        /// <summary>
        /// 通过实现此方法来拦截调用并执行所需的拦截行为。
        /// </summary>
        /// <param name="input">给定要调用拦截目标时的输入信息。</param>
        /// <param name="getNext">给定要通过行为链来获取下一个拦截行为的委托。</param>
        /// <returns>返回从拦截目标获得的返回信息。</returns>
        public virtual IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            input.NotNull(nameof(input));
            getNext.NotNull(nameof(getNext));

            // 获取记录器
            var log = GetLogger(input.MethodBase);

            // 重调运行
            var result = getNext()(input, getNext);

            // 异常处理部分
            if (result.Exception == null)
            {
                // 调用成功
                InvokeSuccess(log, result);
            }
            else
            {
                // 记录可能存在的参数信息
                LogDebug(log, input);

                // 调用异常
                InvokeException(log, result);
            }

            // 调用完成
            InvokeFinally(log, result);

            return result;
        }


        /// <summary>
        /// 记录调试信息。
        /// </summary>
        /// <param name="log">给定的 <see cref="ILog"/>。</param>
        /// <param name="input">给定要调用拦截目标时的输入信息。</param>
        protected virtual void LogDebug(ILog log, IMethodInvocation input)
        {
            if (input.Arguments.Count < 1)
                return;

            // 参数部分
            var sb = new StringBuilder();

            for (int i = 0; i < input.Arguments.Count; i++)
            {
                var parameter = input.Arguments[i];

                if (!ReferenceEquals(parameter, null))
                    sb.AppendFormat("第{0}个参数值为:{1}", i + 1, parameter.ToString());
                else
                    sb.AppendFormat("第{0}个参数值为:空", i + 1);

                if (i != input.Arguments.Count - 1)
                    sb.Append("; ");
            }

            log.Debug(sb.ToString());
        }

        /// <summary>
        /// 调用成功。
        /// </summary>
        /// <param name="log">给定的 <see cref="ILog"/>。</param>
        /// <param name="methodReturn">给定的 <see cref="IMethodReturn"/>。</param>
        protected virtual void InvokeSuccess(ILog log, IMethodReturn methodReturn)
        {
            //log.Debug("执行成功，无异常");
        }

        /// <summary>
        /// 调用异常。
        /// </summary>
        /// <param name="log">给定的 <see cref="ILog"/>。</param>
        /// <param name="methodReturn">给定的 <see cref="IMethodReturn"/>。</param>
        protected virtual void InvokeException(ILog log, IMethodReturn methodReturn)
        {
            // 记录异常的内容 比如 Log4Net 等
            log.Error(methodReturn.Exception.InnerMessage(), methodReturn.Exception);

            // methodReturn.Exception 重置为 null 表示异常已经被处理
            methodReturn.Exception = null;
        }

        /// <summary>
        /// 调用完成。
        /// </summary>
        /// <param name="log">给定的 <see cref="ILog"/>。</param>
        /// <param name="methodReturn">给定的 <see cref="IMethodReturn"/>。</param>
        protected virtual void InvokeFinally(ILog log, IMethodReturn methodReturn)
        {
            //log.Debug("完成");
        }


        /// <summary>
        /// 获取一个布尔值，该值表示当前拦截行为被调用时，是否真的需要执行某些操作。
        /// </summary>
        public bool WillExecute
        {
            get { return true; }
        }
        
    }
}
