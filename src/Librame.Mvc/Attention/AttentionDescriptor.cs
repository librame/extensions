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
    /// 注意描述符。
    /// </summary>
    public class AttentionDescriptor
    {
        /// <summary>
        /// 标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 引用链接。
        /// </summary>
        public string ReferUrl { get; set; }

        /// <summary>
        /// 级别。
        /// </summary>
        public AttentionLevel Level { get; set; }

        /// <summary>
        /// 异常。
        /// </summary>
        public Exception Exception { get; set; }
    }
}
