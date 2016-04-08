using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace MooncakeTool.Controllers
{
    public class VolumnController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(string startDate, string endDate)
        {
            HttpResponseMessage result;
            try
            {
                DateTime? start = Convert.ToDateTime(startDate);
                DateTime? end = Convert.ToDateTime(endDate);
                List<MSDNVolumn_Result> lists = MooncakeTool.Common.BaseThreadDll.GetVolumnbyMonth(start, end);
                string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(lists);
                result = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            }
            catch
            {
                throw new Exception("datetime is incorrect!");
            }
            return result;
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