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
    using Utility;

    /// <summary>
    /// 存储管理器。
    /// </summary>
    public class StorageManager : AbstractAdapterCollectionManager, IStorageManager
    {
        /// <summary>
        /// 构造一个存储管理器实例。
        /// </summary>
        /// <param name="adapters">给定的适配器器管理器。</param>
        public StorageManager(IAdapterCollection adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 将对象转换为方便存储的字符串。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        public virtual string ToString(object obj)
        {
            var str = obj.AsPairsString();

            return Adapters.Algorithm.StandardAes.Encrypt(str);
        }


        /// <summary>
        /// 将存储的字符串还原为对象。
        /// </summary>
        /// <typeparam name="T">指定的对象类型。</typeparam>
        /// <param name="str">给定的存储字符串。</param>
        /// <returns>返回对象。</returns>
        public virtual T FromString<T>(string str)
        {
            return (T)FromString(str, typeof(T));
        }

        /// <summary>
        /// 将存储的字符串还原为对象。
        /// </summary>
        /// <param name="str">给定的存储字符串。</param>
        /// <param name="type">给定的对象类型。</param>
        /// <returns>返回对象。</returns>
        public virtual object FromString(string str, Type type)
        {
            str = Adapters.Algorithm.StandardAes.Decrypt(str);

            return str.FromPairsString(type);
        }

    }
}
