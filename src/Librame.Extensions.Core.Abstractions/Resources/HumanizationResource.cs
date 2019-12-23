#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Resources
{
    /// <summary>
    /// 人性化资源。
    /// </summary>
    public class HumanizationResource : IResource
    {
        /// <summary>
        /// 人性化分钟数以前。
        /// </summary>
        public string HumanizedMinutesAgo { get;set; }

        /// <summary>
        /// 人性化小时数以前。
        /// </summary>
        public string HumanizedHoursAgo { get; set; }

        /// <summary>
        /// 人性化天数以前。
        /// </summary>
        public string HumanizedDaysAgo { get; set; }

        /// <summary>
        /// 人性化月数以前。
        /// </summary>
        public string HumanizedMonthsAgo { get; set; }

        /// <summary>
        /// 人性化年数以前。
        /// </summary>
        public string HumanizedYearsAgo { get; set; }
    }
}
