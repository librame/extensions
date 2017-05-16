#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Common.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace Librame.Data.Providers
{
    using Utility;

    /// <summary>
    /// EntityFramework 管道。
    /// </summary>
    public class EntityProvider : DbContext, IProvider
    {
        /// <summary>
        /// 当前日志对象。
        /// </summary>
        protected readonly static ILog Log = LibrameArchitecture.Logging.GetLogger<EntityProvider>();


        /// <summary>
        /// 获取数据首选项。
        /// </summary>
        public DataSettings DataSettings { get; }

        /// <summary>
        /// 构造一个 <see cref="EntityProvider"/> 实例。
        /// </summary>
        /// <param name="dataSettings">给定的数据首选项。</param>
        public EntityProvider(DataSettings dataSettings)
            : base(dataSettings.GetRealDatabase())
        {
            DataSettings = dataSettings;
        }

        /// <summary>
        /// 创建模型。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="DbModelBuilder"/>。</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            OnMappingAssemblies(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Types().Configure((config) =>
            {
                // 表名属性特性优先
                var table = TypeUtility.GetClassAttribute<TableAttribute>(config.ClrType);
                if (table != null && !string.IsNullOrEmpty(table.Name))
                {
                    if (!string.IsNullOrEmpty(table.Schema))
                        config.ToTable(table.Name, table.Schema);
                    else
                        config.ToTable(table.Name);
                }
                else
                {
                    // 默认复数化实体类名
                    config.ToTable(WordHelper.AsPluralize(config.ClrType.Name));
                }
            });
        }

        /// <summary>
        /// 映射程序集集合。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="DbModelBuilder"/>。</param>
        protected virtual void OnMappingAssemblies(DbModelBuilder modelBuilder)
        {
            var assemblies = GetMappingAssemblies();
            assemblies.NotNull(nameof(assemblies));

            if (DataSettings.EnableEntityAutomapping)
            {
                assemblies.Invoke((a) =>
                {
                    Log.Debug("Register assembly: " + a?.FullName);

                    // 提取要映射的实体类型集合
                    var types = TypeUtility.GetAssignableTypes<IEntityAutomapping>(a);

                    if (ReferenceEquals(types, null) || types.Length < 1)
                    {
                        Log.Error("Register entity types is not null");
                    }

                    types.Invoke((t) =>
                    {
                        Log.Debug("Register entity type: " + t?.FullName);

                        modelBuilder.RegisterEntityType(t);
                    });
                });
            }
            else
            {
                assemblies.Invoke((a) =>
                {
                    Log.Debug("Register assembly: " + a?.FullName);

                    modelBuilder.Configurations.AddFromAssembly(a);
                });
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

    }
}
