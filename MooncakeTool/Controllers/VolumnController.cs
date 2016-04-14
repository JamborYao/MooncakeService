using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using MooncakeTool.Common;

namespace MooncakeTool.Controllers
{
    public class VolumnController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("api/getVolumn/{startDate}/{endDate}")]
        public HttpResponseMessage Get(string startDate, string endDate)
        {
            Func<DateTime?, DateTime?, object> dllMethod = BaseThreadDll.GetVolumnbyMonth;
            return BaseThreadDll.GetNumbyMonth(startDate, endDate, dllMethod);
        }

        [Route("api/getPageView/{startDate}/{endDate}")]
        public HttpResponseMessage GetPageView(string startDate, string endDate)
        {        
            Func<DateTime?, DateTime?, object> dllMethod = BaseThreadDll.GetPageViewbyMonth;
            return BaseThreadDll.GetNumbyMonth(startDate, endDate, dllMethod);          
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}