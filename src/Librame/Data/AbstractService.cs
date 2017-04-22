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
    /// 抽象服务。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public abstract class AbstractService<T> : IService<T>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService{T}"/> 实例。
        /// </summary>
        public AbstractService()
        {
        }


        /// <summary>
        /// 仓库接口。
        /// </summary>
        public IRepository<T> Repository { get; set; }
    }
}
