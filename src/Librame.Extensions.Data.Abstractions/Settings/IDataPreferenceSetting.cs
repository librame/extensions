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
    /// 数据偏好设置接口。
    /// </summary>
    public interface IDataPreferenceSetting : IPreferenceSetting
    {
        /// <summary>
        /// 编译文件夹。
        /// </summary>
        string CompilersFolder { get; }

        /// <summary>
        /// 文件型数据库文件夹。
        /// </summary>
        string DatabasesFolder { get; }

        /// <summary>
        /// 迁移文件夹。
        /// </summary>
        string MigrationsFolder { get; }

        /// <summary>
        /// 初始化器文件夹。
        /// </summary>
        string InitializersFolder { get; }


        /// <summary>
        /// 访问器可隔离字符串连接符。
        /// </summary>
        char AccessorIsolateableStringConnector { get; }

        /// <summary>
        /// 迁移命令信息连接字符串的连接符。
        /// </summary>
        char MigrationCommandInfoConnectionStringConnector { get; }


        /// <summary>
        /// 默认创建时间。
        /// </summary>
        DateTimeOffset DefaultCreatedTime { get; }

        /// <summary>
        /// 默认排序。
        /// </summary>
        float DefaultRank { get; }

        /// <summary>
        /// 默认架构。
        /// </summary>
        string DefaultSchema { get; }

        /// <summary>
        /// 默认状态。
        /// </summary>
        DataStatus DefaultStatus { get; }

        /// <summary>
        /// 查看按类型名称创建的最大长度。
        /// </summary>
        int ViewCreatedByTypeNameMaxLength { get; }
    }
}
