#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
        /// 编译文件夹。
        /// </summary>
        public virtual string CompilersFolder { get; }
            = "data_compilers";

        /// <summary>
        /// 文件型数据库文件夹。
        /// </summary>
        public virtual string DatabasesFolder { get; }
            = "data_bases";

        /// <summary>
        /// 迁移文件夹。
        /// </summary>
        public virtual string MigrationsFolder { get; }
            = "data_migrations";

        /// <summary>
        /// 初始化器文件夹。
        /// </summary>
        public virtual string InitializersFolder { get; }
            = "data_initializers";


        /// <summary>
        /// 访问器可隔离字符串连接符。
        /// </summary>
        public virtual char AccessorIsolateableStringConnector { get; }
            = '_'; // 与租户域名“.”字符作区分

        /// <summary>
        /// 迁移命令信息连接字符串的连接符。
        /// </summary>
        public virtual char MigrationCommandInfoConnectionStringConnector { get; }
            = ':';


        /// <summary>
        /// 默认创建时间（<see cref="DateTimeOffset.UtcNow"/>）。
        /// </summary>
        public virtual DateTimeOffset DefaultCreatedTime { get; }
            = DateTimeOffset.UtcNow;

        /// <summary>
        /// 默认排序（10）。
        /// </summary>
        public virtual float DefaultRank { get; }
            = 10;

        /// <summary>
        /// 默认架构（dbo）。
        /// </summary>
        public virtual string DefaultSchema { get; }
            = "dbo";

        /// <summary>
        /// 默认状态（<see cref="DataStatus.Public"/>）。
        /// </summary>
        public virtual DataStatus DefaultStatus { get; }
            = DataStatus.Public;

        /// <summary>
        /// 查看按类型名称创建的最大长度（默认为 100，超过部分以省略号代替）。
        /// </summary>
        public virtual int ViewCreatedByTypeNameMaxLength { get; }
            = 100;
    }
}
