#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Common.Logging;
using Librame;
using Librame.Authorization;
using Librame.Authorization.Descriptors;
using Librame.Data;
using Librame.Utility;
using Newtonsoft.Json;
using System.Net.Http;

namespace System.Web.Http
{
    /// <summary>
    /// API 控制器。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    //[ApiAuthorize]
    public class ApiController<T> : ApiController, IApiController<T>
        where T : class
    {
        private readonly IRepository<T> _repository = null;
        private readonly ILog _logger = null;

        /// <summary>
        /// 构造一个 API 控制器实例。
        /// </summary>
        /// <param name="repository">给定的数据仓库接口。</param>
        public ApiController(IRepository<T> repository)
            : base()
        {
            _repository = repository;

            if (_logger == null)
                _logger = LibrameArchitecture.Logging.GetLogger<ApiController<T>>();
        }

        
        /// <summary>
        /// 数据仓库。
        /// </summary>
        public IRepository<T> Repository => _repository;

        /// <summary>
        /// 当前日志接口。
        /// </summary>
        public ILog Logger => _logger;

        /// <summary>
        /// 当前认证适配器接口。
        /// </summary>
        public IAuthorizeAdapter Adapter => LibrameArchitecture.Adapters.Authorization;

        /// <summary>
        /// 当前请求的 HTTP 上下文。
        /// </summary>
        public HttpContextBase HttpContext => (HttpContextBase)Request.Properties["MS_HttpContext"];


        /// <summary>
        /// 响应 JSON 消息。
        /// </summary>
        /// <typeparam name="TModel">指定的模型类型。</typeparam>
        /// <param name="model">给定要响应输出的模型实例（如果为空，则返回 404 异常）。</param>
        /// <param name="formatting">要格式化的方式（可选）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回 HTTP 响应消息。</returns>
        protected virtual HttpResponseMessage ResponseJsonMessage<TModel>(TModel model,
            Formatting formatting = Formatting.None,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
            where TModel : class
        {
            (model == null).InvalidHttpRequest();
            
            var encoding = LibrameArchitecture.Adapters.Settings.Encoding;

            StringContent content = null;

            var type = typeof(TModel);
            if (type.IsValueType || type.IsString())
            {
                content = new StringContent(model.ToString(), encoding);
            }
            else
            {
                // 不绕过值类型与字符串类型（已作单独处理）
                var json = model.AsJson(false, formatting, serializerSettings, converters);
                content = new StringContent(json, encoding, JsonHelper.CONTENT_TYPE);
            }

            return new HttpResponseMessage
            {
                Content = content
            };
        }


        /// <summary>
        /// 验证令牌是否有效。
        /// </summary>
        /// <param name="descriptor">输出令牌描述符。</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool ValidateToken(out ITokenDescriptor descriptor)
        {
            // 认证令牌
            var token = string.Empty;

            // 尝试从 HTTP 请求标头中包含的认证信息（Basic 基础认证）
            var authorization = Request.Headers.Authorization;
            if (authorization != null && !string.IsNullOrEmpty(authorization.Parameter))
            {
                // 解码令牌
                token = DecodeToken(authorization.Parameter);
            }
            else
            {
                // 从票根中解析令牌（兼容 .NET WEB 平台；未加密）
                token = HttpContext.Session.ResolveTicket().Token;
            }

            if (string.IsNullOrEmpty(token))
            {
                // 记录调试信息
                Logger.Debug("获取 HTTP 标头或会话状态中的认证信息失败，可能未登录");
                descriptor = null;
                return false;
            }

            return ValidateToken(token, out descriptor);
        }

        /// <summary>
        /// 解码令牌。
        /// </summary>
        /// <param name="encodeToken">给定经过编码的令牌字符串。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string DecodeToken(string encodeToken)
        {
            return Adapter.Managers.Ciphertext.Decode(encodeToken);
        }

        /// <summary>
        /// 验证令牌是否有效。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool ValidateToken(string token)
        {
            ITokenDescriptor descriptor = null;
            return ValidateToken(token, out descriptor);
        }
        /// <summary>
        /// 验证令牌是否有效。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="descriptor">输出令牌描述符。</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool ValidateToken(string token, out ITokenDescriptor descriptor)
        {
            // 默认通过管道进行数据库查询
            descriptor = Adapter.Providers.Token.Authenticate(token);

            return (descriptor != null && !string.IsNullOrEmpty(descriptor.Name)
                && !string.IsNullOrEmpty(descriptor.Ticket));
        }


        /// <summary>
        /// 支持预请求（如谷歌浏览器）。
        /// </summary>
        /// <returns>返回状态 202 的响应消息。</returns>
        [HttpOptions]
        public virtual HttpResponseMessage Options()
        {
            return new HttpResponseMessage
            {
                StatusCode = Net.HttpStatusCode.Accepted
            };
        }

    }
}
