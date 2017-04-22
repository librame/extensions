#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Forms.Schemes
{
    /// <summary>
    /// 方案构建器。
    /// </summary>
    public class SchemeBuilder : ISchemeBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="SchemeBuilder"/> 实例。
        /// </summary>
        public SchemeBuilder()
            : this(ColorPrimaryScheme.Blue, SchemeType.WhiteOrLight)
        {
        }
        /// <summary>
        /// 构造一个 <see cref="SchemeBuilder"/> 实例。
        /// </summary>
        /// <param name="colors">给定的颜色主要方案。</param>
        /// <param name="schemeType">给定的方案类型。</param>
        protected SchemeBuilder(ColorPrimaryScheme colors, SchemeType schemeType)
        {
            Colors = colors;
            SchemeType = schemeType;
        }


        /// <summary>
        /// 获取 <see cref="ColorPrimaryScheme"/>。
        /// </summary>
        public virtual ColorPrimaryScheme Colors { get; protected set; }

        /// <summary>
        /// 获取 <see cref="PenPrimaryScheme"/>。
        /// </summary>
        public virtual PenPrimaryScheme Pens
        {
            get { return new PenPrimaryScheme(Colors); }
        }

        /// <summary>
        /// 获取 <see cref="BrushPrimaryScheme"/>。
        /// </summary>
        public virtual BrushPrimaryScheme Brushes
        {
            get { return new BrushPrimaryScheme(Colors); }
        }


        /// <summary>
        /// 获取方案类型。
        /// </summary>
        public virtual SchemeType SchemeType { get; protected set; }

        /// <summary>
        /// 获取 <see cref="FontScheme"/>。
        /// </summary>
        public virtual FontScheme Fonts
        {
            get { return FontScheme.SegoeUI; }
        }

        /// <summary>
        /// 获取 <see cref="TextScheme"/>。
        /// </summary>
        public virtual TextScheme Texts
        {
            get { return TextScheme.Parse(SchemeType); }
        }

        /// <summary>
        /// 获取 <see cref="CheckboxOffScheme"/>。
        /// </summary>
        public virtual CheckboxOffScheme CheckboxOff
        {
            get { return CheckboxOffScheme.Parse(SchemeType); }
        }

        /// <summary>
        /// 获取 <see cref="RaisedButtonScheme"/>。
        /// </summary>
        public virtual RaisedButtonScheme RaisedButton
        {
            get { return RaisedButtonScheme.Parse(SchemeType); }
        }

        /// <summary>
        /// 获取 <see cref="FlatButtonScheme"/>。
        /// </summary>
        public virtual FlatButtonScheme FlatButton
        {
            get { return FlatButtonScheme.Parse(SchemeType); }
        }

        /// <summary>
        /// 获取 <see cref="ContextMenuStripScheme"/>。
        /// </summary>
        public virtual ContextMenuStripScheme ContextMenuStrip
        {
            get { return ContextMenuStripScheme.Parse(SchemeType); }
        }

        /// <summary>
        /// 获取 <see cref="BackgroundScheme"/>。
        /// </summary>
        public virtual BackgroundScheme Background
        {
            get { return BackgroundScheme.Parse(SchemeType); }
        }


        /// <summary>
        /// 获取 <see cref="ActionBarScheme"/>。
        /// </summary>
        public virtual ActionBarScheme ActionBar
        {
            get { return ActionBarScheme.Default; }
        }

    }
}
