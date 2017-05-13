using System.ComponentModel;
using System.Windows.Forms;

namespace Librame.Forms.Material
{
    public class MaterialLabel : Label, IMaterialControl
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

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            
            ForeColor = Skin.Scheme.Texts.Primary.Color;
            Font = Skin.Scheme.Fonts.Regular11;

            BackColorChanged += (sender, args) => ForeColor = Skin.Scheme.Texts.Primary.Color;
        }
    }
}
