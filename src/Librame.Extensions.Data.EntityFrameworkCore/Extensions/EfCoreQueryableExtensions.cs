#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 可查询接口静态扩展。
    /// </summary>
    public static class EfCoreQueryableExtensions
    {
        private static readonly TypeInfo _queryCompilerTypeInfo
            = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo _queryCompilerField
            = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly FieldInfo _queryModelGeneratorField
            = typeof(QueryCompiler).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo _dataBaseField
            = _queryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo _databaseDependenciesField
            = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");


        /// <summary>
        /// 转换为 SQL 查询语句。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="query">给定的 <see cref="IQueryable{TEntity}"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string ToSql<TEntity>(this IQueryable<TEntity> query)
        {
            var queryCompiler = (QueryCompiler)_queryCompilerField.GetValue(query.Provider);
            var queryModelGenerator = (QueryModelGenerator)_queryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(query.Expression);

            var database = _dataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)_databaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);

            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);

            var sql = modelVisitor.Queries.First().ToString();
            return sql;
        }

    }
}
