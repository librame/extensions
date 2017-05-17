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

namespace Librame.Authorization.Managers
{
    using Adaptation;
    
    /// <summary>
    /// 存储管理器接口。
    /// </summary>
    public interface IStorageManager : IAdapterCollectionManager
    {
        /// <summary>
        /// 将对象转换为方便存储的字符串。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        string ToString(object obj);


        /// <summary>
        /// 将存储的字符串还原为对象。
        /// </summary>
        /// <typeparam name="T">指定的对象类型。</typeparam>
        /// <param name="str">给定的存储字符串。</param>
        /// <returns>返回对象。</returns>
        T FromString<T>(string str);

        /// <summary>
        /// 将存储的字符串还原为对象。
        /// </summary>
        /// <param name="str">给定的存储字符串。</param>
        /// <param name="type">给定的对象类型。</param>
        /// <returns>返回对象。</returns>
        object FromString(string str, Type type);
    }
}
