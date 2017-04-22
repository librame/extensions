#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Forms.Material
{
    /// <summary>
    /// 质感控件接口。
    /// </summary>
    public interface IMaterialControl
    {
        /// <summary>
        /// 获取或设置深度。
        /// </summary>
        int Depth { get; set; }

        /// <summary>
        /// 获取或设置鼠标状态。
        /// </summary>
        MouseState MouseState { get; set; }

        /// <summary>
        /// 获取皮肤。
        /// </summary>
        ISkinProvider Skin { get; }
    }
}
