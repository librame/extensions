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

namespace System.Web.Mvc
{
    /// <summary>
    /// MVC 处理程序错误特性。
    /// </summary>
    public class MvcHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 开始异常处理。
        /// </summary>
        /// <param name="filterContext">给定的 <see cref="ExceptionContext"/>。</param>
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var obj = new
            {
                Message = filterContext.Exception.InnerMessage(),
                StatusCode = filterContext.RequestContext.HttpContext.Response.StatusCode
            };

            // 记录错误日志
            var logger = LibrameArchitecture.Logging.GetLogger(ExceptionType);
            logger.Error(obj.StatusCode, filterContext.Exception);

            if (string.IsNullOrEmpty(View))
            {
                if (obj.StatusCode == 404)
                    View = "/Views/Error/FileNotFound.cshtml";

                else if (obj.StatusCode == 500)
                    View = "/Views/Error/InternalServer.cshtml";
            }

            if (!string.IsNullOrEmpty(View))
            {
                // 设置为 True 阻止 Golbal 里面的错误执行
                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult()
                {
                    ViewName = View
                };
            }
        }

    }

}
