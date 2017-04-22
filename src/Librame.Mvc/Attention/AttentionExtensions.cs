#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace System.Web.Mvc
{
    /// <summary>
    /// <see cref="AttentionResult"/> 静态扩展。
    /// </summary>
    public static class AttentionExtensions
    {
        /// <summary>
        /// 错误注意动作结果。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="controller">给定的控制器。</param>
        /// <param name="message">给定的消息。</param>
        /// <param name="status">给定的动作状态。</param>
        /// <param name="referUrl">给定的引用链接（可选）。</param>
        /// <param name="exception">给定的异常（可选）。</param>
        /// <returns>返回动作结果。</returns>
        public static ActionResult AttentError<TEntity>(this Controller controller, string message,
            ActionStatus status, string referUrl = null, Exception exception = null)
        {
            var title = ActionHelper.ParseTitle<TEntity>(status);

            return controller.Attent(message, title, referUrl, AttentionLevel.Error, exception);
        }
        /// <summary>
        /// 错误注意动作结果。
        /// </summary>
        /// <param name="controller">给定的控制器。</param>
        /// <param name="message">给定的消息。</param>
        /// <param name="title">给定的标题。</param>
        /// <param name="referUrl">给定的引用链接（可选）。</param>
        /// <param name="exception">给定的异常（可选）。</param>
        /// <returns>返回动作结果。</returns>
        public static ActionResult AttentError(this Controller controller, string message, string title,
            string referUrl = null, Exception exception = null)
        {
            return controller.Attent(title, message, referUrl, AttentionLevel.Error, exception);
        }

        /// <summary>
        /// 成功注意动作结果。
        /// </summary>
        /// <param name="controller">给定的控制器。</param>
        /// <param name="message">给定的消息。</param>
        /// <param name="title">给定的标题。</param>
        /// <param name="referUrl">给定的引用链接（可选）。</param>
        /// <returns>返回动作结果。</returns>
        public static ActionResult AttentSuccess(this Controller controller, string message, string title,
            string referUrl = null)
        {
            return controller.Attent(title, message, referUrl, AttentionLevel.Success);
        }

        /// <summary>
        /// 阻止注意动作结果。
        /// </summary>
        /// <param name="controller">给定的控制器。</param>
        /// <param name="message">给定的消息。</param>
        /// <param name="title">给定的标题。</param>
        /// <param name="referUrl">给定的引用链接（可选）。</param>
        /// <returns>返回动作结果。</returns>
        public static ActionResult AttentBlock(this Controller controller, string message, string title,
            string referUrl = null)
        {
            return controller.Attent(title, message, referUrl, AttentionLevel.Block);
        }

        /// <summary>
        /// 注意动作结果。
        /// </summary>
        /// <param name="controller">给定的控制器。</param>
        /// <param name="message">给定的消息。</param>
        /// <param name="title">给定的标题。</param>
        /// <param name="referUrl">给定的引用链接（可选）。</param>
        /// <param name="level">给定的级别（可选）。</param>
        /// <param name="exception">给定的异常（可选）。</param>
        /// <returns>返回动作结果。</returns>
        public static ActionResult Attent(this Controller controller, string message, string title,
            string referUrl = null, AttentionLevel level = AttentionLevel.Info, Exception exception = null)
        {
            // 默认引用链接为当前动作链接
            if (string.IsNullOrEmpty(referUrl))
                referUrl = controller.Request.UrlReferrer?.ToString();

            return controller.Attent(new AttentionDescriptor()
            {
                Title = title,
                Message = message,
                ReferUrl = referUrl,
                Level = level,
                Exception = exception
            });
        }
        
        /// <summary>
        /// 注意动作结果。
        /// </summary>
        /// <param name="controller">给定的控制器。</param>
        /// <param name="descriptor">给定的描述符。</param>
        /// <returns>返回动作结果。</returns>
        public static ActionResult Attent(this Controller controller, AttentionDescriptor descriptor)
        {
            return new AttentionResult()
            {
                Descriptor = descriptor
            };
        }

    }
}
