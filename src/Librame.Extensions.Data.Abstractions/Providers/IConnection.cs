#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 连接接口。
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        string DefaultString { get; set; }

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        string WriteString { get; set; }

        /// <summary>
        /// 写入分离（默认不启用）。
        /// </summary>
        bool WriteSeparation { get; set; }
    }
}
