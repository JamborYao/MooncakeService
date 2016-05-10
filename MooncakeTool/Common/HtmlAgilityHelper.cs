using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;

namespace MooncakeTool.Common
{
    public class HtmlAgilityHelper
    {
        /// <summary>
        /// convert one page html to SampleCode entity and return
        /// </summary>
        /// <param name="html">one page html</param>
        /// <param name="samples">convert one page to SampleCode entity</param>
        /// <param name="num">if not find any sample set num as -1 used to stop do while loop</param>
        /// <returns></returns>
        public static List<SampleCode> HtmlToEntity(string html, ref List<SampleCode> samples, ref int num)
        {
            if (html.IndexOf("unable to find any samples") > 0) { num = -1; }

            else {
                HtmlDocument hdoc = new HtmlDocument();
                hdoc.LoadHtml(html);
                HtmlNodeCollection nodes = hdoc.DocumentNode.SelectNodes("//li[@class='card']");
                foreach (var node in nodes)
                {
                    SampleCode sample = new SampleCode();
                    sample.Title = node.SelectSingleNode(".//span[@class='sheet']//a").InnerText;
                    sample.Description = node.SelectSingleNode(".//span[@class='sheet']//span").InnerText;
                    var link = node.SelectSingleNode(".//span[@class='sheet']//a");
                    var href = link.Attributes["href"].Value;
                    sample.GitResourceUrl = MooncakeTool.Common.HttpRequestHelper. GetGitHubURL(href);
                    sample.Author = node.SelectSingleNode(".//div[@class='meta']//span//a").InnerText;
                    var tempUpdate = node.SelectSingleNode(".//div[@class='meta']//span").InnerText;
                    var dataString = tempUpdate.Substring(tempUpdate.IndexOf(":") + 1);
                    var tt = dataString.Substring(0, dataString.Length - 1);
                    sample.LastUpdate = Convert.ToDateTime(dataString);

                    //add sample code product
                    var products = node.SelectNodes(".//div[@class='tags']//a[@class!='platform-label']");
                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            var pro = product.InnerText;
                            sample.SampleProducts.Add(new SampleProduct() { ProductId = ProductDll.FindProductIDbyName(pro) });
                        }
                    }
                    //add sample code platform
                    var platforms = node.SelectNodes(".//div[@class='tags']//a[@class='platform-label']");
                    if (platforms != null)
                    {
                        foreach (var platform in platforms)
                        {
                            var pla = platform.InnerText;
                            sample.SamplePlatforms.Add(new SamplePlatform() { PlatformId = PlatformDll.FindPlatformIDbyName(pla) });
                        }
                    }
                    //add sample code pull request
                    sample.GitHubPullRequests = MooncakeTool.Common.GitHubDeveloper.GetGitHubPullEntity(sample.GitResourceUrl);
                    sample.GitHubIssues= MooncakeTool.Common.GitHubDeveloper.GetGitHubIssuesEntity(sample.GitResourceUrl);
                    sample.GitHubCommits = MooncakeTool.Common.GitHubDeveloper.GetGitHubCommitsEntity(sample.GitResourceUrl);

                    samples.Add(sample);

                }
            }
            return samples;
        }
       
    }
}