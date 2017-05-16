using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Librame.Forms.Material
{
	public class MaterialMenuStrip : MenuStrip, IMaterialControl
	{
        /// <summary>
        /// 获取或设置深度。
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 获取或设置鼠标状态。
        /// </summary>
        public MouseState MouseState { get; set; }

        /// <summary>
        /// 获取皮肤。
        /// </summary>
        public ISkinProvider Skin
        {
            get { return LibrameArchitecture.Adapters.Forms.Skin; }
        }

        public MaterialMenuStrip()
		{
			Renderer = new MaterialMenuStripRender();

			if (DesignMode)
			{
				Dock = DockStyle.None;
				Anchor |= AnchorStyles.Right;
				AutoSize = false;
				Location = new Point(0, 28);
			}
		}
		
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			Font = Skin.Scheme.Fonts.Medium10;
			BackColor = Skin.Scheme.Colors.Primary;
		}
	}

	internal class MaterialMenuStripRender : ToolStripProfessionalRenderer, IMaterialControl
	{
		//Properties for managing the material design properties
		public int Depth { get; set; }

        public ISkinProvider Skin { get { return LibrameArchitecture.Adapters.Forms.Skin; } }

        public MouseState MouseState { get; set; }

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			var g = e.Graphics;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;

			if (e.Item.IsOnDropDown)
			{
				var itemRect = GetItemRect(e.Item);
				var textRect = new Rectangle(24, itemRect.Y, itemRect.Width - (24 + 16), itemRect.Height);
				g.DrawString(e.Text, Skin.Scheme.Fonts.Medium10, e.Item.Enabled ? Skin.Scheme.Brushes.TextShade : Skin.Scheme.Texts.DisabledOrHint.Brush, textRect, new StringFormat() { LineAlignment = StringAlignment.Center });
			}
			else
			{
				g.DrawString(e.Text, Skin.Scheme.Fonts.Medium10, Brushes.White, e.TextRectangle, new StringFormat() { LineAlignment = StringAlignment.Center });
			}
		}

		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			var g = e.Graphics;
			g.Clear(Skin.Scheme.Colors.Primary);

			//Draw background
			var itemRect = GetItemRect(e.Item);
			if (e.Item.IsOnDropDown)
			{
				g.FillRectangle(e.Item.Selected && e.Item.Enabled ? Skin.Scheme.ContextMenuStrip.Brush : Skin.Scheme.Background.Brush, itemRect);
			}
			else
			{
				g.FillRectangle(e.Item.Selected ? Skin.Scheme.FlatButton.BackgroundPressed.Brush : Skin.Scheme.Brushes.Primary, itemRect);
			}

			//Ripple animation
			var toolStrip = e.ToolStrip as MaterialContextMenuStrip;
			if (toolStrip != null)
			{
				var animationManager = toolStrip.animationManager;
				var animationSource = toolStrip.animationSource;
				if (toolStrip.animationManager.IsAnimating() && e.Item.Bounds.Contains(animationSource))
				{
					for (int i = 0; i < animationManager.GetAnimationCount(); i++)
					{
						var animationValue = animationManager.GetProgress(i);
						var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.Black));
						var rippleSize = (int)(animationValue * itemRect.Width * 2.5);
						g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, itemRect.Y - itemRect.Height, rippleSize, itemRect.Height * 3));
					}
				}
			}
		}

		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			//base.OnRenderImageMargin(e);
		}

		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			var g = e.Graphics;

			g.FillRectangle(new SolidBrush(Skin.Scheme.Background.Color), e.Item.Bounds);
			g.DrawLine(new Pen(Skin.Scheme.Texts.Dividers.Color), new Point(e.Item.Bounds.Left, e.Item.Bounds.Height / 2), new Point(e.Item.Bounds.Right, e.Item.Bounds.Height / 2));
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			//var g = e.Graphics;

			//g.DrawRectangle(new Pen(Skin.Scheme.Texts.Dividers.Color), new Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
		}

		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			var g = e.Graphics;
			const int ARROW_SIZE = 4;

			var arrowMiddle = new Point(e.ArrowRectangle.X + e.ArrowRectangle.Width / 2, e.ArrowRectangle.Y + e.ArrowRectangle.Height / 2);
			var arrowBrush = e.Item.Enabled ? Skin.Scheme.Brushes.TextShade : Skin.Scheme.Texts.DisabledOrHint.Brush;
			using (var arrowPath = new GraphicsPath())
			{
				arrowPath.AddLines(new[] { new Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y - ARROW_SIZE), new Point(arrowMiddle.X, arrowMiddle.Y), new Point(arrowMiddle.X - ARROW_SIZE, arrowMiddle.Y + ARROW_SIZE) });
				arrowPath.CloseFigure();

				g.FillPath(arrowBrush, arrowPath);
			}
		}

		private Rectangle GetItemRect(ToolStripItem item)
		{
			return new Rectangle(0, item.ContentRectangle.Y, item.ContentRectangle.Width + 4, item.ContentRectangle.Height);
		}
	}
}
