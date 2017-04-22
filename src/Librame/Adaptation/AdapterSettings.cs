#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Adaptation
{
    /// <summary>
    /// 适配器首选项。
    /// </summary>
    public class AdapterSettings : IAdapterSettings
    {
        /// <summary>
        /// 授权编号。
        /// </summary>
        public string AuthId { get; set; }

        /// <summary>
        /// 字符编码。
        /// </summary>
        public System.Text.Encoding Encoding { get; set; }
    }
}
