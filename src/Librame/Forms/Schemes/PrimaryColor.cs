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
    /// 主颜色。
    /// </summary>
    public enum PrimaryColor
    {
        /// <summary>
        /// 红色50。
        /// </summary>
        [Description("红色50")]
        Red50 = 0xFFEBEE,
        /// <summary>
        /// 红色100。
        /// </summary>
        [Description("红色100")]
        Red100 = 0xFFCDD2,
        /// <summary>
        /// 红色200。
        /// </summary>
        [Description("红色200")]
        Red200 = 0xEF9A9A,
        /// <summary>
        /// 红色300。
        /// </summary>
        [Description("红色300")]
        Red300 = 0xE57373,
        /// <summary>
        /// 红色400。
        /// </summary>
        [Description("红色400")]
        Red400 = 0xEF5350,
        /// <summary>
        /// 红色500。
        /// </summary>
        [Description("红色500")]
        Red500 = 0xF44336,
        /// <summary>
        /// 红色600。
        /// </summary>
        [Description("红色600")]
        Red600 = 0xE53935,
        /// <summary>
        /// 红色700。
        /// </summary>
        [Description("红色700")]
        Red700 = 0xD32F2F,
        /// <summary>
        /// 红色800。
        /// </summary>
        [Description("红色800")]
        Red800 = 0xC62828,
        /// <summary>
        /// 红色900。
        /// </summary>
        [Description("红色900")]
        Red900 = 0xB71C1C,

        /// <summary>
        /// 粉色50。
        /// </summary>
        [Description("粉色50")]
        Pink50 = 0xFCE4EC,
        /// <summary>
        /// 粉色100。
        /// </summary>
        [Description("粉色100")]
        Pink100 = 0xF8BBD0,
        /// <summary>
        /// 粉色200。
        /// </summary>
        [Description("粉色200")]
        Pink200 = 0xF48FB1,
        /// <summary>
        /// 粉色300。
        /// </summary>
        [Description("粉色300")]
        Pink300 = 0xF06292,
        /// <summary>
        /// 粉色400。
        /// </summary>
        [Description("粉色400")]
        Pink400 = 0xEC407A,
        /// <summary>
        /// 粉色500。
        /// </summary>
        [Description("粉色500")]
        Pink500 = 0xE91E63,
        /// <summary>
        /// 粉色600。
        /// </summary>
        [Description("粉色600")]
        Pink600 = 0xD81B60,
        /// <summary>
        /// 粉色700。
        /// </summary>
        [Description("粉色700")]
        Pink700 = 0xC2185B,
        /// <summary>
        /// 粉色800。
        /// </summary>
        [Description("粉色800")]
        Pink800 = 0xAD1457,
        /// <summary>
        /// 粉色900。
        /// </summary>
        [Description("粉色900")]
        Pink900 = 0x880E4F,

        /// <summary>
        /// 紫色50。
        /// </summary>
        [Description("紫色50")]
        Purple50 = 0xF3E5F5,
        /// <summary>
        /// 紫色100。
        /// </summary>
        [Description("紫色100")]
        Purple100 = 0xE1BEE7,
        /// <summary>
        /// 紫色200。
        /// </summary>
        [Description("紫色200")]
        Purple200 = 0xCE93D8,
        /// <summary>
        /// 紫色300。
        /// </summary>
        [Description("紫色300")]
        Purple300 = 0xBA68C8,
        /// <summary>
        /// 紫色400。
        /// </summary>
        [Description("紫色400")]
        Purple400 = 0xAB47BC,
        /// <summary>
        /// 紫色500。
        /// </summary>
        [Description("紫色500")]
        Purple500 = 0x9C27B0,
        /// <summary>
        /// 紫色600。
        /// </summary>
        [Description("紫色600")]
        Purple600 = 0x8E24AA,
        /// <summary>
        /// 紫色700。
        /// </summary>
        [Description("紫色700")]
        Purple700 = 0x7B1FA2,
        /// <summary>
        /// 紫色800。
        /// </summary>
        [Description("紫色800")]
        Purple800 = 0x6A1B9A,
        /// <summary>
        /// 紫色900。
        /// </summary>
        [Description("紫色900")]
        Purple900 = 0x4A148C,

        /// <summary>
        /// 深紫色50。
        /// </summary>
        [Description("深紫色50")]
        DeepPurple50 = 0xEDE7F6,
        /// <summary>
        /// 深紫色100。
        /// </summary>
        [Description("深紫色100")]
        DeepPurple100 = 0xD1C4E9,
        /// <summary>
        /// 深紫色200。
        /// </summary>
        [Description("深紫色200")]
        DeepPurple200 = 0xB39DDB,
        /// <summary>
        /// 深紫色300。
        /// </summary>
        [Description("深紫色300")]
        DeepPurple300 = 0x9575CD,
        /// <summary>
        /// 深紫色400。
        /// </summary>
        [Description("深紫色400")]
        DeepPurple400 = 0x7E57C2,
        /// <summary>
        /// 深紫色500。
        /// </summary>
        [Description("深紫色500")]
        DeepPurple500 = 0x673AB7,
        /// <summary>
        /// 深紫色600。
        /// </summary>
        [Description("深紫色600")]
        DeepPurple600 = 0x5E35B1,
        /// <summary>
        /// 深紫色700。
        /// </summary>
        [Description("深紫色700")]
        DeepPurple700 = 0x512DA8,
        /// <summary>
        /// 深紫色800。
        /// </summary>
        [Description("深紫色800")]
        DeepPurple800 = 0x4527A0,
        /// <summary>
        /// 深紫色900。
        /// </summary>
        [Description("深紫色900")]
        DeepPurple900 = 0x311B92,

        /// <summary>
        /// 靛蓝色50。
        /// </summary>
        [Description("靛蓝色50")]
        Indigo50 = 0xE8EAF6,
        /// <summary>
        /// 靛蓝色100。
        /// </summary>
        [Description("靛蓝色100")]
        Indigo100 = 0xC5CAE9,
        /// <summary>
        /// 靛蓝色200。
        /// </summary>
        [Description("靛蓝色200")]
        Indigo200 = 0x9FA8DA,
        /// <summary>
        /// 靛蓝色300。
        /// </summary>
        [Description("靛蓝色300")]
        Indigo300 = 0x7986CB,
        /// <summary>
        /// 靛蓝色400。
        /// </summary>
        [Description("靛蓝色400")]
        Indigo400 = 0x5C6BC0,
        /// <summary>
        /// 靛蓝色500。
        /// </summary>
        [Description("靛蓝色500")]
        Indigo500 = 0x3F51B5,
        /// <summary>
        /// 靛蓝色600。
        /// </summary>
        [Description("靛蓝色600")]
        Indigo600 = 0x3949AB,
        /// <summary>
        /// 靛蓝色700。
        /// </summary>
        [Description("靛蓝色700")]
        Indigo700 = 0x303F9F,
        /// <summary>
        /// 靛蓝色800。
        /// </summary>
        [Description("靛蓝色800")]
        Indigo800 = 0x283593,
        /// <summary>
        /// 靛蓝色900。
        /// </summary>
        [Description("靛蓝色900")]
        Indigo900 = 0x1A237E,

        /// <summary>
        /// 蓝色50。
        /// </summary>
        [Description("蓝色50")]
        Blue50 = 0xE3F2FD,
        /// <summary>
        /// 蓝色100。
        /// </summary>
        [Description("蓝色100")]
        Blue100 = 0xBBDEFB,
        /// <summary>
        /// 蓝色200。
        /// </summary>
        [Description("蓝色200")]
        Blue200 = 0x90CAF9,
        /// <summary>
        /// 蓝色300。
        /// </summary>
        [Description("蓝色300")]
        Blue300 = 0x64B5F6,
        /// <summary>
        /// 蓝色400。
        /// </summary>
        [Description("蓝色400")]
        Blue400 = 0x42A5F5,
        /// <summary>
        /// 蓝色500。
        /// </summary>
        [Description("蓝色500")]
        Blue500 = 0x2196F3,
        /// <summary>
        /// 蓝色600。
        /// </summary>
        [Description("蓝色600")]
        Blue600 = 0x1E88E5,
        /// <summary>
        /// 蓝色700。
        /// </summary>
        [Description("蓝色700")]
        Blue700 = 0x1976D2,
        /// <summary>
        /// 蓝色800。
        /// </summary>
        [Description("蓝色800")]
        Blue800 = 0x1565C0,
        /// <summary>
        /// 蓝色900。
        /// </summary>
        [Description("蓝色900")]
        Blue900 = 0x0D47A1,

        /// <summary>
        /// 亮蓝色50。
        /// </summary>
        [Description("亮蓝色50")]
        LightBlue50 = 0xE1F5FE,
        /// <summary>
        /// 亮蓝色100。
        /// </summary>
        [Description("亮蓝色100")]
        LightBlue100 = 0xB3E5FC,
        /// <summary>
        /// 亮蓝色200。
        /// </summary>
        [Description("亮蓝色200")]
        LightBlue200 = 0x81D4FA,
        /// <summary>
        /// 亮蓝色300。
        /// </summary>
        [Description("亮蓝色300")]
        LightBlue300 = 0x4FC3F7,
        /// <summary>
        /// 亮蓝色400。
        /// </summary>
        [Description("亮蓝色400")]
        LightBlue400 = 0x29B6F6,
        /// <summary>
        /// 亮蓝色500。
        /// </summary>
        [Description("亮蓝色500")]
        LightBlue500 = 0x03A9F4,
        /// <summary>
        /// 亮蓝色600。
        /// </summary>
        [Description("亮蓝色600")]
        LightBlue600 = 0x039BE5,
        /// <summary>
        /// 亮蓝色700。
        /// </summary>
        [Description("亮蓝色700")]
        LightBlue700 = 0x0288D1,
        /// <summary>
        /// 亮蓝色800。
        /// </summary>
        [Description("亮蓝色800")]
        LightBlue800 = 0x0277BD,
        /// <summary>
        /// 亮蓝色900。
        /// </summary>
        [Description("亮蓝色900")]
        LightBlue900 = 0x01579B,

        /// <summary>
        /// 蓝绿色50。
        /// </summary>
        [Description("蓝绿色50")]
        Cyan50 = 0xE0F7FA,
        /// <summary>
        /// 蓝绿色100。
        /// </summary>
        [Description("蓝绿色100")]
        Cyan100 = 0xB2EBF2,
        /// <summary>
        /// 蓝绿色200。
        /// </summary>
        [Description("蓝绿色200")]
        Cyan200 = 0x80DEEA,
        /// <summary>
        /// 蓝绿色300。
        /// </summary>
        [Description("蓝绿色300")]
        Cyan300 = 0x4DD0E1,
        /// <summary>
        /// 蓝绿色400。
        /// </summary>
        [Description("蓝绿色400")]
        Cyan400 = 0x26C6DA,
        /// <summary>
        /// 蓝绿色500。
        /// </summary>
        [Description("蓝绿色500")]
        Cyan500 = 0x00BCD4,
        /// <summary>
        /// 蓝绿色600。
        /// </summary>
        [Description("蓝绿色600")]
        Cyan600 = 0x00ACC1,
        /// <summary>
        /// 蓝绿色700。
        /// </summary>
        [Description("蓝绿色700")]
        Cyan700 = 0x0097A7,
        /// <summary>
        /// 蓝绿色800。
        /// </summary>
        [Description("蓝绿色800")]
        Cyan800 = 0x00838F,
        /// <summary>
        /// 蓝绿色900。
        /// </summary>
        [Description("蓝绿色900")]
        Cyan900 = 0x006064,

        /// <summary>
        /// 青绿色50。
        /// </summary>
        [Description("青绿色50")]
        Teal50 = 0xE0F2F1,
        /// <summary>
        /// 青绿色100。
        /// </summary>
        [Description("青绿色100")]
        Teal100 = 0xB2DFDB,
        /// <summary>
        /// 青绿色200。
        /// </summary>
        [Description("青绿色200")]
        Teal200 = 0x80CBC4,
        /// <summary>
        /// 青绿色300。
        /// </summary>
        [Description("青绿色300")]
        Teal300 = 0x4DB6AC,
        /// <summary>
        /// 青绿色400。
        /// </summary>
        [Description("青绿色400")]
        Teal400 = 0x26A69A,
        /// <summary>
        /// 青绿色500。
        /// </summary>
        [Description("青绿色500")]
        Teal500 = 0x009688,
        /// <summary>
        /// 青绿色600。
        /// </summary>
        [Description("青绿色600")]
        Teal600 = 0x00897B,
        /// <summary>
        /// 青绿色700。
        /// </summary>
        [Description("青绿色700")]
        Teal700 = 0x00796B,
        /// <summary>
        /// 青绿色800。
        /// </summary>
        [Description("青绿色800")]
        Teal800 = 0x00695C,
        /// <summary>
        /// 青绿色900。
        /// </summary>
        [Description("青绿色900")]
        Teal900 = 0x004D40,

        /// <summary>
        /// 绿色50。
        /// </summary>
        [Description("绿色50")]
        Green50 = 0xE8F5E9,
        /// <summary>
        /// 绿色100。
        /// </summary>
        [Description("绿色100")]
        Green100 = 0xC8E6C9,
        /// <summary>
        /// 绿色200。
        /// </summary>
        [Description("绿色200")]
        Green200 = 0xA5D6A7,
        /// <summary>
        /// 绿色300。
        /// </summary>
        [Description("绿色300")]
        Green300 = 0x81C784,
        /// <summary>
        /// 绿色400。
        /// </summary>
        [Description("绿色400")]
        Green400 = 0x66BB6A,
        /// <summary>
        /// 绿色500。
        /// </summary>
        [Description("绿色500")]
        Green500 = 0x4CAF50,
        /// <summary>
        /// 绿色600。
        /// </summary>
        [Description("绿色600")]
        Green600 = 0x43A047,
        /// <summary>
        /// 绿色700。
        /// </summary>
        [Description("绿色700")]
        Green700 = 0x388E3C,
        /// <summary>
        /// 绿色800。
        /// </summary>
        [Description("绿色800")]
        Green800 = 0x2E7D32,
        /// <summary>
        /// 绿色900。
        /// </summary>
        [Description("绿色900")]
        Green900 = 0x1B5E20,

        /// <summary>
        /// 亮绿色50。
        /// </summary>
        [Description("亮绿色50")]
        LightGreen50 = 0xF1F8E9,
        /// <summary>
        /// 亮绿色100。
        /// </summary>
        [Description("亮绿色100")]
        LightGreen100 = 0xDCEDC8,
        /// <summary>
        /// 亮绿色200。
        /// </summary>
        [Description("亮绿色200")]
        LightGreen200 = 0xC5E1A5,
        /// <summary>
        /// 亮绿色300。
        /// </summary>
        [Description("亮绿色300")]
        LightGreen300 = 0xAED581,
        /// <summary>
        /// 亮绿色400。
        /// </summary>
        [Description("亮绿色400")]
        LightGreen400 = 0x9CCC65,
        /// <summary>
        /// 亮绿色500。
        /// </summary>
        [Description("亮绿色500")]
        LightGreen500 = 0x8BC34A,
        /// <summary>
        /// 亮绿色600。
        /// </summary>
        [Description("亮绿色600")]
        LightGreen600 = 0x7CB342,
        /// <summary>
        /// 亮绿色700。
        /// </summary>
        [Description("亮绿色700")]
        LightGreen700 = 0x689F38,
        /// <summary>
        /// 亮绿色800。
        /// </summary>
        [Description("亮绿色800")]
        LightGreen800 = 0x558B2F,
        /// <summary>
        /// 亮绿色900。
        /// </summary>
        [Description("亮绿色900")]
        LightGreen900 = 0x33691E,

        /// <summary>
        /// 绿黄色50。
        /// </summary>
        [Description("绿黄色50")]
        Lime50 = 0xF9FBE7,
        /// <summary>
        /// 绿黄色100。
        /// </summary>
        [Description("绿黄色100")]
        Lime100 = 0xF0F4C3,
        /// <summary>
        /// 绿黄色200。
        /// </summary>
        [Description("绿黄色200")]
        Lime200 = 0xE6EE9C,
        /// <summary>
        /// 绿黄色300。
        /// </summary>
        [Description("绿黄色300")]
        Lime300 = 0xDCE775,
        /// <summary>
        /// 绿黄色400。
        /// </summary>
        [Description("绿黄色400")]
        Lime400 = 0xD4E157,
        /// <summary>
        /// 绿黄色500。
        /// </summary>
        [Description("绿黄色500")]
        Lime500 = 0xCDDC39,
        /// <summary>
        /// 绿黄色600。
        /// </summary>
        [Description("绿黄色600")]
        Lime600 = 0xC0CA33,
        /// <summary>
        /// 绿黄色700。
        /// </summary>
        [Description("绿黄色700")]
        Lime700 = 0xAFB42B,
        /// <summary>
        /// 绿黄色800。
        /// </summary>
        [Description("绿黄色800")]
        Lime800 = 0x9E9D24,
        /// <summary>
        /// 绿黄色900。
        /// </summary>
        [Description("绿黄色900")]
        Lime900 = 0x827717,

        /// <summary>
        /// 黄色50。
        /// </summary>
        [Description("黄色50")]
        Yellow50 = 0xFFFDE7,
        /// <summary>
        /// 黄色100。
        /// </summary>
        [Description("黄色100")]
        Yellow100 = 0xFFF9C4,
        /// <summary>
        /// 黄色200。
        /// </summary>
        [Description("黄色200")]
        Yellow200 = 0xFFF59D,
        /// <summary>
        /// 黄色300。
        /// </summary>
        [Description("黄色300")]
        Yellow300 = 0xFFF176,
        /// <summary>
        /// 黄色400。
        /// </summary>
        [Description("黄色400")]
        Yellow400 = 0xFFEE58,
        /// <summary>
        /// 黄色500。
        /// </summary>
        [Description("黄色500")]
        Yellow500 = 0xFFEB3B,
        /// <summary>
        /// 黄色600。
        /// </summary>
        [Description("黄色600")]
        Yellow600 = 0xFDD835,
        /// <summary>
        /// 黄色700。
        /// </summary>
        [Description("黄色700")]
        Yellow700 = 0xFBC02D,
        /// <summary>
        /// 黄色800。
        /// </summary>
        [Description("黄色800")]
        Yellow800 = 0xF9A825,
        /// <summary>
        /// 黄色900。
        /// </summary>
        [Description("黄色900")]
        Yellow900 = 0xF57F17,

        /// <summary>
        /// 琥珀色50。
        /// </summary>
        [Description("琥珀色50")]
        Amber50 = 0xFFF8E1,
        /// <summary>
        /// 琥珀色100。
        /// </summary>
        [Description("琥珀色100")]
        Amber100 = 0xFFECB3,
        /// <summary>
        /// 琥珀色200。
        /// </summary>
        [Description("琥珀色200")]
        Amber200 = 0xFFE082,
        /// <summary>
        /// 琥珀色300。
        /// </summary>
        [Description("琥珀色300")]
        Amber300 = 0xFFD54F,
        /// <summary>
        /// 琥珀色400。
        /// </summary>
        [Description("琥珀色400")]
        Amber400 = 0xFFCA28,
        /// <summary>
        /// 琥珀色500。
        /// </summary>
        [Description("琥珀色500")]
        Amber500 = 0xFFC107,
        /// <summary>
        /// 琥珀色600。
        /// </summary>
        [Description("琥珀色600")]
        Amber600 = 0xFFB300,
        /// <summary>
        /// 琥珀色700。
        /// </summary>
        [Description("琥珀色700")]
        Amber700 = 0xFFA000,
        /// <summary>
        /// 琥珀色800。
        /// </summary>
        [Description("琥珀色800")]
        Amber800 = 0xFF8F00,
        /// <summary>
        /// 琥珀色900。
        /// </summary>
        [Description("琥珀色900")]
        Amber900 = 0xFF6F00,

        /// <summary>
        /// 橙色50。
        /// </summary>
        [Description("橙色50")]
        Orange50 = 0xFFF3E0,
        /// <summary>
        /// 橙色100。
        /// </summary>
        [Description("橙色100")]
        Orange100 = 0xFFE0B2,
        /// <summary>
        /// 橙色200。
        /// </summary>
        [Description("橙色200")]
        Orange200 = 0xFFCC80,
        /// <summary>
        /// 橙色300。
        /// </summary>
        [Description("橙色300")]
        Orange300 = 0xFFB74D,
        /// <summary>
        /// 橙色400。
        /// </summary>
        [Description("橙色400")]
        Orange400 = 0xFFA726,
        /// <summary>
        /// 橙色500。
        /// </summary>
        [Description("橙色500")]
        Orange500 = 0xFF9800,
        /// <summary>
        /// 橙色600。
        /// </summary>
        [Description("橙色600")]
        Orange600 = 0xFB8C00,
        /// <summary>
        /// 橙色700。
        /// </summary>
        [Description("橙色700")]
        Orange700 = 0xF57C00,
        /// <summary>
        /// 橙色800。
        /// </summary>
        [Description("橙色800")]
        Orange800 = 0xEF6C00,
        /// <summary>
        /// 橙色900。
        /// </summary>
        [Description("橙色900")]
        Orange900 = 0xE65100,

        /// <summary>
        /// 深橙色50。
        /// </summary>
        [Description("深橙色50")]
        DeepOrange50 = 0xFBE9E7,
        /// <summary>
        /// 深橙色100。
        /// </summary>
        [Description("深橙色100")]
        DeepOrange100 = 0xFFCCBC,
        /// <summary>
        /// 深橙色200。
        /// </summary>
        [Description("深橙色200")]
        DeepOrange200 = 0xFFAB91,
        /// <summary>
        /// 深橙色300。
        /// </summary>
        [Description("深橙色300")]
        DeepOrange300 = 0xFF8A65,
        /// <summary>
        /// 深橙色400。
        /// </summary>
        [Description("深橙色400")]
        DeepOrange400 = 0xFF7043,
        /// <summary>
        /// 深橙色500。
        /// </summary>
        [Description("深橙色500")]
        DeepOrange500 = 0xFF5722,
        /// <summary>
        /// 深橙色600。
        /// </summary>
        [Description("深橙色600")]
        DeepOrange600 = 0xF4511E,
        /// <summary>
        /// 深橙色700。
        /// </summary>
        [Description("深橙色700")]
        DeepOrange700 = 0xE64A19,
        /// <summary>
        /// 深橙色800。
        /// </summary>
        [Description("深橙色800")]
        DeepOrange800 = 0xD84315,
        /// <summary>
        /// 深橙色900。
        /// </summary>
        [Description("深橙色900")]
        DeepOrange900 = 0xBF360C,

        /// <summary>
        /// 棕色50。
        /// </summary>
        [Description("棕色50")]
        Brown50 = 0xEFEBE9,
        /// <summary>
        /// 棕色100。
        /// </summary>
        [Description("棕色100")]
        Brown100 = 0xD7CCC8,
        /// <summary>
        /// 棕色200。
        /// </summary>
        [Description("棕色200")]
        Brown200 = 0xBCAAA4,
        /// <summary>
        /// 棕色300。
        /// </summary>
        [Description("棕色300")]
        Brown300 = 0xA1887F,
        /// <summary>
        /// 棕色400。
        /// </summary>
        [Description("棕色400")]
        Brown400 = 0x8D6E63,
        /// <summary>
        /// 棕色500。
        /// </summary>
        [Description("棕色500")]
        Brown500 = 0x795548,
        /// <summary>
        /// 棕色600。
        /// </summary>
        [Description("棕色600")]
        Brown600 = 0x6D4C41,
        /// <summary>
        /// 棕色700。
        /// </summary>
        [Description("棕色700")]
        Brown700 = 0x5D4037,
        /// <summary>
        /// 棕色800。
        /// </summary>
        [Description("棕色800")]
        Brown800 = 0x4E342E,
        /// <summary>
        /// 棕色900。
        /// </summary>
        [Description("棕色900")]
        Brown900 = 0x3E2723,

        /// <summary>
        /// 灰色50。
        /// </summary>
        [Description("灰色50")]
        Grey50 = 0xFAFAFA,
        /// <summary>
        /// 灰色100。
        /// </summary>
        [Description("灰色100")]
        Grey100 = 0xF5F5F5,
        /// <summary>
        /// 灰色200。
        /// </summary>
        [Description("灰色200")]
        Grey200 = 0xEEEEEE,
        /// <summary>
        /// 灰色300。
        /// </summary>
        [Description("灰色300")]
        Grey300 = 0xE0E0E0,
        /// <summary>
        /// 灰色400。
        /// </summary>
        [Description("灰色400")]
        Grey400 = 0xBDBDBD,
        /// <summary>
        /// 灰色500。
        /// </summary>
        [Description("灰色500")]
        Grey500 = 0x9E9E9E,
        /// <summary>
        /// 灰色600。
        /// </summary>
        [Description("灰色600")]
        Grey600 = 0x757575,
        /// <summary>
        /// 灰色700。
        /// </summary>
        [Description("灰色700")]
        Grey700 = 0x616161,
        /// <summary>
        /// 灰色800。
        /// </summary>
        [Description("灰色800")]
        Grey800 = 0x424242,
        /// <summary>
        /// 灰色900。
        /// </summary>
        [Description("灰色900")]
        Grey900 = 0x212121,

        /// <summary>
        /// 蓝灰色50。
        /// </summary>
        [Description("蓝灰色50")]
        BlueGrey50 = 0xECEFF1,
        /// <summary>
        /// 蓝灰色100。
        /// </summary>
        [Description("蓝灰色100")]
        BlueGrey100 = 0xCFD8DC,
        /// <summary>
        /// 蓝灰色200。
        /// </summary>
        [Description("蓝灰色200")]
        BlueGrey200 = 0xB0BEC5,
        /// <summary>
        /// 蓝灰色300。
        /// </summary>
        [Description("蓝灰色300")]
        BlueGrey300 = 0x90A4AE,
        /// <summary>
        /// 蓝灰色400。
        /// </summary>
        [Description("蓝灰色400")]
        BlueGrey400 = 0x78909C,
        /// <summary>
        /// 蓝灰色500。
        /// </summary>
        [Description("蓝灰色500")]
        BlueGrey500 = 0x607D8B,
        /// <summary>
        /// 蓝灰色600。
        /// </summary>
        [Description("蓝灰色600")]
        BlueGrey600 = 0x546E7A,
        /// <summary>
        /// 蓝灰色700。
        /// </summary>
        [Description("蓝灰色700")]
        BlueGrey700 = 0x455A64,
        /// <summary>
        /// 蓝灰色800。
        /// </summary>
        [Description("蓝灰色800")]
        BlueGrey800 = 0x37474F,
        /// <summary>
        /// 蓝灰色900。
        /// </summary>
        [Description("蓝灰色900")]
        BlueGrey900 = 0x263238,
    }
}
