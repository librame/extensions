#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据状态枚举型状态接口。
    /// </summary>
    public interface IStatus : IStatus<DataStatus>
    {
    }


    /// <summary>
    /// 状态接口。
    /// </summary>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    public interface IStatus<TStatus>
        where TStatus : struct
    {
        /// <summary>
        /// 状态。
        /// </summary>
        TStatus Status { get; set; }
    }
}
