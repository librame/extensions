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
using FluentNHibernate.Automapping;
using System;

namespace Librame.Data.Mappings
{
    /// <summary>
    /// NHibernate 自映射配置。
    /// </summary>
    public class HibernateAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        /// <summary>
        /// 当前日志对象。
        /// </summary>
        protected readonly static ILog Log = LibrameArchitecture.Logging.GetLogger<Providers.HibernateProvider>();

        
        /// <summary>
        /// 是否应该映射当前类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public override bool ShouldMap(Type type)
        {
            // 默认会排除抽象类与接口
            bool result = DataHelper.ShouldAutomapping(type);
            if (result)
            {
                Log.Debug("Register entity type: " + type.FullName);
            }

            return result;
        }

    }
}
