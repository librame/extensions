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
using System.Threading;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 工作描述符。
    /// </summary>
    public class JobDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="JobDescriptor"/>。
        /// </summary>
        /// <param name="parameters">给定的参数数组。</param>
        public JobDescriptor(params object[] parameters)
        {
            Parameters = parameters;
        }


        /// <summary>
        /// 参数数组。
        /// </summary>
        public object[] Parameters { get; }

        /// <summary>
        /// 已启用。
        /// </summary>
        public bool Enabled { get; set; }
            = true;

        /// <summary>
        /// 执行动作。
        /// </summary>
        public Action<Thread, object[]> Execution { get; set; }

        /// <summary>
        /// 完成回调动作。
        /// </summary>
        public Action<Thread, object[]> FinishCallback { get; set; }

        /// <summary>
        /// 异常回调动作。
        /// </summary>
        public Action<Thread, object[], Exception> ErrorCallback { get; set; }
    }
}
