using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Librame.Forms.Material.Animations;

namespace Librame.Forms.Material
{
    public class MaterialRadioButton : RadioButton, IMaterialControl
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
        /// 获取或设置鼠标位置。
        /// </summary>
        [Browsable(false)]
        public Point MouseLocation { get; set; }

        /// <summary>
        /// 获取皮肤。
        /// </summary>
        [Browsable(false)]
        public ISkinProvider Skin
        {
            get { return LibrameArchitecture.AdapterManager.Forms.Skin; }
        }

        private bool ripple;
        [Category("Behavior")]
        public bool Ripple
        {
            get { return ripple; }
            set
            {
                ripple = value;
                AutoSize = AutoSize; //Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

		// animation managers
        private readonly AnimationManager animationManager;
        private readonly AnimationManager rippleAnimationManager;

		// size related variables which should be recalculated onsizechanged
		private Rectangle radioButtonBounds;
        private int boxOffset;

		// size constants
		private const int RADIOBUTTON_SIZE = 19;
		private const int RADIOBUTTON_SIZE_HALF = RADIOBUTTON_SIZE / 2;
		private const int RADIOBUTTON_OUTER_CIRCLE_WIDTH = 2;
		private const int RADIOBUTTON_INNER_CIRCLE_SIZE = RADIOBUTTON_SIZE - (2 * RADIOBUTTON_OUTER_CIRCLE_WIDTH);

        public MaterialRadioButton()
		{
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.06
            };
            rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
            rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged += (sender, args) => animationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);
			
            SizeChanged += OnSizeChanged;

            Ripple = true;
            MouseLocation = new Point(-1, -1);
        }
        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            boxOffset = Height / 2 - (int)Math.Ceiling(RADIOBUTTON_SIZE / 2d);
            radioButtonBounds = new Rectangle(boxOffset, boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            int width = boxOffset + 20 + (int)CreateGraphics().MeasureString(Text, Skin.Scheme.Fonts.Medium10).Width;
            return Ripple ? new Size(width, 30) : new Size(width, 20);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = Skin.Settings.TextRendering;

            // clear the control
            g.Clear(Parent.BackColor);

            var RADIOBUTTON_CENTER = boxOffset + RADIOBUTTON_SIZE_HALF;

            var animationProgress = animationManager.GetProgress();

            int colorAlpha = Enabled ? (int)(animationProgress * 255.0) : Skin.Scheme.CheckboxOff.Disabled.Color.A;
            int backgroundAlpha = Enabled ? (int)(Skin.Scheme.CheckboxOff.Enable.Color.A * (1.0 - animationProgress)) : Skin.Scheme.CheckboxOff.Disabled.Color.A;
            float animationSize = (float)(animationProgress * 8f);
            float animationSizeHalf = animationSize / 2;
            animationSize = (float)(animationProgress * 9f);

			var brush = new SolidBrush(Color.FromArgb(colorAlpha, Enabled ? Skin.Scheme.Colors.Accent : Skin.Scheme.CheckboxOff.Disabled.Color));
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (Ripple && rippleAnimationManager.IsAnimating())
            {
                for (int i = 0; i < rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = rippleAnimationManager.GetProgress(i);
                    var animationSource = new Point(RADIOBUTTON_CENTER, RADIOBUTTON_CENTER);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)((animationValue * 40)), ((bool)rippleAnimationManager.GetData(i)[0]) ? Color.Black : brush.Color));
                    var rippleHeight = (Height % 2 == 0) ? Height - 3 : Height - 2;
                    var rippleSize = (rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? (int)(rippleHeight * (0.8d + (0.2d * animationValue))) : rippleHeight;
                    using (var path = DrawHelper.CreateRoundRect(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize, rippleSize / 2))
                    {
                        g.FillPath(rippleBrush, path);
                    }

                    rippleBrush.Dispose();
                }
            }

            // draw radiobutton circle
            Color uncheckedColor = DrawHelper.BlendColor(Parent.BackColor, Enabled ? Skin.Scheme.CheckboxOff.Enable.Color : Skin.Scheme.CheckboxOff.Disabled.Color, backgroundAlpha);

            using (var path = DrawHelper.CreateRoundRect(boxOffset, boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE, 9f))
            {
                g.FillPath(new SolidBrush(uncheckedColor), path);

	            if (Enabled)
	            {
                    g.FillPath(brush, path);
	            }
            }

            g.FillEllipse(
                new SolidBrush(Parent.BackColor), 
                RADIOBUTTON_OUTER_CIRCLE_WIDTH + boxOffset, 
                RADIOBUTTON_OUTER_CIRCLE_WIDTH + boxOffset, 
                RADIOBUTTON_INNER_CIRCLE_SIZE,
                RADIOBUTTON_INNER_CIRCLE_SIZE);

            if (Checked)
            {
                using (var path = DrawHelper.CreateRoundRect(RADIOBUTTON_CENTER - animationSizeHalf, RADIOBUTTON_CENTER - animationSizeHalf, animationSize, animationSize, 4f))
                {
                    g.FillPath(brush, path);
                }
            }
            SizeF stringSize = g.MeasureString(Text, Skin.Scheme.Fonts.Medium10);
            g.DrawString(Text, Skin.Scheme.Fonts.Medium10, Enabled ? Skin.Scheme.Texts.Primary.Brush : Skin.Scheme.Texts.DisabledOrHint.Brush, boxOffset + 22, Height / 2 - stringSize.Height / 2);

            brush.Dispose();  
            pen.Dispose();
        }

        private bool IsMouseInCheckArea()
        {
            return radioButtonBounds.Contains(MouseLocation);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Font = Skin.Scheme.Fonts.Medium10;

            if (DesignMode) return;

            MouseState = MouseState.Out;
            MouseEnter += (sender, args) =>
            {
                MouseState = MouseState.Hover;
            };
            MouseLeave += (sender, args) =>
            {
                MouseLocation = new Point(-1, -1);
                MouseState = MouseState.Out;
            };
            MouseDown += (sender, args) =>
            {
                MouseState = MouseState.Down;

                if (Ripple && args.Button == MouseButtons.Left && IsMouseInCheckArea())
                {
                    rippleAnimationManager.SecondaryIncrement = 0;
                    rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Checked });
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.Hover;
                rippleAnimationManager.SecondaryIncrement = 0.08;
            };
            MouseMove += (sender, args) =>
            {
                MouseLocation = args.Location;
                Cursor = IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default;
            };
        }
    }
}
