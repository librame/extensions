using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Librame.Forms.Material
{
    /// <summary>
    /// 质感标签控件。
    /// </summary>
    public class MaterialTabControl : TabControl, IMaterialControl
    {
        /// <summary>
        /// 获取或设置深度。
        /// </summary>
        [Browsable(false)]
        public int Depth { get; set; }

        /// <summary>
        /// 获取或设置鼠标状态。
        /// </summary>
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        /// <summary>
        /// 获取皮肤。
        /// </summary>
        [Browsable(false)]
        public ISkinProvider Skin
        {
            get { return LibrameArchitecture.AdapterManager.Forms.Skin; }
        }


        /// <summary>
        /// 处理。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }

    }
}
