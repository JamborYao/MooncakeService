using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace MooncakeTool.Common
{
    public class GitHubDeveloper
    {
        /// <summary>
        /// git git hub pull reqeust to List<GitHubPullRequest>
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<GitHubPullRequest> GetGitHubPullEntity(string url)
        {
            
            object jsonObject = GetGitHubJsonObject(url, "pulls");
            List<GitHubPullRequest> pulls = new List<GitHubPullRequest>();

            var jsonpulls = ((Newtonsoft.Json.Linq.JContainer)jsonObject);
            foreach (var jsonpull in jsonpulls)
            {
                GitHubPullRequest pull = new GitHubPullRequest();
                pull.State = jsonpull.Value<string>("state");
                pull.Number = jsonpull.Value<int>("number");
                pull.Html_Url = jsonpull.Value<string>("html_url");
                pull.Title = jsonpull.Value<string>("title");
                pull.Body = jsonpull.Value<string>("body");
                pull.CreateAt = jsonpull.Value<DateTime>("created_at");
                pull.UpdateAt = jsonpull.Value<DateTime>("updated_at");
                //pull.GitCodeId =;
                pulls.Add(pull);
            }
            return pulls;
        }


        public static List<GitHubIssue> GetGitHubIssuesEntity(string url)
        {
            object jsonObject = GetGitHubJsonObject(url, "issues");
            List<GitHubIssue> issues = new List<GitHubIssue>();
            var jsonissues = ((Newtonsoft.Json.Linq.JContainer)jsonObject);
            foreach (var jsonissue in jsonissues)
            {
                GitHubIssue issue = new GitHubIssue();
                issue.State = jsonissue.Value<string>("state");
                issue.Number = jsonissue.Value<int>("number");
                issue.Html_Url = jsonissue.Value<string>("comments_url");
                issue.Title = jsonissue.Value<string>("title");
                issue.Body = jsonissue.Value<string>("body");
                issue.CreateAt = jsonissue.Value<DateTime>("created_at");
                issue.UpdateAt = jsonissue.Value<DateTime>("updated_at");
                issues.Add(issue);
            }

            return issues;
        }
        public static List<GitHubCommit> GetGitHubCommitsEntity(string url)
        {
            object jsonObject = GetGitHubJsonObject(url, "commits");
            List<GitHubCommit> commits = new List<GitHubCommit>();
            var jsonissues = ((Newtonsoft.Json.Linq.JContainer)jsonObject);
            foreach (var jsonissue in jsonissues)
            {
                GitHubCommit issue = new GitHubCommit();
                issue.Sha = jsonissue.Value<string>("sha");
                issue.Html_Url = jsonissue.Value<string>("html_url");
                foreach (var subjson in jsonissue)
                {
                    if (((Newtonsoft.Json.Linq.JProperty)(subjson)).Name == "parents"&&subjson.FirstOrDefault().Count()>0)
                    {
                        issue.PSha = subjson.FirstOrDefault().First().Value<string>("sha");
                    }
                    if (((Newtonsoft.Json.Linq.JProperty)(subjson)).Name == "commit")
                    {
                        issue.Message = subjson.First().Value<string>("message");
                    }
                }
                commits.Add(issue);
            }

            return commits;
        }

        public static object GetGitHubJsonObject(string url, string type)
        {
            if (url == null) return null;
            url = url.Replace("github.com", "api.github.com/repos");
            if (url.EndsWith("/"))
            {
                url = $"{url}{type}";
            }
            else
            {
                url = $"{url}/{type}";
            }
            List<GitHubPullRequest> pulls = new List<GitHubPullRequest>();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "MooncakeTool";
            String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("jamboryao" + ":" + "123Aking"));
            request.Headers.Add("Authorization", $"Basic {encoded}");
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                var content = reader.ReadToEnd();
                stream.Close();
                reader.Close();
                response.Close();
                var jsonObject = JsonConvert.DeserializeObject(content);
                return jsonObject;
            }
            catch
            {
                throw;
            }
        }

    }
}