#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Utility;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Web.Mvc
{
    /// <summary>
    /// 注意视图结果。
    /// </summary>
    public class AttentionResult : ViewResult
    {
        /// <summary>
        /// 注意描述符。
        /// </summary>
        public AttentionDescriptor Descriptor { get; set; }


        /// <summary>
        /// 执行结果。
        /// </summary>
        /// <param name="context">给定的控制器上下文。</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.NotNull(nameof(context));

            ViewData.Model = Descriptor;
            ViewName = "Attent";
            context.RouteData.Values["controller"] = "Home";

            ViewEngineResult result = ViewEngines.Engines.FindView(context, ViewName, null);
            if (null == result.View)
            {
                throw new InvalidOperationException(FormatErrorMessage(ViewName, result.SearchedLocations));
            }
            try
            {
                ViewContext viewContext = new ViewContext(context, result.View, ViewData, TempData, context.HttpContext.Response.Output);
                result.View.Render(viewContext, viewContext.Writer);
            }
            finally
            {
                result.ViewEngine.ReleaseView(context, result.View);
            }


            //ViewEngineResult result = null;

            //if (View == null)
            //{
            //    result = FindView(context);
            //    View = result.View;
            //}

            //TextWriter writer = context.HttpContext.Response.Output;
            //ViewContext viewContext = new ViewContext(context, View, ViewData, TempData, writer);
            //View.Render(viewContext, writer);

            //if (result != null)
            //{
            //    result.ViewEngine.ReleaseView(context, View);
            //}
        }

        private string FormatErrorMessage(string viewName, IEnumerable<string> searchedLocations)
        {
            string format = "The view '{0}' or its master was not found or no view engine supports the searched locations. The following locations were searched:{1}";
            StringBuilder builder = new StringBuilder();
            foreach (string str in searchedLocations)
            {
                builder.AppendLine();
                builder.Append(str);
            }
            return string.Format(CultureInfo.CurrentCulture, format, viewName, builder);
        }

    }
}
