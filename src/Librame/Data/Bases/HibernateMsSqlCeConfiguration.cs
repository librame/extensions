#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using FluentNHibernate.Cfg.Db;
using System;
using System.IO;

namespace Librame.Data.Bases
{
    /// <summary>
    /// NHibernate MsSqlCe 数据库配置。
    /// </summary>
    public class HibernateMsSqlCeConfiguration : PersistenceConfiguration<HibernateMsSqlCeConfiguration>
    {
        /// <summary>
        /// 构造一个 <see cref="HibernateMsSqlCeConfiguration"/> 实例。
        /// </summary>
        protected HibernateMsSqlCeConfiguration()
        {
            Driver<HibernateMsSqlServerCeDriver>();
        }


        /// <summary>
        /// 使用数据库文件配置。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="HibernateMsSqlCeConfiguration"/>。</returns>
        public HibernateMsSqlCeConfiguration UsingFile(string fileName)
        {
            //if (createDatabase)
            //{
            //    File.Delete(fileName);
            //}

            string localConnectionString = string.Format("Data Source={0}", fileName);
            if (!File.Exists(fileName))
            {
                var folder = Path.GetDirectoryName(fileName);
                if (!string.IsNullOrEmpty(folder))
                    Directory.CreateDirectory(folder);

                try
                {
                    CreateSqlCeDatabaseFile(localConnectionString);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            ConnectionString(localConnectionString);
            Driver(typeof(HibernateMsSqlServerCeDriver).AssemblyQualifiedName);

            return this;
        }

        private void CreateSqlCeDatabaseFile(string connectionString)
        {
            // We want to execute this code using Reflection, to avoid having a binary
            // dependency on SqlCe assembly

            //engine engine = new SqlCeEngine();
            //const string assemblyName = "System.Data.SqlServerCe, Version=4.0.0.1, Culture=neutral, PublicKeyToken=89845dcd8080cc91";
            const string assemblyName = "System.Data.SqlServerCe";
            const string typeName = "System.Data.SqlServerCe.SqlCeEngine";

            var sqlceEngineHandle = Activator.CreateInstance(assemblyName, typeName);
            var engine = sqlceEngineHandle.Unwrap();

            //engine.LocalConnectionString = connectionString;
            engine.GetType().GetProperty("LocalConnectionString").SetValue(engine, connectionString, null/*index*/);

            //engine.CreateDatabase();
            engine.GetType().GetMethod("CreateDatabase").Invoke(engine, null);

            //engine.Dispose();
            engine.GetType().GetMethod("Dispose").Invoke(engine, null);
        }


        /// <summary>
        /// 获取 MsSqlCe40 版配置。
        /// </summary>
        public static HibernateMsSqlCeConfiguration MsSqlCe40
        {
            get { return new HibernateMsSqlCeConfiguration().Dialect<HibernateMsSqlCe40Dialect>(); }
        }

    }
}
