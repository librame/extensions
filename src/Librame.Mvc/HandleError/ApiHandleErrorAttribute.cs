#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame;
using Librame.Utility;
using System.Net.Http;

namespace System.Web.Http.Filters
{
    /// <summary>
    /// API 处理程序错误特性。
    /// </summary>
    public class ApiHandleErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 开始异常处理。
        /// </summary>
        /// <param name="filterContext">给定的 <see cref="HttpActionExecutedContext"/>。</param>
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            base.OnException(filterContext);

            var obj = new
            {
                Message = filterContext.Exception.InnerMessage(),
                StatusCode = filterContext.Response.StatusCode
            };

            // 记录错误日志
            var logger = LibrameArchitecture.LoggingAdapter.GetLogger<ApiHandleErrorAttribute>();
            logger.Error(obj.Message, filterContext.Exception);
            
            filterContext.Response = new HttpResponseMessage()
            {
                StatusCode = obj.StatusCode,
                Content = new StringContent(obj.AsJson())
            };
        }

    }
}
