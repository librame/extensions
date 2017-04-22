#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Forms
{
    /// <summary>
    /// 窗体适配器接口。
    /// </summary>
    public interface IFormsAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取或设置 <see cref="FormsSettings"/>。
        /// </summary>
        FormsSettings FormsSettings { get; set; }


        /// <summary>
        /// 获取方案构建器。
        /// </summary>
        Schemes.ISchemeBuilder Scheme { get; }

        /// <summary>
        /// 获取皮肤提供程序。
        /// </summary>
        ISkinProvider Skin { get; }
    }
}
