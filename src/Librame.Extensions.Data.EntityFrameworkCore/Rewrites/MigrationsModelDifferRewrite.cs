#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal
{
    /// <summary>
    /// <see cref="MigrationsModelDiffer"/> 改写。
    /// </summary>
    [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
    public class MigrationsModelDifferRewrite : MigrationsModelDiffer
    {
        /// <summary>
        /// 构造一个 <see cref="MigrationsModelDifferRewrite"/>。
        /// </summary>
        /// <param name="typeMappingSource">给定的 <see cref="IRelationalTypeMappingSource"/>。</param>
        /// <param name="migrationsAnnotations">给定的 <see cref="IMigrationsAnnotationProvider"/>。</param>
        /// <param name="changeDetector">给定的 <see cref="IChangeDetector"/>。</param>
        /// <param name="updateAdapterFactory">给定的 <see cref="IUpdateAdapterFactory"/>。</param>
        /// <param name="commandBatchPreparerDependencies">给定的 <see cref="CommandBatchPreparerDependencies"/>。</param>
        public MigrationsModelDifferRewrite(IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotations,
            IChangeDetector changeDetector,
            IUpdateAdapterFactory updateAdapterFactory,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies)
            : base(typeMappingSource, migrationsAnnotations, changeDetector,
                  updateAdapterFactory, commandBatchPreparerDependencies)
        {
        }


        #region IEntityType

        /// <summary>
        /// 表映射差异。
        /// </summary>
        /// <param name="source">给定的来源 <see cref="TableMapping"/>。</param>
        /// <param name="target">给定的目标 <see cref="TableMapping"/>。</param>
        /// <param name="diffContext">给定的差异上下文。</param>
        /// <returns>返回 <see cref="IEnumerable{MigrationOperation}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected override IEnumerable<MigrationOperation> Diff(TableMapping source,
            TableMapping target, DiffContext diffContext)
        {
            if (source.Schema != target.Schema
                || source.Name != target.Name)
            {
                // 如果是创建分表操作
                var entityType = target.EntityTypes[0];
                if (entityType.ClrType.IsDefined<ShardableAttribute>())
                {
                    // 不使用 Add 方法创建表操作，会添加重复的索引导致异常
                    var createTableOperation = new CreateTableOperation
                    {
                        Schema = target.Schema,
                        Name = target.Name,
                        Comment = target.GetComment()
                    };
                    createTableOperation.AddAnnotations(MigrationsAnnotations.For(entityType));

                    createTableOperation.Columns.AddRange
                        (GetSortedProperties(target)
                        .SelectMany(p => Add(p, diffContext, inline: true))
                        .Cast<AddColumnOperation>());

                    var primaryKey = entityType.FindPrimaryKey();
                    if (primaryKey != null)
                    {
                        createTableOperation.PrimaryKey = Add(primaryKey, diffContext).Cast<AddPrimaryKeyOperation>().Single();
                    }

                    createTableOperation.UniqueConstraints.AddRange(
                        target.GetKeys().Where(k => !k.IsPrimaryKey()).SelectMany(k => Add(k, diffContext))
                            .Cast<AddUniqueConstraintOperation>());

                    createTableOperation.CheckConstraints.AddRange(
                        target.GetCheckConstraints().SelectMany(c => Add(c, diffContext))
                            .Cast<CreateCheckConstraintOperation>());

                    foreach (var targetEntityType in target.EntityTypes)
                    {
                        diffContext.AddCreate(targetEntityType, createTableOperation);
                    }

                    yield return createTableOperation;

                    yield break; // 跳出迭代，防止出现重复的添加主键、索引等相关操作
                }
                else
                {
                    yield return new RenameTableOperation
                    {
                        Schema = source.Schema,
                        Name = source.Name,
                        NewSchema = target.Schema,
                        NewName = target.Name
                    };
                }
            }

            // Validation should ensure that all the relevant annotations for the collocated entity types are the same
            var sourceMigrationsAnnotations = MigrationsAnnotations.For(source.EntityTypes[0]).ToList();
            var targetMigrationsAnnotations = MigrationsAnnotations.For(target.EntityTypes[0]).ToList();

            if (source.GetComment() != target.GetComment()
                || HasDifferences(sourceMigrationsAnnotations, targetMigrationsAnnotations))
            {
                var alterTableOperation = new AlterTableOperation
                {
                    Name = target.Name,
                    Schema = target.Schema,
                    Comment = target.GetComment(),
                    OldTable = { Comment = source.GetComment() }
                };

                alterTableOperation.AddAnnotations(targetMigrationsAnnotations);
                alterTableOperation.OldTable.AddAnnotations(sourceMigrationsAnnotations);

                yield return alterTableOperation;
            }

            var operations = Diff(source.GetProperties(), target.GetProperties(), diffContext)
                .Concat(Diff(source.GetKeys(), target.GetKeys(), diffContext))
                .Concat(Diff(source.GetIndexes(), target.GetIndexes(), diffContext))
                .Concat(Diff(source.GetCheckConstraints(), target.GetCheckConstraints(), diffContext));

            foreach (var operation in operations)
            {
                yield return operation;
            }

            DiffData(source, target, diffContext);
        }


        ///// <summary>
        /////     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        /////     the same compatibility standards as public APIs. It may be changed or removed without notice in
        /////     any release. You should only use it directly in your code with extreme caution and knowing that
        /////     doing so can result in application failures when updating to a new Entity Framework Core release.
        ///// </summary>
        //[SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        //protected override IEnumerable<MigrationOperation> Add(TableMapping target, DiffContext diffContext)
        //{
        //    var entityType = target.EntityTypes[0];
        //    var createTableOperation = new CreateTableOperation
        //    {
        //        Schema = target.Schema,
        //        Name = target.Name,
        //        Comment = target.GetComment()
        //    };
        //    createTableOperation.AddAnnotations(MigrationsAnnotations.For(entityType));

        //    createTableOperation.Columns.AddRange(
        //        GetSortedProperties(target).SelectMany(p => Add(p, diffContext, inline: true)).Cast<AddColumnOperation>());

        //    var primaryKey = target.EntityTypes[0].FindPrimaryKey();
        //    if (primaryKey != null)
        //    {
        //        createTableOperation.PrimaryKey = Add(primaryKey, diffContext).Cast<AddPrimaryKeyOperation>().Single();
        //    }

        //    createTableOperation.UniqueConstraints.AddRange(
        //        target.GetKeys().Where(k => !k.IsPrimaryKey()).SelectMany(k => Add(k, diffContext))
        //            .Cast<AddUniqueConstraintOperation>());

        //    createTableOperation.CheckConstraints.AddRange(
        //        target.GetCheckConstraints().SelectMany(c => Add(c, diffContext))
        //            .Cast<CreateCheckConstraintOperation>());

        //    foreach (var targetEntityType in target.EntityTypes)
        //    {
        //        diffContext.AddCreate(targetEntityType, createTableOperation);
        //    }

        //    yield return createTableOperation;

        //    foreach (var operation in target.GetIndexes().SelectMany(i => Add(i, diffContext)))
        //    {
        //        yield return operation;
        //    }
        //}


        private static IEnumerable<IProperty> GetSortedProperties(TableMapping target)
            => GetSortedProperties(target.GetRootType())
                .Distinct((x, y) => x.GetColumnName() == y.GetColumnName());

        private static IEnumerable<IProperty> GetSortedProperties(IEntityType entityType)
        {
            var shadowProperties = new List<IProperty>();
            var shadowPrimaryKeyProperties = new List<IProperty>();
            var primaryKeyPropertyGroups = new Dictionary<PropertyInfo, IProperty>();
            var groups = new Dictionary<PropertyInfo, List<IProperty>>();
            var unorderedGroups = new Dictionary<PropertyInfo, SortedDictionary<int, IProperty>>();
            var types = new Dictionary<Type, SortedDictionary<int, PropertyInfo>>();

            foreach (var property in entityType.GetDeclaredProperties())
            {
                var clrProperty = property.PropertyInfo;
                if (clrProperty == null)
                {
                    if (property.IsPrimaryKey())
                    {
                        shadowPrimaryKeyProperties.Add(property);

                        continue;
                    }

                    var foreignKey = property.GetContainingForeignKeys()
                        .FirstOrDefault(fk => fk.DependentToPrincipal?.PropertyInfo != null);
                    if (foreignKey == null)
                    {
                        shadowProperties.Add(property);

                        continue;
                    }

                    clrProperty = foreignKey.DependentToPrincipal.PropertyInfo;
                    var groupIndex = foreignKey.Properties.IndexOf(property);

                    unorderedGroups.GetOrAddNew(clrProperty).Add(groupIndex, property);
                }
                else
                {
                    if (property.IsPrimaryKey())
                    {
                        primaryKeyPropertyGroups.Add(clrProperty, property);
                    }

                    groups.Add(
                        clrProperty, new List<IProperty> { property });
                }

                var clrType = clrProperty.DeclaringType;
                var index = clrType.GetTypeInfo().DeclaredProperties
                    .IndexOf(clrProperty, PropertyInfoEqualityComparer.Instance);

                Debug.Assert(clrType != null);
                types.GetOrAddNew(clrType)[index] = clrProperty;
            }

            foreach (var group in unorderedGroups)
            {
                groups.Add(group.Key, group.Value.Values.ToList());
            }

            foreach (var definingForeignKey in entityType.GetDeclaredReferencingForeignKeys()
                .Where(
                    fk => fk.DeclaringEntityType.GetRootType() != entityType.GetRootType()
                        && fk.DeclaringEntityType.GetTableName() == entityType.GetTableName()
                        && fk.DeclaringEntityType.GetSchema() == entityType.GetSchema()
                        && fk
                        == fk.DeclaringEntityType
                            .FindForeignKey(
                                fk.DeclaringEntityType.FindPrimaryKey().Properties,
                                entityType.FindPrimaryKey(),
                                entityType)))
            {
                var clrProperty = definingForeignKey.PrincipalToDependent?.PropertyInfo;
                var properties = GetSortedProperties(definingForeignKey.DeclaringEntityType).ToList();
                if (clrProperty == null)
                {
                    shadowProperties.AddRange(properties);

                    continue;
                }

                groups.Add(clrProperty, properties);

                var clrType = clrProperty.DeclaringType;
                var index = clrType.GetTypeInfo().DeclaredProperties
                    .IndexOf(clrProperty, PropertyInfoEqualityComparer.Instance);

                Debug.Assert(clrType != null);
                types.GetOrAddNew(clrType)[index] = clrProperty;
            }

            var graph = new Multigraph<Type, object>();
            graph.AddVertices(types.Keys);

            foreach (var left in types.Keys)
            {
                var found = false;
                foreach (var baseType in left.GetBaseTypes())
                {
                    foreach (var right in types.Keys)
                    {
                        if (right == baseType)
                        {
                            graph.AddEdge(right, left, null);
                            found = true;

                            break;
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                }
            }

            var sortedPropertyInfos = graph.TopologicalSort().SelectMany(e => types[e].Values).ToList();

            return sortedPropertyInfos
                .Select(pi => primaryKeyPropertyGroups.ContainsKey(pi) ? primaryKeyPropertyGroups[pi] : null)
                .Where(e => e != null)
                .Concat(shadowPrimaryKeyProperties)
                .Concat(sortedPropertyInfos.Where(pi => !primaryKeyPropertyGroups.ContainsKey(pi)).SelectMany(p => groups[p]))
                .Concat(shadowProperties)
                .Concat(entityType.GetDirectlyDerivedTypes().SelectMany(GetSortedProperties));
        }


        private class PropertyInfoEqualityComparer : IEqualityComparer<PropertyInfo>
        {
            private PropertyInfoEqualityComparer()
            {
            }

            public static readonly PropertyInfoEqualityComparer Instance = new PropertyInfoEqualityComparer();

            public bool Equals(PropertyInfo x, PropertyInfo y)
                => x.IsSameAs(y);

            public int GetHashCode(PropertyInfo obj)
                => throw new NotImplementedException();
        }

        #endregion


        #region IKey

        ///// <summary>
        /////     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        /////     the same compatibility standards as public APIs. It may be changed or removed without notice in
        /////     any release. You should only use it directly in your code with extreme caution and knowing that
        /////     doing so can result in application failures when updating to a new Entity Framework Core release.
        ///// </summary>
        //[SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        //protected override IEnumerable<MigrationOperation> Remove(IKey source,
        //    DiffContext diffContext)
        //{
        //    var sourceEntityType = source.DeclaringEntityType.GetRootType();

        //    // sourceEntityType.ClrType 为 NULL
        //    var targetEntityType = diffContext.GetTargetTables()
        //        .FirstOrDefault(p => p.EntityTypes[0].Name == sourceEntityType.Name);

        //    if (targetEntityType?.EntityTypes[0].ClrType.IsDefined<ShardableAttribute>() == true)
        //    {
        //        // 不移除分表实体的前表主键
        //        yield break;
        //    }

        //    MigrationOperation operation;
        //    if (source.IsPrimaryKey())
        //    {
        //        operation = new DropPrimaryKeyOperation
        //        {
        //            Schema = sourceEntityType.GetSchema(),
        //            Table = sourceEntityType.GetTableName(),
        //            Name = source.GetName()
        //        };
        //    }
        //    else
        //    {
        //        operation = new DropUniqueConstraintOperation
        //        {
        //            Schema = sourceEntityType.GetSchema(),
        //            Table = sourceEntityType.GetTableName(),
        //            Name = source.GetName()
        //        };
        //    }

        //    operation.AddAnnotations(MigrationsAnnotations.ForRemove(source));

        //    yield return operation;
        //}

        #endregion

    }
}
