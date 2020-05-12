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
    /// 数据偏好设置。
    /// </summary>
    public class DataPreferenceSetting : AbstractPreferenceSetting, IDataPreferenceSetting
    {
        /// <summary>
        /// 编译目录。
        /// </summary>
        public virtual string CompilersFolder
            => "data_compilers";

        /// <summary>
        /// 文件数据库目录。
        /// </summary>
        public virtual string DatabasesFolder
            => "data_bases";

        /// <summary>
        /// 迁移目录。
        /// </summary>
        public virtual string MigrationsFolder
            => "data_migrations";


        /// <summary>
        /// 默认创建时间（<see cref="DateTimeOffset.UtcNow"/>）。
        /// </summary>
        public virtual DateTimeOffset DefaultCreatedTime
            => DateTimeOffset.UtcNow;

        /// <summary>
        /// 默认排序（10）。
        /// </summary>
        public virtual float DefaultRank
            => 10;

        /// <summary>
        /// 默认状态（<see cref="DataStatus.Public"/>）。
        /// </summary>
        public virtual DataStatus DefaultStatus
            => DataStatus.Public;
    }
}
