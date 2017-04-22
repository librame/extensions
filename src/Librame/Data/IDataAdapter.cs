#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Data
{
    /// <summary>
    /// 数据适配器接口。
    /// </summary>
    public interface IDataAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取或设置 <see cref="DataSettings"/>。
        /// </summary>
        DataSettings DataSettings { get; set; }


        /// <summary>
        /// 获取 EntityFramework 仓库。
        /// </summary>
        /// <typeparam name="T">指定的实体类型。</typeparam>
        /// <param name="dataSettings">给定的数据首选项（默认为空表示使用当前属性 <see cref="DataSettings"/> 实例）。</param>
        /// <returns>返回 <see cref="IRepository{T}"/>。</returns>
        IRepository<T> GetEntityRepository<T>(DataSettings dataSettings = null) where T : class;

        /// <summary>
        /// 获取 NHibernate 仓库。
        /// </summary>
        /// <typeparam name="T">指定的实体类型。</typeparam>
        /// <param name="dataSettings">给定的数据首选项（默认为空表示使用当前属性 <see cref="DataSettings"/> 实例）。</param>
        /// <returns>返回 <see cref="IRepository{T}"/>。</returns>
        IRepository<T> GetHibernateRepository<T>(DataSettings dataSettings = null) where T : class;

        /// <summary>
        /// 获取当前仓库。
        /// </summary>
        /// <typeparam name="T">指定的实体类型。</typeparam>
        /// <returns>返回 <see cref="IRepository{T}"/>。</returns>
        IRepository<T> GetCurrentRepository<T>() where T : class;

    }
}
