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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据偏好设置接口。
    /// </summary>
    public interface IDataPreferenceSetting : IPreferenceSetting
    {
        /// <summary>
        /// 编译目录。
        /// </summary>
        string CompilersFolder { get; }

        /// <summary>
        /// 文件数据库目录。
        /// </summary>
        string DatabasesFolder { get; }

        /// <summary>
        /// 迁移目录。
        /// </summary>
        string MigrationsFolder { get; }


        /// <summary>
        /// 默认创建时间。
        /// </summary>
        DateTimeOffset DefaultCreatedTime { get; }

        /// <summary>
        /// 默认排序。
        /// </summary>
        float DefaultRank { get; }

        /// <summary>
        /// 默认状态。
        /// </summary>
        DataStatus DefaultStatus { get; }
    }
}
