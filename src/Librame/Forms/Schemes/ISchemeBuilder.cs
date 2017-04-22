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
    /// 方案构建器接口。
    /// </summary>
    public interface ISchemeBuilder
    {
        /// <summary>
        /// 获取 <see cref="ColorPrimaryScheme"/>。
        /// </summary>
        ColorPrimaryScheme Colors { get; }

        /// <summary>
        /// 获取 <see cref="PenPrimaryScheme"/>。
        /// </summary>
        PenPrimaryScheme Pens { get; }

        /// <summary>
        /// 获取 <see cref="BrushPrimaryScheme"/>。
        /// </summary>
        BrushPrimaryScheme Brushes { get; }


        /// <summary>
        /// 获取方案类型。
        /// </summary>
        SchemeType SchemeType { get; }

        /// <summary>
        /// 获取 <see cref="FontScheme"/>。
        /// </summary>
        FontScheme Fonts { get; }

        /// <summary>
        /// 获取 <see cref="TextScheme"/>。
        /// </summary>
        TextScheme Texts { get; }

        /// <summary>
        /// 获取 <see cref="CheckboxOffScheme"/>。
        /// </summary>
        CheckboxOffScheme CheckboxOff { get; }

        /// <summary>
        /// 获取 <see cref="RaisedButtonScheme"/>。
        /// </summary>
        RaisedButtonScheme RaisedButton { get; }

        /// <summary>
        /// 获取 <see cref="FlatButtonScheme"/>。
        /// </summary>
        FlatButtonScheme FlatButton { get; }

        /// <summary>
        /// 获取 <see cref="ContextMenuStripScheme"/>。
        /// </summary>
        ContextMenuStripScheme ContextMenuStrip { get; }

        /// <summary>
        /// 获取 <see cref="BackgroundScheme"/>。
        /// </summary>
        BackgroundScheme Background { get; }


        /// <summary>
        /// 获取 <see cref="ActionBarScheme"/>。
        /// </summary>
        ActionBarScheme ActionBar { get; }

    }
}
