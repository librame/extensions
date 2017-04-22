#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Librame.Forms
{
    using Material;
    using Schemes;
    using Utility;
    
    /// <summary>
    /// 皮肤提供程序。
    /// </summary>
    public class SkinProvider : ISkinProvider
    {
        /// <summary>
        /// 获取 <see cref="Settings"/>。
        /// </summary>
        public FormsSettings Settings { get; }

        /// <summary>
        /// 获取 <see cref="ISchemeBuilder"/>。
        /// </summary>
        public ISchemeBuilder Scheme { get; }
        
        /// <summary>
        /// 构造一个 <see cref="SkinProvider"/> 实例。
        /// </summary>
        /// <param name="settings">给定的 <see cref="FormsSettings"/>。</param>
        /// <param name="scheme">给定的 <see cref="ISchemeBuilder"/>。</param>
        public SkinProvider(FormsSettings settings, ISchemeBuilder scheme)
        {
            Settings = settings.NotNull(nameof(settings));
            Scheme = scheme.NotNull(nameof(scheme));
        }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name
        {
            get { return "Default"; }
        }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description
        {
            get { return "以蓝色为主色调"; }
        }


        //[DllImport("gdi32.dll")]
        //private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);

        private readonly IList<MaterialForm> formsToManage = new List<MaterialForm>();

        /// <summary>
        /// 添加窗体到管理列表。
        /// </summary>
        /// <param name="materialForm">给定的 <see cref="MaterialForm"/>。</param>
        public void AddFormToManage(MaterialForm materialForm)
        {
            formsToManage.Add(materialForm);
            UpdateBackgrounds();
        }

        /// <summary>
        /// 从管理列表中移除窗体。
        /// </summary>
        /// <param name="materialForm">给定的 <see cref="MaterialForm"/>。</param>
        public void RemoveFormToManage(MaterialForm materialForm)
        {
            formsToManage.Remove(materialForm);
        }

        private void UpdateBackgrounds()
        {
            var newBackColor = Scheme.Background.Color;
            foreach (var materialForm in formsToManage)
            {
                materialForm.BackColor = newBackColor;
                UpdateControl(materialForm, newBackColor);
            }
        }

        private void UpdateToolStrip(ToolStrip toolStrip, Color newBackColor)
        {
            if (toolStrip == null) return;

            toolStrip.BackColor = newBackColor;
            foreach (ToolStripItem control in toolStrip.Items)
            {
                control.BackColor = newBackColor;
                if (control is MaterialToolStripMenuItem && (control as MaterialToolStripMenuItem).HasDropDownItems)
                {
                    //recursive call
                    UpdateToolStrip((control as MaterialToolStripMenuItem).DropDown, newBackColor);
                }
            }
        }

        private void UpdateControl(Control controlToUpdate, Color newBackColor)
        {
            if (controlToUpdate == null) return;

            if (controlToUpdate.ContextMenuStrip != null)
            {
                UpdateToolStrip(controlToUpdate.ContextMenuStrip, newBackColor);
            }
            var tabControl = controlToUpdate as MaterialTabControl;
            if (tabControl != null)
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    tabPage.BackColor = newBackColor;
                }
            }

            if (controlToUpdate is MaterialDivider)
            {
                controlToUpdate.BackColor = Scheme.Texts.Dividers.Color;
            }

            if (controlToUpdate is MaterialListView)
            {
                controlToUpdate.BackColor = newBackColor;

            }

            //recursive call
            foreach (Control control in controlToUpdate.Controls)
            {
                UpdateControl(control, newBackColor);
            }

            controlToUpdate.Invalidate();
        }

    }
}
