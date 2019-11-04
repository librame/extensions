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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象单例（使用时须定义一个派生自本抽象单例的密封类）。
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/leolion/p/10275027.html
    /// </remarks>
    /// <typeparam name="TSingleton">定义的单例密封类型。</typeparam>
    public abstract class AbstractSingleton<TSingleton> : ISingleton
        where TSingleton : class
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractSingleton{TSingleton}"/>。
        /// </summary>
        /// <remarks>
        /// 派生类须定义一个无参数且不可被公开实例化的构造函数。
        /// </remarks>
        protected AbstractSingleton()
        {
        }


        /// <summary>
        /// 获取该类的单例实例。
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TSingleton Instance
            => SingletonFactory.Instance;


        class SingletonFactory
        {
            /// <summary>
            /// 创建弱引用对象（表示在引用对象的同时仍然允许通过垃圾回收来回收该对象）。
            /// </summary>
            [ThreadStatic]
            private static WeakReference _reference;


            /// <summary>
            /// 防止编译器生成默认构造函数。
            /// </summary>
            private SingletonFactory()
            {
            }

            /// <summary>
            /// 显式静态构造函数，告诉 C# 编译器不要将类型标记为 BeforeFieldInit。
            /// </summary>
            static SingletonFactory()
            {
            }


            internal static TSingleton Instance
            {
                get
                {
                    if (!(_reference?.Target is TSingleton instance))
                    {
                        instance = CreateInstance();
                        _reference = new WeakReference(instance);
                    }

                    return instance;
                }
            }


            [MethodImpl(MethodImplOptions.NoInlining)]
            private static TSingleton CreateInstance()
            {
                var type = typeof(TSingleton);

                TSingleton instance;

                try
                {
                    instance = (TSingleton)type.InvokeMember(type.Name,
                            BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic,
                            binder: null,
                            target: null,
                            args: null,
                            CultureInfo.InvariantCulture);
                }
                catch (MissingMethodException ex)
                {
                    throw new TypeLoadException(
                        string.Format(CultureInfo.InvariantCulture,
                            "The type '{0}' must have a private constructor to be used in the Singleton pattern.",
                            type.FullName), ex);
                }

                return instance;
            }
        }

    }
}
