#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Socket
{
    /// <summary>
    /// 套接字适配器接口。
    /// </summary>
    public interface ISocketAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 监听服务端。
        /// </summary>
        /// <param name="port">给定的端口号。</param>
        /// <param name="ip">给定的 IP 地址。</param>
        /// <param name="backlog">可以接受排队等待的传入连接数。</param>
        void ListeningServer(int port, string ip = DefaultSocketAdapter.LOCALHOST, int backlog = 10);

        /// <summary>
        /// 连接到服务端。
        /// </summary>
        /// <param name="port">给定的端口号。</param>
        /// <param name="host">给定的服务端主机。</param>
        /// <param name="sendData">要发送的数据。</param>
        /// <param name="encoding">给定的字符编码（可选；如果要发送数据，则默认为 UTF-8 字符编码）。</param>
        /// <returns>返回接收到的内容。</returns>
        string ConnectedServer(int port, string host = DefaultSocketAdapter.LOCALHOST, string sendData = null, Encoding encoding = null);
    }
}
