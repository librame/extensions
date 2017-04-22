#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Drawing.Text;

namespace Librame.Forms
{
    /// <summary>
    /// 窗体首选项。
    /// </summary>
    public class FormsSettings : Adaptation.AbstractAdapterSettings, Adaptation.IAdapterSettings
    {
        /// <summary>
        /// 窗体内距。
        /// </summary>
        public int FormPadding { get; set; }

        /// <summary>
        /// 获取 <see cref="TextRenderingHint"/>。
        /// </summary>
        public TextRenderingHint TextRendering { get; set; }
    }
}
