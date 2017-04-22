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

namespace Librame
{
    /// <summary>
    /// Librame 基类。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class LibrameBase<T>
    {
        private static readonly ILog _log = null;

        static LibrameBase()
        {
            _log = LibrameArchitecture.LoggingAdapter.GetLogger<T>();
        }

        /// <summary>
        /// 获取日志。
        /// </summary>
        public static ILog Log
        {
            get { return _log; }
        }

    }
}
