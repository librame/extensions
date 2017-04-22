#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Utility
{
    /// <summary>
    /// 单例。
    /// </summary>
    public class Singleton<T>
        where T : class, new()
    {
        /// <summary>
        /// 双重检查锁定单例。
        /// </summary>
        public sealed class DoubleCheckLocked
        {
            private static readonly object _locker = new object();
            private static T _instance = default(T);

            private DoubleCheckLocked()
            {
            }

            /// <summary>
            /// 获取当前实例。
            /// </summary>
            public static T Current
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (_locker)
                        {
                            if (_instance == null)
                            {
                                _instance = new T();
                            }
                        }
                    }

                    return _instance;
                }
            }
        }


        /// <summary>
        /// 静态工厂单例。
        /// </summary>
        public sealed class StaticFactory
        {
            private static readonly T _instance = new T();

            private StaticFactory()
            {
            }

            /// <summary>
            /// 获取当前实例。
            /// </summary>
            public static T Current
            {
                get { return _instance; }
            }
        }


        /// <summary>
        /// 静态内部延迟单例。
        /// </summary>
        public sealed class StaticNestedLazy
        {
            private StaticNestedLazy()
            {
            }

            /// <summary>
            /// 获取当前实例。
            /// </summary>
            public static T Current
            {
                get { return Nested._instance; }
            }

            private static class Nested
            {
                static Nested()
                {
                }

                internal static readonly T _instance = new T();
            }
        }

    }
}
