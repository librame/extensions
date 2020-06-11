#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Mediators
{
    using Core.Mediators;

    /// <summary>
    /// 迁移通知。
    /// </summary>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    public class MigrationNotification<TMigration> : INotificationIndication
        where TMigration : class
    {
        /// <summary>
        /// 迁移。
        /// </summary>
        public TMigration Migration { get; set; }
    }
}
