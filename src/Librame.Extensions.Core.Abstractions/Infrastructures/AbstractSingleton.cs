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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象单例（使用时须定义一个派生自此基类的密封类）。
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/leolion/p/10275027.html
    /// </remarks>
    /// <typeparam name="TSingleton">定义的单例密封类型。</typeparam>
    public abstract class AbstractSingleton<TSingleton>
        where TSingleton : class
    {
        // 派生类须定义一个无参数的私有构造函数
        //private Constructor() { }

        /// <summary>
        /// 获取该类的单例实例。
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TSingleton Instance => SingletonFactory.Instance;


        /// <summary>
        /// 创建单例实例的单例类工厂。
        /// </summary>
        private class SingletonFactory
        {
            /// <summary>
            /// 创建弱引用对象（表示在引用对象的同时仍然允许通过垃圾回收来回收该对象）。
            /// </summary>
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
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static SingletonFactory()
            {
            }

            /// <summary>
            /// 获取实例。
            /// </summary>
            internal static TSingleton Instance
            {
                get
                {
                    if (!(_reference?.Target is TSingleton instance))
                    {
                        instance = GetInstance();
                        _reference = new WeakReference(instance);
                    }

                    return instance;
                }
            }

            /// <summary>
            /// 获取特定类型的实例。
            /// </summary>
            /// <returns>返回单例。</returns>
            [SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Type.InvokeMember")]
            private static TSingleton GetInstance()
            {
                var type = typeof(TSingleton);

                TSingleton instance;

                try
                {
                    instance = (TSingleton)type.InvokeMember(type.Name,
                            BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic,
                            null, null, null,
                            CultureInfo.InvariantCulture);
                }
                catch (MissingMethodException ex)
                {
                    throw new TypeLoadException(
                        string.Format(CultureInfo.CurrentCulture,
                            "The type '{0}' must have a private constructor to be used in the Singleton pattern.",
                            type.FullName), ex);
                }

                return instance;
            }
        }
    }
}
