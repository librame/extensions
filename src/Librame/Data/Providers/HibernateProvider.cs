#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Linq;
using System.Reflection;

namespace Librame.Data.Providers
{
    using Bases;
    using Conventions;
    using Mappings;
    using Utility;

    /// <summary>
    /// NHibernate 管道。
    /// </summary>
    public class HibernateProvider : LibrameBase<HibernateProvider>, IProvider
    {
        /// <summary>
        /// 获取数据首选项。
        /// </summary>
        public DataSettings DataSettings { get; }
        
        /// <summary>
        /// 构造一个 <see cref="HibernateProvider"/> 实例。
        /// </summary>
        /// <param name="dataSettings">给定的 <see cref="DataSettings"/>。</param>
        public HibernateProvider(DataSettings dataSettings)
        {
            DataSettings = dataSettings.NotNull(nameof(dataSettings));
        }

        /// <summary>
        /// 获取 <see cref="IPersistenceConfigurer"/>。
        /// </summary>
        /// <returns>返回 <see cref="IPersistenceConfigurer"/>。</returns>
        protected virtual IPersistenceConfigurer GetPersistenceConfigurer()
        {
            if (DataSettings.IsDatabaseFile)
            {
                var fileName = DataSettings.GetDatabaseByFileName();
                var extension = fileName.PathExtension();

                switch (extension.ToLower())
                {
                    case "sqlite":
                        return SQLiteConfiguration.Standard.UsingFile(fileName);
                    case ".db":
                        goto case "sqlite";

                    case "sqlce":
                        return HibernateMsSqlCeConfiguration.MsSqlCe40.UsingFile(fileName);
                    case ".sdf":
                        goto case "sqlce";

                    default:
                        throw new NotSupportedException(string.Format("not supported database ({0})", fileName));
                }
            }
            else
            {
                Action<ConnectionStringBuilder> builderFactory = null;

                if (DataSettings.IsConnectionStringKey)
                {
                    var key = DataSettings.GetDatabaseByConnectionString();
                    builderFactory = (builder) => builder.FromConnectionStringWithKey(key);
                }
                else if (DataSettings.IsAppSettingKey)
                {
                    var key = DataSettings.GetDatabaseByAppSetting();
                    builderFactory = (builder) => builder.FromAppSetting(key);
                }
                else
                {
                    builderFactory = (builder) => builder.Is(DataSettings.Database);
                }

                switch (DataSettings.ProviderName.ToLower())
                {
                    case "mssql":
                        return MsSqlConfiguration.MsSql2012.ConnectionString(builderFactory);

                    case "mysql":
                        return MySQLConfiguration.Standard.ConnectionString(builderFactory);

                    case "oracle":
                        return OracleClientConfiguration.Oracle10.ConnectionString(builderFactory);

                    case "oracledata":
                        return OracleDataClientConfiguration.Oracle10.ConnectionString(builderFactory);

                    case "oraclemanageddate":
                        return OracleManagedDataClientConfiguration.Oracle10.ConnectionString(builderFactory);

                    default:
                        throw new NotSupportedException(string.Format("not supported provider name ({0})", DataSettings.ProviderName));
                }
            }
        }

        /// <summary>
        /// 建立公开配置。
        /// </summary>
        /// <param name="config">给定的 <see cref="Configuration"/>。</param>
        protected virtual void BuildExposeConfiguration(Configuration config)
        {
            config
                .SetProperty(NHibernate.Cfg.Environment.FormatSql, bool.FalseString)
                .SetProperty(NHibernate.Cfg.Environment.GenerateStatistics, bool.FalseString)
                .SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, Hbm2DDLKeyWords.None.ToString())
                .SetProperty(NHibernate.Cfg.Environment.PropertyBytecodeProvider, "lcg")
                .SetProperty(NHibernate.Cfg.Environment.PropertyUseReflectionOptimizer, bool.TrueString)
                .SetProperty(NHibernate.Cfg.Environment.QueryStartupChecking, bool.FalseString)
                .SetProperty(NHibernate.Cfg.Environment.ShowSql, bool.TrueString)
                .SetProperty(NHibernate.Cfg.Environment.StatementFetchSize, "100")
                .SetProperty(NHibernate.Cfg.Environment.UseProxyValidator, bool.FalseString)
                .SetProperty(NHibernate.Cfg.Environment.UseSqlComments, bool.FalseString)
                .SetProperty(NHibernate.Cfg.Environment.WrapResultSets, bool.TrueString)
                .SetProperty(NHibernate.Cfg.Environment.BatchSize, "256");

            // 此行不能删除，否则会提示找不到表名（代码创建表名模式）
            var export = new SchemaExport(config);

            // 创建表结构
            // 第一个 true 表示在控制台打印 SQL 语句
            // 第二个 true 表示导入 SQL 语句到数据库（每次都会重建数据表）; false 表示数据表不存在会抛出异常
            export.Create(false, false);
        }

        /// <summary>
        /// 映射程序集集合。
        /// </summary>
        /// <returns>返回 <see cref="AutoPersistenceModel"/>。</returns>
        protected virtual AutoPersistenceModel OnMappingAssemblies()
        {
            var assemblies = GetMappingAssemblies();
            assemblies.NotNull(nameof(assemblies));

            if (DataSettings.EnableEntityAutomapping)
            {
                return AutoMap.Source(new CombinedAssemblyTypeSource(assemblies.Select(a =>
                {
                    Log.Debug("Register assembly: " + a.FullName);

                    return new AssemblyTypeSource(a);
                })),
                new HibernateAutomappingConfiguration());
            }
            else
            {
                return AutoMap.Source(new CombinedAssemblyTypeSource(assemblies.Select(a =>
                {
                    Log.Debug("Register assembly: " + a.FullName);

                    return new AssemblyTypeSource(a);
                })));
            }
        }


        /// <summary>
        /// 获取包含要映射实体的程序集集合。
        /// </summary>
        /// <returns>返回程序集数组。</returns>
        public virtual Assembly[] GetMappingAssemblies()
        {
            return DataHelper.GetMappingAssemblies(DataSettings.AssemblyStrings);
        }


        /// <summary>
        /// 建立会话工厂接口。
        /// </summary>
        /// <returns>返回会话工厂接口实例。</returns>
        public virtual ISessionFactory BuildSessionFactory()
        {
            var persistenceModel = OnMappingAssemblies();

            // 映射约定
            persistenceModel.Conventions.Setup(con =>
            {
                con.Add<HibernateTableNameConvention>();
                con.Add<HibernateIdConvention>();
                //con.Add<HibernateStringLengthConvention>();
                con.Add<HibernateReferenceConvention>();
                con.Add<HibernateHasManyConvention>();
                con.Add<HibernateHasOneConvention>();
                con.Add<HibernateHasManyToManyConvention>();
            });

            var persistenceConfigurer = GetPersistenceConfigurer();

            // 映射文件目录（输出再创建）
            //var dataAdapter = LibrameArchitecture.AdapterManager.DataAdapter;
            //var mappingDirectory = dataAdapter.AdapterConfigDirectory.AppendPath("Mappings");

            try
            {
                return Fluently.Configure()
                    .Database(persistenceConfigurer)
                    .Mappings(m => m.AutoMappings.Add(persistenceModel)) // .ExportTo(mappingDirectory)
                    .ExposeConfiguration(cfg => BuildExposeConfiguration(cfg))
                    .BuildSessionFactory();
            }
            catch (ReflectionTypeLoadException rtlEx)
            {
                Log.Error("Register entity types is not null", rtlEx);

                throw rtlEx;
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);

                throw ex;
            }
        }

    }
}
