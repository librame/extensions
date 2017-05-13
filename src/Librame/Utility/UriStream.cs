#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;
using System.Net;
using System.Text;

namespace Librame.Utility
{
    /// <summary>
    /// URI 流。
    /// </summary>
    public class UriStream
    {
        private readonly string _url = null;

        /// <summary>
        /// 构造一个 <see cref="UriStream"/> 实例。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        public UriStream(string url)
        {
            _url = url.NotNullOrEmpty(url);
        }


        private const string CONTENT_TYPE = "application/x-www-form-urlencoded";
        private const string CONTENT_TYPE_UPLOAD = "multipart/form-data";

        /// <summary>
        /// 响应 URL 请求的值。
        /// </summary>
        /// <typeparam name="TValue">指定的响应类型。</typeparam>
        /// <param name="responseFactory">给定的响应方法。</param>
        /// <param name="requestFactory">给定的请求方法。</param>
        /// <returns>返回值。</returns>
        protected virtual TValue Response<TValue>(Func<HttpWebResponse, TValue> responseFactory,
            Action<HttpWebRequest> requestFactory = null)
        {
            responseFactory.NotNull(nameof(responseFactory));

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_url);
                //request.Method = UriRequestMethod.Get.ToString();
                //request.ContentType = CONTENT_TYPE;

                // Set some reasonable limits on resources used by this request
                //request.MaximumAutomaticRedirections = 4;
                //request.MaximumResponseHeadersLength = 4;

                // Set credentials to use for this request.
                //request.Credentials = CredentialCache.DefaultCredentials;

                if (!ReferenceEquals(requestFactory, null))
                    requestFactory.Invoke(request);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return responseFactory.Invoke(response);
                }
            }
            catch
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// 获取当前请求的字符编码。
        /// </summary>
        /// <param name="response">给定的 <see cref="HttpWebResponse"/>。</param>
        /// <returns>返回 <see cref="Encoding"/>。</returns>
        protected virtual Encoding GetEncoding(HttpWebResponse response)
        {
            return string.IsNullOrEmpty(response.ContentEncoding) ?
                Encoding.UTF8 : Encoding.GetEncoding(response.ContentEncoding);
        }


        /// <summary>
        /// 获取 URL 请求的内容。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetContent()
        {
            return Response(r =>
            {
                var sb = new StringBuilder();
                using (var rs = r.GetResponseStream())
                {
                    var encoding = GetEncoding(r);
                    using (var sr = new StreamReader(rs, encoding))
                    {
                        // 一次性读取
                        //return sr.ReadToEnd();

                        // 分块流式读取
                        char[] read = new char[256];
                        int count = sr.Read(read, 0, 256);

                        while (count > 0)
                        {
                            string str = new string(read, 0, count);
                            sb.Append(str);

                            count = sr.Read(read, 0, 256);
                        }
                    }
                }

                return sb.ToString();
            });
        }

        /// <summary>
        /// 获取 URL 请求的内容长度。
        /// </summary>
        /// <returns>返回 64 位整数。</returns>
        public virtual long GetContentLength()
        {
            return Response(r =>
            {
                // 未设置内容长度
                if (r.ContentLength == -1)
                {
                    // 支持 64 位整数长度
                    long length = 0;
                    using (var rs = r.GetResponseStream())
                    {
                        var encoding = GetEncoding(r);
                        using (var sr = new StreamReader(rs, encoding))
                        {
                            // 分块流式读取
                            char[] read = new char[256];
                            int count = sr.Read(read, 0, 256);

                            while (count > 0)
                            {
                                string str = new string(read, 0, count);
                                length += str.Length;

                                count = sr.Read(read, 0, 256);
                            }
                        }
                    }

                    return length;
                }
                else
                {
                    return r.ContentLength;
                }
            });
        }

    }
}
