using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Librame.Forms.Material.Animations;

namespace Librame.Forms.Material
{
    public class MaterialRaisedButton : Button, IMaterialControl
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

        private readonly AnimationManager animationManager;

        public MaterialRaisedButton()
        {
            //Primary = true;

            animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            animationManager.StartNewAnimation(AnimationDirection.In, mevent.Location);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = Skin.Settings.TextRendering;

            g.Clear(Parent.BackColor);

            using (var backgroundPath = DrawHelper.CreateRoundRect(ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width - 1,
                ClientRectangle.Height - 1,
                1f))
            {
                //g.FillPath(Primary ? Skin.Scheme.Brushes.Primary : Skin.Scheme.RaisedButton.Background.Brush, backgroundPath);
                g.FillPath(Skin.Scheme.Brushes.Primary, backgroundPath);
            }

            if (animationManager.IsAnimating())
            {
                for (int i = 0; i < animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = animationManager.GetProgress(i);
                    var animationSource = animationManager.GetSource(i);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.White));
                    var rippleSize = (int)(animationValue * Width * 2);
                    g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                }
            }

            //Text.ToUpper()
            g.DrawString(
                Text,
                Skin.Scheme.Fonts.Medium10,
                Skin.Scheme.RaisedButton.Text.Brush,
                ClientRectangle,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
    }
}
