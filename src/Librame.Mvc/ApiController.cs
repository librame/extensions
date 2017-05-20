#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame;
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

        /// <summary>
        /// 构造一个 API 控制器实例。
        /// </summary>
        /// <param name="repository">给定的数据仓库接口。</param>
        public ApiController(IRepository<T> repository)
            : base()
        {
            _repository = repository;
        }


        /// <summary>
        /// 数据仓库。
        /// </summary>
        public virtual IRepository<T> Repository
        {
            get { return _repository; }
        }
        

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

            var json = model.AsJson(formatting, serializerSettings, converters);
            var encoding = LibrameArchitecture.Adapters.Settings.Encoding;

            return new HttpResponseMessage
            {
                Content = new StringContent(json, encoding, "application/json")
            };
        }

    }
}
