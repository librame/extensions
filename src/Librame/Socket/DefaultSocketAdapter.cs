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
    /// 默认套接字适配器。
    /// </summary>
    public class DefaultSocketAdapter : AbstractSocketAdapter, ISocketAdapter
    {
        /// <summary>
        /// 本机 IP 地址。
        /// </summary>
        internal const string LOCALHOST = "127.0.0.1";


        /// <summary>
        /// 监听服务端。
        /// </summary>
        /// <param name="port">给定的端口号。</param>
        /// <param name="ip">给定的 IP 地址。</param>
        /// <param name="backlog">可以接受排队等待的传入连接数。</param>
        public virtual void ListeningServer(int port, string ip = LOCALHOST, int backlog = 10)
        {
            using (var listener = new SocketListener(port, ip, backlog)) // Start listening
            {
                for (;;)
                {
                    using (var remote = listener.Accept()) // Accepts a connection (blocks execution)
                    {
                        var data = remote.Receive(); // Receives data (blocks execution)
                        remote.Send(data); // Sends the received data back
                    }
                }
            }
        }


        /// <summary>
        /// 连接到服务端。
        /// </summary>
        /// <param name="port">给定的端口号。</param>
        /// <param name="host">给定的服务端主机。</param>
        /// <param name="sendData">要发送的数据。</param>
        /// <param name="encoding">给定的字符编码（可选；如果要发送数据，则默认为 UTF-8 字符编码）。</param>
        /// <returns>返回接收到的内容。</returns>
        public virtual string ConnectedServer(int port, string host = LOCALHOST, string sendData = null, Encoding encoding = null)
        {
            if (ReferenceEquals(encoding, null))
                encoding = Encoding.UTF8;

            using (var socket = new ConnectedSocket(host, port, encoding)) // Connects to 127.0.0.1 on port 1337
            {
                if (!ReferenceEquals(sendData, null))
                    socket.Send(sendData); // Sends some data

                return socket.Receive(); // Receives some data back (blocks execution)
            }
        }

    }
}
