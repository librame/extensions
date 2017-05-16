#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Runtime.InteropServices;

namespace Librame.Resource.Schema
{
    /// <summary>
    /// 格式结构部分。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class FormatSchemaSection : AbstractSchemaSection
    {
        /// <summary>
        /// 格式部分名称。
        /// </summary>
        protected override string SectionName
        {
            get { return "Format"; }
        }


        /// <summary>
        /// 格式标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 格式扩展名。
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 格式版本。
        /// </summary>
        public string Version { get; set; }
    }
}
