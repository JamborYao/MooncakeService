using MooncakeTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                string url = string.Format(sampleCodeUrl, num);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                body = reader.ReadToEnd();
                if (body.IndexOf("unable to find any samples") > 0) break;
                HtmlAgilityHelper.HtmlToEntity(body, ref samples);

               
                response.Close();
                reader.Close();
                num++;
            }
            while (true);
            SampleCodeDll.BatchInsertSampleCode(samples);

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