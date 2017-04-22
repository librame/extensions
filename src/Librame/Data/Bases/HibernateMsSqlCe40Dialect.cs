#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using NHibernate.Dialect;

namespace Librame.Data.Bases
{
    /// <summary>
    /// NHibernate MsSqlCe40 数据库方言。
    /// </summary>
    public class HibernateMsSqlCe40Dialect : MsSqlCe40Dialect
    {
        /// <summary>
        /// 支持变量限制。
        /// </summary>
        public override bool SupportsVariableLimit
        {
            get { return true; }
        }

    }
}
