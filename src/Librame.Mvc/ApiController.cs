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
using System.Net;
using System.Net.Http;

namespace System.Web.Http
{
    /// <summary>
    /// API 控制器。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TId">指定的主键类型。</typeparam>
    public class ApiController<T, TId> : ApiController, IApiController<T, TId>
    {
        private readonly IService<T> _service = null;

        /// <summary>
        /// 构造一个 <see cref="ApiController{T, TId}"/> 实例。
        /// </summary>
        /// <param name="service">给定的服务接口。</param>
        public ApiController(IService<T> service)
            : base()
        {
            _service = service;
        }


        /// <summary>
        /// 获取服务。
        /// </summary>
        public virtual IService<T> Service
        {
            get { return _service; }
        }


        /// <summary>
        /// 响应 JSON 消息。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定要响应输出的值。</param>
        /// <param name="formatting">要格式化的方式（可选）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回 HTTP 响应消息。</returns>
        protected virtual HttpResponseMessage ResponseJsonMessage<TValue>(TValue value,
            Formatting formatting = Formatting.None,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            var json = value.AsJson(formatting, serializerSettings, converters);

            var encoding = LibrameArchitecture.AdapterManager.Settings.Encoding;

            return new HttpResponseMessage
            {
                Content = new StringContent(json, encoding, "application/json")
            };
        }


        ///// <summary>
        ///// 获取所有类型实例。
        ///// </summary>
        ///// <example>
        ///// GET api/values
        ///// </example>
        ///// <returns>返回类型实例集合。</returns>
        //public virtual HttpResponseMessage Get()
        //{
        //    var items = Service.Repository.GetMany();

        //    return ResponseJsonMessage(items);
        //}

        
        /// <summary>
        /// 获取指定主键的类型实例。
        /// </summary>
        /// <example>
        /// GET api/values/5
        /// </example>
        /// <param name="id">给定的主键。</param>
        /// <returns>返回类型实例。</returns>
        public virtual HttpResponseMessage Get(TId id)
        {
            var item = Service.Repository.Get(id);
            
            if (ReferenceEquals(item, null))
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return ResponseJsonMessage(item);
        }


        ///// <summary>
        ///// 添加类型实例。
        ///// </summary>
        ///// <example>
        ///// POST api/values
        ///// </example>
        ///// <param name="value"></param>
        //public virtual void Post([FromBody]string value)
        //{
        //}


        ///// <summary>
        ///// 更新指定主键类型实例。
        ///// </summary>
        ///// <example>
        ///// PUT api/values/5
        ///// </example>
        ///// <param name="id">给定的主键。</param>
        ///// <param name="value"></param>
        //public virtual void Put(int id, [FromBody]string value)
        //{
        //}

        // PATCH api/values/5


        ///// <summary>
        ///// 删除指定主键类型实例。
        ///// </summary>
        ///// <example>
        ///// DELETE api/values/5
        ///// </example>
        ///// <param name="id">给定的主键。</param>
        //public virtual void Delete(TId id)
        //{
        //    var item = Service.Repository.Get(id);

        //    if (!ReferenceEquals(item, null))
        //        Service.Repository.Delete(item);
        //}


        // HEAD
        // OPTIONS

    }
}
