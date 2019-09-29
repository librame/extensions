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
    using Core;

    /// <summary>
    /// 数据迁移通知。
    /// </summary>
    public class DataMigrationNotification : INotification
    {
        /// <summary>
        /// 迁移。
        /// </summary>
        public DataMigration Migration { get; set; }
    }
}
