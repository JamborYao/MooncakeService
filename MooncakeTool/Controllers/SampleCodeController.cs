using MooncakeTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MooncakeTool.Controllers
{
    public class SampleCodeController : ApiController
    {
        private string sampleCodeUrl = "https://azure.microsoft.com/en-us/documentation/samples/samplesapi/?term=&sortType=0&service=&platform=&pageNumber={0}";
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
        [Route("api/catchSampleCode")]
        [HttpGet]
        public void CatchSampleCodeToEntity()
        {
            List<SampleCode> samples = new List<SampleCode>();
            string body = string.Empty;
            int num = 1;
            do
            {
                HtmlAgilityHelper.HtmlToEntity(HtmlAgilityHelper.GetOnePageHtml(num), ref samples,ref num);
                num++;
            }
            while (num>0);

            SampleCodeDll.BatchInsertSampleCode(samples);

        }
        [Route("api/getSamples/{num}")]
        public HttpResponseMessage GetSampleCodeViaPage(int num)
        {
            List<SampleCode> samples = new List<SampleCode>();
            HttpResponseMessage result;
            HtmlAgilityHelper.HtmlToEntity(HtmlAgilityHelper.GetOnePageHtml(num), ref samples, ref num);
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(samples);
            result = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };           
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