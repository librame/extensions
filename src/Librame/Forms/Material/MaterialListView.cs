using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Librame.Forms.Material
{
	public class MaterialListView : ListView, IMaterialControl
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
            get { return LibrameArchitecture.AdapterManager.FormsAdapter.Skin; }
        }


		public MaterialListView()
		{
			GridLines = false;
			FullRowSelect = true;
			HeaderStyle = ColumnHeaderStyle.Nonclickable;
			View = View.Details;
			OwnerDraw = true;
			ResizeRedraw = true;
			BorderStyle = BorderStyle.None;
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            //Fix for hovers, by default it doesn't redraw
            //TODO: should only redraw when the hovered line changed, this to reduce unnecessary redraws
            MouseLocation = new Point(-1, -1);
			MouseState = MouseState.Out;
			MouseEnter += delegate
			{
				MouseState = MouseState.Hover;
			}; 
			MouseLeave += delegate
			{
				MouseState = MouseState.Out; 
				MouseLocation = new Point(-1, -1);
				Invalidate();
			};
			MouseDown += delegate { MouseState = MouseState.Down; };
			MouseUp += delegate{ MouseState = MouseState.Hover; };
			MouseMove += delegate(object sender, MouseEventArgs args)
			{
				MouseLocation = args.Location;
				Invalidate();
			};
		}

		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
            e.Graphics.FillRectangle(new SolidBrush(Skin.Scheme.Background.Color), new Rectangle(e.Bounds.X, e.Bounds.Y, Width, e.Bounds.Height));

            e.Graphics.DrawString(e.Header.Text,
				Skin.Scheme.Fonts.Medium10,
				Skin.Scheme.Texts.Secondary.Brush,
				new Rectangle(e.Bounds.X + ITEM_PADDING, e.Bounds.Y + ITEM_PADDING, e.Bounds.Width - ITEM_PADDING * 2, e.Bounds.Height - ITEM_PADDING * 2),
				getStringFormat());
		}

		private const int ITEM_PADDING = 12;
		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			//We draw the current line of items (= item with subitems) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
			var b = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
			var g = Graphics.FromImage(b);
            
            //always draw default background
            g.FillRectangle(new SolidBrush(Skin.Scheme.Background.Color), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			
			if (e.State.HasFlag(ListViewItemStates.Selected))
			{
				//selected background
				g.FillRectangle(Skin.Scheme.FlatButton.BackgroundPressed.Brush, new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			}
			else if (e.Bounds.Contains(MouseLocation) && MouseState == MouseState.Hover)
			{
				//hover background
				g.FillRectangle(Skin.Scheme.FlatButton.BackgroundHover.Brush, new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			}


			//Draw separator
			g.DrawLine(new Pen(Skin.Scheme.Texts.Dividers.Color), e.Bounds.Left, 0, e.Bounds.Right, 0);
			
			foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
			{
				//Draw text
				g.DrawString(subItem.Text, Skin.Scheme.Fonts.Medium10, Skin.Scheme.Texts.Primary.Brush,
								 new Rectangle(subItem.Bounds.Location.X + ITEM_PADDING, ITEM_PADDING, subItem.Bounds.Width - 2 * ITEM_PADDING, subItem.Bounds.Height - 2 * ITEM_PADDING),
								 getStringFormat());
			}

			e.Graphics.DrawImage((Image) b.Clone(), e.Item.Bounds.Location);
			g.Dispose();
			b.Dispose();
		}

		private StringFormat getStringFormat()
		{
			return new StringFormat
			{
				FormatFlags = StringFormatFlags.LineLimit,
				Trimming = StringTrimming.EllipsisCharacter,
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center
			};
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();

            //This is a hax for the needed padding.
            //Another way would be intercepting all ListViewItems and changing the sizes, but really, that will be a lot of work
            //This will do for now.
            Font = new Font(Skin.Scheme.Fonts.Medium12.FontFamily, 24);
        }
	}
}
