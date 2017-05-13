using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Librame.Website.ApiControllers
{
    using Data;

    /// <summary>
    /// 行政区划应用程序接口控制器。
    /// </summary>
    public class PrefecturesController : ApiController<Prefecture, int>
    {
        /// <summary>
        /// 构造一个行政区划控制器。
        /// </summary>
        /// <param name="repository">给定的数据仓库接口。</param>
        public PrefecturesController(IRepository<Prefecture> repository)
            : base(repository)
        {
        }


        /// <summary>
        /// 获取行政区划集合。
        /// </summary>
        /// <example>
        /// GET api/Regions/?page=1&size=1
        /// </example>
        /// <param name="page">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回分页集合。</returns>
        public HttpResponseMessage Get(int page, int size)
        {
            var paging = Repository.GetPagingByIndex(page, size, (order) => order.Asc(s => s.Id));

            return ResponseJsonMessage(paging);
        }

    }
}
