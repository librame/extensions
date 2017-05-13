#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Data
{
    using Utility;

    /// <summary>
    /// 数据首选项。
    /// </summary>
    public class DataSettings : Adaptation.AbstractAdapterSettings, Adaptation.IAdapterSettings
    {
        /// <summary>
        /// 数据库信息。
        /// </summary>
        /// <remarks>
        /// 支持数据库文件、连接字符串、ConnectionStringKey=、AppSettingsKey=等。
        /// </remarks>
        public string Database { get; set; }

        /// <summary>
        /// 是否为文件类数据库。
        /// </summary>
        /// <remarks>
        /// 以启用系统对相对路径的支持。
        /// </remarks>
        public bool IsDatabaseFile { get; set; }

        /// <summary>
        /// 数据库提供程序名。
        /// </summary>
        /// <remarks>
        /// 主要针对非文件型数据库。
        /// </remarks>
        public string ProviderName { get; set; }

        /// <summary>
        /// 包含实体的程序集列表。
        /// </summary>
        /// <remarks>
        /// 多个之间以英文分号分隔。
        /// </remarks>
        public string AssemblyStrings { get; set; }

        /// <summary>
        /// 启用实体自映射（即仅自映射实现了 <see cref="IEntityAutomapping"/> 接口的实体）。
        /// </summary>
        public bool EnableEntityAutomapping { get; set; }


        /// <summary>
        /// EntityFramework 管道类型字符串。
        /// </summary>
        public string EntityProviderTypeString { get; set; }

        /// <summary>
        /// NHibernate 管道类型字符串。
        /// </summary>
        public string HibernateProviderTypeString { get; set; }

        
        /// <summary>
        /// 构造一个 <see cref="DataSettings"/> 实例。
        /// </summary>
        public DataSettings()
        {
        }


        /// <summary>
        /// 是否为 AppSetting 键名。
        /// </summary>
        protected internal virtual bool IsAppSettingKey
        {
            get { return IsDatabaseKey(DataHelper.APP_SETTING_KEY); }
        }

        /// <summary>
        /// 是否为 ConnectionString 键名。
        /// </summary>
        protected internal virtual bool IsConnectionStringKey
        {
            get { return IsDatabaseKey(DataHelper.CONNECTION_STRING_KEY); }
        }
        
        /// <summary>
        /// 是否包含数据库指定键名。
        /// </summary>
        /// <param name="key">给定的键名</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool IsDatabaseKey(string key)
        {
            if (string.IsNullOrEmpty(Database))
                return false;

            return Database.StartsWith(key);
        }


        /// <summary>
        /// 通过 AppSetting 键名获取数据库。
        /// </summary>
        /// <returns>返回字符串。</returns>
        protected internal virtual string GetDatabaseByAppSetting()
        {
            if (IsAppSettingKey)
                return Database.Replace(DataHelper.APP_SETTING_KEY, string.Empty);

            return Database;
        }

        /// <summary>
        /// 通过 ConnectionString 键名获取数据库。
        /// </summary>
        /// <returns>返回字符串。</returns>
        protected internal virtual string GetDatabaseByConnectionString()
        {
            if (IsConnectionStringKey)
                return Database.Replace(DataHelper.CONNECTION_STRING_KEY, string.Empty);

            return Database;
        }


        /// <summary>
        /// 通过文件名获取数据库。
        /// </summary>
        /// <returns>返回字符串。</returns>
        protected internal virtual string GetDatabaseByFileName()
        {
            if (IsDatabaseFile && Database.IsRelativePath())
                return PathUtility.BaseDirectory.AppendPath(Database);

            return Database;
        }


        /// <summary>
        /// 获取真实数据库。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetRealDatabase()
        {
            if (string.IsNullOrEmpty(Database))
                return string.Empty;

            if (IsConnectionStringKey)
                return GetDatabaseByConnectionString();

            if (IsDatabaseFile)
                return GetDatabaseByFileName();

            if (IsAppSettingKey)
                return GetDatabaseByAppSetting();
            
            return Database;
        }

    }
}
