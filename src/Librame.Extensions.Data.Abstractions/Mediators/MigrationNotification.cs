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

namespace Librame.Extensions.Data.Mediators
{
    using Core.Mediators;
    using Data.Stores;

    /// <summary>
    /// 迁移通知。
    /// </summary>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public class MigrationNotification<TMigration, TGenId> : INotification
        where TMigration : DataMigration<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 迁移。
        /// </summary>
        public TMigration Migration { get; set; }
    }
}
