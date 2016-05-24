using MooncakeTool.Common;
using MooncakeTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace MooncakeTool.Controllers
{
    //[EnableCors(origins: "http://10.168.172.89:89", headers: "*", methods: "*")]
    [EnableCors(origins: "http://localhost:35273", headers: "*", methods: "*")]
    public class SampleCodeController : ApiController
    {
        /// <summaryst>
        /// background service used to update database:
        /// 1) code sample table
        /// 2) githubissue table
        /// 3) githubcommit table
        /// 4) githubpullrequest table
        /// 5) log new data to history table
        /// long time (not to used to client) need to achieve in a certain time (one time a day)      
        /// </summary>
        [Route("api/refreshDatabase")]
        [HttpGet]
        public void CatchSampleCodeToEntity()
        {
            List<SampleCode> samples = new List<SampleCode>();
            string body = string.Empty;
            int num = 1;
            //do while get all page and save to sample code entity
            do
            {
                HtmlAgilityHelper.HtmlToEntity(HttpRequestHelper.GetOnePageHtml(num), ref samples, ref num);
                num++;
            }
            while (num !=0);
            SampleCodeDll.BatchInsertSampleCode(samples);
           // CommitDll.FindAllIsNewEntity();
        }

        /// <summary>
        /// get one page html and return json object(entire)
        /// plan to query data from database and show in client
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        [Route("api/getSamples/{num}")]
        public HttpResponseMessage GetSampleCodeViaPage(int num)
        {
            List<SampleCode> samples = new List<SampleCode>();
            HttpResponseMessage result;
            HtmlAgilityHelper.HtmlToEntity(HttpRequestHelper.GetOnePageHtml(num), ref samples, ref num);
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(samples);
            result = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return result;

        }

        [Route("api/test")]
        [HttpGet]
        public void Test()
        {
            Common.GitHubDeveloper.GetGitHubCommitsEntity("https://github.com/Azure-Samples/storage-blob-dotnet-getting-started");
        }

        [Route("api/getCodeInfobyPage/{page}/{searchKey}/{product}/{platform}/{state}")]
        [HttpGet]
        public HttpResponseMessage GetCodeSampleInfoByPage(int page,string searchKey,int? product,int? platform, int? state)
        {
            var result = SampleCodeDll.GetCardInfo(page,searchKey,product,platform,state);
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }

        [Route("api/getPageNumber/{searchKey}/{product}/{platform}/{state}")]
        [HttpGet]
        public HttpResponseMessage GetPageNumber(string searchKey, int? product, int? platform, int? state)
        {
            var result= SampleCodeDll.GetTotalSampleNumber(searchKey, product, platform, state);
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }

        [Route("api/getSearchPageNumber/{number}")]
        [HttpGet]
        public HttpResponseMessage GetSearchPageNumber(int number)
        {
            var result = SampleCodeDll.GetSearchSampleNumber(number);
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }
        [Route("api/getNewCommit/{id}")]
        [HttpGet]
        public HttpResponseMessage GetNewCommit(int id)
        {
            var result = CommitDll.FindNewCommitById(id);
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }

        [Route("api/getAllProduct")]
        [HttpGet]
        public HttpResponseMessage GetAllProduct()
        {
            List<Product> result = ProductDll.FindAllProduct();
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }

        [Route("api/getAllPlatform")]
        [HttpGet]
        public HttpResponseMessage GetAllPlatform()
        {
            List<Platform> result = PlatformDll.FindAllPlatform();
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }

        [Route("api/searchByTitle/{title}")]
        [HttpGet]
        public HttpResponseMessage SearchByTitle(string title)
        {
            List<CardModel> result = SampleCodeDll.SearchByTitle(title);
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }
        [Route("api/getSampleCodeOperation/{id}")]
        [HttpGet]
        public HttpResponseMessage GetSampleCodeOperation(int id)
        {
            OperationModel result = CodeOperationDll.GetCodeOperation(id);
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }
        [Route("api/getAllState")]
        [HttpGet]
        public HttpResponseMessage GetAllState()
        {
            var result = CodeOperationDll.GetAllCodeState();
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            var json = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return json;
        }

        [Route("api/updateCodeState")]
        [HttpPost]
        public void UpdateCodeState(OperationModel data)
        {
            CodeOperationDll.AddOperation(data);
        }
    }
}