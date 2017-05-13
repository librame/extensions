#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Forms.Schemes
{
    /// <summary>
    /// 强调颜色。
    /// </summary>
    public enum AccentColor
    {
        /// <summary>
        /// 红色100。
        /// </summary>
        [Description("红色100")]
        Red100 = 0xFF8A80,
        /// <summary>
        /// 红色200。
        /// </summary>
        [Description("红色200")]
        Red200 = 0xFF5252,
        /// <summary>
        /// 红色400。
        /// </summary>
        [Description("红色400")]
        Red400 = 0xFF1744,
        /// <summary>
        /// 红色700。
        /// </summary>
        [Description("红色700")]
        Red700 = 0xD50000,

        /// <summary>
        /// 粉色100。
        /// </summary>
        [Description("粉色100")]
        Pink100 = 0xFF80AB,
        /// <summary>
        /// 粉色200。
        /// </summary>
        [Description("粉色200")]
        Pink200 = 0xFF4081,
        /// <summary>
        /// 粉色400。
        /// </summary>
        [Description("粉色400")]
        Pink400 = 0xF50057,
        /// <summary>
        /// 粉色700。
        /// </summary>
        [Description("粉色700")]
        Pink700 = 0xC51162,

        /// <summary>
        /// 紫色100。
        /// </summary>
        [Description("紫色100")]
        Purple100 = 0xEA80FC,
        /// <summary>
        /// 紫色200。
        /// </summary>
        [Description("紫色200")]
        Purple200 = 0xE040FB,
        /// <summary>
        /// 紫色400。
        /// </summary>
        [Description("紫色400")]
        Purple400 = 0xD500F9,
        /// <summary>
        /// 紫色700。
        /// </summary>
        [Description("紫色700")]
        Purple700 = 0xAA00FF,

        /// <summary>
        /// 深紫色100。
        /// </summary>
        [Description("深紫色100")]
        DeepPurple100 = 0xB388FF,
        /// <summary>
        /// 深紫色200。
        /// </summary>
        [Description("深紫色200")]
        DeepPurple200 = 0x7C4DFF,
        /// <summary>
        /// 深紫色400。
        /// </summary>
        [Description("深紫色400")]
        DeepPurple400 = 0x651FFF,
        /// <summary>
        /// 深紫色700。
        /// </summary>
        [Description("深紫色700")]
        DeepPurple700 = 0x6200EA,

        /// <summary>
        /// 靛蓝色100。
        /// </summary>
        [Description("靛蓝色100")]
        Indigo100 = 0x8C9EFF,
        /// <summary>
        /// 靛蓝色200。
        /// </summary>
        [Description("靛蓝色200")]
        Indigo200 = 0x536DFE,
        /// <summary>
        /// 靛蓝色400。
        /// </summary>
        [Description("靛蓝色400")]
        Indigo400 = 0x3D5AFE,
        /// <summary>
        /// 靛蓝色700。
        /// </summary>
        [Description("靛蓝色700")]
        Indigo700 = 0x304FFE,

        /// <summary>
        /// 蓝色100。
        /// </summary>
        [Description("蓝色100")]
        Blue100 = 0x82B1FF,
        /// <summary>
        /// 蓝色200。
        /// </summary>
        [Description("蓝色200")]
        Blue200 = 0x448AFF,
        /// <summary>
        /// 蓝色400。
        /// </summary>
        [Description("蓝色400")]
        Blue400 = 0x2979FF,
        /// <summary>
        /// 蓝色700。
        /// </summary>
        [Description("蓝色700")]
        Blue700 = 0x2962FF,

        /// <summary>
        /// 亮蓝色100。
        /// </summary>
        [Description("亮蓝色100")]
        LightBlue100 = 0x80D8FF,
        /// <summary>
        /// 亮蓝色200。
        /// </summary>
        [Description("亮蓝色200")]
        LightBlue200 = 0x40C4FF,
        /// <summary>
        /// 亮蓝色400。
        /// </summary>
        [Description("亮蓝色400")]
        LightBlue400 = 0x00B0FF,
        /// <summary>
        /// 亮蓝色700。
        /// </summary>
        [Description("亮蓝色700")]
        LightBlue700 = 0x0091EA,

        /// <summary>
        /// 蓝绿色100。
        /// </summary>
        [Description("蓝绿色100")]
        Cyan100 = 0x84FFFF,
        /// <summary>
        /// 蓝绿色200。
        /// </summary>
        [Description("蓝绿色200")]
        Cyan200 = 0x18FFFF,
        /// <summary>
        /// 蓝绿色400。
        /// </summary>
        [Description("蓝绿色400")]
        Cyan400 = 0x00E5FF,
        /// <summary>
        /// 蓝绿色700。
        /// </summary>
        [Description("蓝绿色700")]
        Cyan700 = 0x00B8D4,

        /// <summary>
        /// 青绿色100。
        /// </summary>
        [Description("青绿色100")]
        Teal100 = 0xA7FFEB,
        /// <summary>
        /// 青绿色200。
        /// </summary>
        [Description("青绿色200")]
        Teal200 = 0x64FFDA,
        /// <summary>
        /// 青绿色400。
        /// </summary>
        [Description("青绿色400")]
        Teal400 = 0x1DE9B6,
        /// <summary>
        /// 青绿色700。
        /// </summary>
        [Description("青绿色700")]
        Teal700 = 0x00BFA5,

        /// <summary>
        /// 绿色100。
        /// </summary>
        [Description("绿色100")]
        Green100 = 0xB9F6CA,
        /// <summary>
        /// 绿色200。
        /// </summary>
        [Description("绿色200")]
        Green200 = 0x69F0AE,
        /// <summary>
        /// 绿色400。
        /// </summary>
        [Description("绿色400")]
        Green400 = 0x00E676,
        /// <summary>
        /// 绿色700。
        /// </summary>
        [Description("绿色700")]
        Green700 = 0x00C853,

        /// <summary>
        /// 亮绿色100。
        /// </summary>
        [Description("亮绿色100")]
        LightGreen100 = 0xCCFF90,
        /// <summary>
        /// 亮绿色200。
        /// </summary>
        [Description("亮绿色200")]
        LightGreen200 = 0xB2FF59,
        /// <summary>
        /// 亮绿色400。
        /// </summary>
        [Description("亮绿色400")]
        LightGreen400 = 0x76FF03,
        /// <summary>
        /// 亮绿色700。
        /// </summary>
        [Description("亮绿色700")]
        LightGreen700 = 0x64DD17,

        /// <summary>
        /// 绿黄色100。
        /// </summary>
        [Description("绿黄色100")]
        Lime100 = 0xF4FF81,
        /// <summary>
        /// 绿黄色200。
        /// </summary>
        [Description("绿黄色200")]
        Lime200 = 0xEEFF41,
        /// <summary>
        /// 绿黄色400。
        /// </summary>
        [Description("绿黄色400")]
        Lime400 = 0xC6FF00,
        /// <summary>
        /// 绿黄色700。
        /// </summary>
        [Description("绿黄色700")]
        Lime700 = 0xAEEA00,

        /// <summary>
        /// 黄色100。
        /// </summary>
        [Description("黄色100")]
        Yellow100 = 0xFFFF8D,
        /// <summary>
        /// 黄色200。
        /// </summary>
        [Description("黄色200")]
        Yellow200 = 0xFFFF00,
        /// <summary>
        /// 黄色400。
        /// </summary>
        [Description("黄色400")]
        Yellow400 = 0xFFEA00,
        /// <summary>
        /// 黄色700。
        /// </summary>
        [Description("黄色700")]
        Yellow700 = 0xFFD600,

        /// <summary>
        /// 琥珀色100。
        /// </summary>
        [Description("琥珀色100")]
        Amber100 = 0xFFE57F,
        /// <summary>
        /// 琥珀色200。
        /// </summary>
        [Description("琥珀色200")]
        Amber200 = 0xFFD740,
        /// <summary>
        /// 琥珀色400。
        /// </summary>
        [Description("琥珀色400")]
        Amber400 = 0xFFC400,
        /// <summary>
        /// 琥珀色700。
        /// </summary>
        [Description("琥珀色700")]
        Amber700 = 0xFFAB00,

        /// <summary>
        /// 橙色100。
        /// </summary>
        [Description("橙色100")]
        Orange100 = 0xFFD180,
        /// <summary>
        /// 橙色200。
        /// </summary>
        [Description("橙色200")]
        Orange200 = 0xFFAB40,
        /// <summary>
        /// 橙色400。
        /// </summary>
        [Description("橙色400")]
        Orange400 = 0xFF9100,
        /// <summary>
        /// 橙色700。
        /// </summary>
        [Description("橙色700")]
        Orange700 = 0xFF6D00,

        /// <summary>
        /// 深橙色100。
        /// </summary>
        [Description("深橙色100")]
        DeepOrange100 = 0xFF9E80,
        /// <summary>
        /// 深橙色200。
        /// </summary>
        [Description("深橙色200")]
        DeepOrange200 = 0xFF6E40,
        /// <summary>
        /// 深橙色400。
        /// </summary>
        [Description("深橙色400")]
        DeepOrange400 = 0xFF3D00,
        /// <summary>
        /// 深橙色700。
        /// </summary>
        [Description("深橙色700")]
        DeepOrange700 = 0xDD2C00,
    }
}
