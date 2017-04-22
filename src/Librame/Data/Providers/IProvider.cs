#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Reflection;

namespace Librame.Data.Providers
{
    /// <summary>
    /// 数据管道接口。
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// 获取数据首选项。
        /// </summary>
        DataSettings DataSettings { get; }


        /// <summary>
        /// 获取包含要映射实体的程序集集合。
        /// </summary>
        /// <returns>返回程序集数组。</returns>
        Assembly[] GetMappingAssemblies();
    }
}
