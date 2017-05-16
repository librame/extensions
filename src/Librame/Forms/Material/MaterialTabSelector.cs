using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Librame.Forms.Material
{
    using Animations;

    /// <summary>
    /// 质感标签选择器。
    /// </summary>
    public class MaterialTabSelector : Control, IMaterialControl
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
            get { return LibrameArchitecture.Adapters.Forms.Skin; }
        }


        private MaterialTabControl _baseTabControl;
        /// <summary>
        /// 获取或设置基础标签控件。
        /// </summary>
        public MaterialTabControl BaseTabControl
        {
            get { return _baseTabControl; }
            set
            {
                _baseTabControl = value;
                if (_baseTabControl == null) return;
                previousSelectedTabIndex = _baseTabControl.SelectedIndex;
                _baseTabControl.Deselected += (sender, args) =>
                {
                    previousSelectedTabIndex = _baseTabControl.SelectedIndex;
                };
                _baseTabControl.SelectedIndexChanged += (sender, args) =>
                {
                    animationManager.SetProgress(0);
                    animationManager.StartNewAnimation(AnimationDirection.In);
                };
                _baseTabControl.ControlAdded += delegate
                {
                    Invalidate();
                };
                _baseTabControl.ControlRemoved += delegate
                {
                    Invalidate();
                };
            }
        }

        private int previousSelectedTabIndex;
        private Point animationSource;
        private readonly AnimationManager animationManager;

        private List<Rectangle> tabRects;
        private const int TAB_HEADER_PADDING = 24;
        private const int TAB_INDICATOR_HEIGHT = 2;


        /// <summary>
        /// 获取或设置平均宽度。
        /// </summary>
        public bool AverageWidths { get; set; }


        /// <summary>
        /// 构造一个 <see cref="MaterialTabSelector"/> 实例。
        /// </summary>
        public MaterialTabSelector()
            : this(48, true)
        {
        }
        /// <summary>
        /// 构造一个 <see cref="MaterialTabSelector"/> 实例。
        /// </summary>
        /// <param name="height">给定的高度。</param>
        /// <param name="averageWidths">是否使用平均宽度。</param>
        public MaterialTabSelector(int height, bool averageWidths)
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            Height = height;
            AverageWidths = averageWidths;

            animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseOut,
                Increment = 0.04
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
        }

        /// <summary>
        /// 开始绘制控件。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = Skin.Settings.TextRendering;

			g.Clear(Skin.Scheme.Colors.Primary);

            if (_baseTabControl == null) return;

            if (!animationManager.IsAnimating() || tabRects == null ||  tabRects.Count != _baseTabControl.TabCount)
                UpdateTabRects();

            double animationProgress = animationManager.GetProgress();

            //Click feedback
            if (animationManager.IsAnimating())
            {
                var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationProgress * 50)), Color.White));
                var rippleSize = (int)(animationProgress * tabRects[_baseTabControl.SelectedIndex].Width * 1.75);

                g.SetClip(tabRects[_baseTabControl.SelectedIndex]);
                g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                g.ResetClip();
                rippleBrush.Dispose();
            }

            //Draw tab headers
            foreach (TabPage tabPage in _baseTabControl.TabPages)
            {
                int currentTabIndex = _baseTabControl.TabPages.IndexOf(tabPage);
				Brush textBrush = new SolidBrush(Color.FromArgb(CalculateTextAlpha(currentTabIndex, animationProgress), Skin.Scheme.Colors.TextShade));

                // tabPage.Text.ToUpper()
                g.DrawString(
                    tabPage.Text, 
                    Skin.Scheme.Fonts.Medium10, 
                    textBrush, 
                    tabRects[currentTabIndex], 
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                textBrush.Dispose();
            }

            //Animate tab indicator
            int previousSelectedTabIndexIfHasOne = previousSelectedTabIndex == -1 ? _baseTabControl.SelectedIndex : previousSelectedTabIndex;
            Rectangle previousActiveTabRect = tabRects[previousSelectedTabIndexIfHasOne];
            Rectangle activeTabPageRect = tabRects[_baseTabControl.SelectedIndex];

            int y = activeTabPageRect.Bottom - 2;
            int x = previousActiveTabRect.X + (int)((activeTabPageRect.X - previousActiveTabRect.X) * animationProgress);
            int width = previousActiveTabRect.Width + (int)((activeTabPageRect.Width - previousActiveTabRect.Width) * animationProgress);

			g.FillRectangle(Skin.Scheme.Brushes.Accent, x, y, width, TAB_INDICATOR_HEIGHT);
        }

        private int CalculateTextAlpha(int tabIndex, double animationProgress)
        {
            int primaryA = Skin.Scheme.ActionBar.Primary.Color.A;
            int secondaryA = Skin.Scheme.ActionBar.Secondary.Color.A;

            if (tabIndex == _baseTabControl.SelectedIndex && !animationManager.IsAnimating())
            {
                return primaryA;
            }
            if (tabIndex != previousSelectedTabIndex && tabIndex != _baseTabControl.SelectedIndex)
            {
                return secondaryA;
            }
            if (tabIndex == previousSelectedTabIndex)
            {
                return primaryA - (int)((primaryA - secondaryA) * animationProgress);
            }
            return secondaryA + (int)((primaryA - secondaryA) * animationProgress);
        }

        /// <summary>
        /// 鼠标释放。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (tabRects == null) UpdateTabRects();
            for (int i = 0; i < tabRects.Count; i++)
            {
                if (tabRects[i].Contains(e.Location))
                {
                    _baseTabControl.SelectedIndex = i;
                }
            }

            animationSource = e.Location;
        }

        private void UpdateTabRects()
        {
            tabRects = new List<Rectangle>();
            
            //If there isn't a base tab control, the rects shouldn't be calculated
            //If there aren't tab pages in the base tab control, the list should just be empty which has been set already; exit the void
            if (_baseTabControl == null || _baseTabControl.TabCount == 0) return;
            
            var averageWidth = (Width - Skin.Settings.FormPadding * 2) / _baseTabControl.TabPages.Count;

            //Calculate the bounds of each tab header specified in the base tab control
            using (var b = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(b))
                {
                    int width = averageWidth;
                    if (!AverageWidths)
                    {
                        width = CalculateSelectorWidth(g, _baseTabControl.TabPages[0].Text);
                    }

                    tabRects.Add(new Rectangle(Skin.Settings.FormPadding, 0, width, Height));

                    for (int i = 1; i < _baseTabControl.TabPages.Count; i++)
                    {
                        if (!AverageWidths)
                        {
                            width = CalculateSelectorWidth(g, _baseTabControl.TabPages[i].Text);
                        }

                        tabRects.Add(new Rectangle(tabRects[i - 1].Right, 0, width, Height));
                    }
                }
            }
        }

        private int CalculateSelectorWidth(Graphics g, string text)
        {
            return TAB_HEADER_PADDING * 2 + (int)g.MeasureString(text, Skin.Scheme.Fonts.Medium10).Width;
        }
    }
}
