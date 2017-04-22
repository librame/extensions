#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using NHibernate.Driver;
using NHibernate.SqlTypes;
using System.Data;

namespace Librame.Data.Bases
{
    /// <summary>
    /// NHibernate MsSqlServerCe 数据库驱动。
    /// </summary>
    public class HibernateMsSqlServerCeDriver : SqlServerCeDriver
    {
        //private PropertyInfo _dbParamSqlDbTypeProperty;

        ///// <summary>
        ///// 配置。
        ///// </summary>
        ///// <param name="settings">给定的选项集合。</param>
        //public override void Configure(IDictionary<string, string> settings)
        //{
        //    base.Configure(settings);

        //    using (var cmd = CreateCommand())
        //    {
        //        var dbParam = cmd.CreateParameter();
        //        _dbParamSqlDbTypeProperty = dbParam.GetType().GetProperty("SqlDbType");
        //    }
        //}

        /// <summary>
        /// 初始化参数。
        /// </summary>
        /// <param name="dbParam">给定的 <see cref="IDbDataParameter"/>。</param>
        /// <param name="name">给定的参数名。</param>
        /// <param name="sqlType">给定的参数类型。</param>
        protected override void InitializeParameter(IDbDataParameter dbParam, string name, SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);

            var property = dbParam.GetType().GetProperty("SqlDbType");

            if (sqlType.DbType == DbType.Binary)
            {
                property.SetValue(dbParam, SqlDbType.Image, null);
                return;
            }

            if (sqlType.Length <= 4000)
            {
                return;
            }

            switch (sqlType.DbType)
            {
                case DbType.String:
                    property.SetValue(dbParam, SqlDbType.NText, null);
                    break;

                case DbType.AnsiString:
                    property.SetValue(dbParam, SqlDbType.Text, null);
                    break;
            }
        }

    }
}
