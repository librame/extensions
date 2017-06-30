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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Librame.Data.Descriptors
{
    /// <summary>
    /// 抽象数据、主键描述符。
    /// </summary>
    /// <typeparam name="TId">指定的主键类型。</typeparam>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public abstract class AbstractDataIdDescriptor<TId> : AbstractIdDescriptor<TId>, IDataIdDescriptor<TId>
        where TId : struct
    {
        /// <summary>
        /// 数据排序。
        /// </summary>
        [DisplayName("数据排序")]
        [DefaultValue(1)]
        public virtual int DataRank { get; set; }

        /// <summary>
        /// 数据状态。
        /// </summary>
        [DisplayName("数据状态")]
        [DefaultValue(DataStatus.Public)]
        public virtual DataStatus DataStatus { get; set; } = DataStatus.Public;
    }
}
