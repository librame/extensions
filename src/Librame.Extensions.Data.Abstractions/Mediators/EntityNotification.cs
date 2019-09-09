#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 实体通知。
    /// </summary>
    public class EntityNotification : INotification
    {
        /// <summary>
        /// 实体集合。
        /// </summary>
        public List<DataEntity> Entities { get; set; }
    }
}
