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
    /// 实例工厂接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IInstanceFactory<T>
    {
        /// <summary>
        /// 获取默认实例名的实例。
        /// </summary>
        /// <returns>返回类型实例。</returns>
        T GetInstance();

        /// <summary>
        /// 获取指定名称的实例。
        /// </summary>
        /// <param name="name">给定的实例名。</param>
        /// <returns>返回类型实例。</returns>
        T GetInstance(string name);


        /// <summary>
        /// 设置默认实例名的实例。
        /// </summary>
        /// <param name="instance">给定的类型实例。</param>
        /// <returns>返回类型实例。</returns>
        T SetInstance(T instance);

        /// <summary>
        /// 设置指定名称的实例。
        /// </summary>
        /// <param name="name">给定的实例名。</param>
        /// <param name="instance">给定的类型实例。</param>
        /// <returns>返回类型实例。</returns>
        T SetInstance(string name, T instance);
    }
}
