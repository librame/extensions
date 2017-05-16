using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Librame.Forms.Material
{
    /// <summary>
    /// Material design-like progress bar
    /// </summary>
    public class MaterialProgressBar : ProgressBar, IMaterialControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialProgressBar"/> class.
        /// </summary>
        public MaterialProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

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
            get { return LibrameArchitecture.Adapters.Forms.Skin; }
        }

        /// <summary>
        /// 获取或设置进度笔刷。
        /// </summary>
        /// <remarks>
        /// 默认为 <see cref="MaterialColorScheme.AccentBrush"/>。
        /// </remarks>
        [Browsable(false)]
        public Brush ProgressingBrush { get; set; }

        /// <summary>
        /// Performs the work of setting the specified bounds of this control.
        /// </summary>
        /// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
        /// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
        /// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
        /// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height" /> property value of the control.</param>
        /// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, 5, specified);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (ReferenceEquals(ProgressingBrush, null))
            {
                ProgressingBrush = Skin.Scheme.Brushes.Accent;
            }

            var doneProgress = (int)(e.ClipRectangle.Width * ((double)Value / Maximum));
            e.Graphics.FillRectangle(ProgressingBrush, 0, 0, doneProgress, e.ClipRectangle.Height);
            e.Graphics.FillRectangle(Skin.Scheme.Texts.DisabledOrHint.Brush, doneProgress, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
        }
    }
}
