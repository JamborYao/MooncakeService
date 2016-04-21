using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MooncakeTool.Common;

namespace MooncakeTool.Controllers
{
    public class GitHubController : ApiController
    {
        public IEnumerable<string> Get()
        {
           // GitHubDeveloper.GetGitHubPullEntity();
            return new string[] { "value1", "value2" };
        }
        [Route("api/test1")]
        [HttpGet]
        public void Test()
        {
            //GitHubDeveloper.GetGitHubPullEntity();
        }
    }
}