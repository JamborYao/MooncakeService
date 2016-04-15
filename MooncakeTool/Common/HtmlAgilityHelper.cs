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
                    sample.GitResourceUrl = string.Format("https://github.com/azure-Samples/{0}/", href.Split('/')[4]);

                    var products = node.SelectNodes(".//div[@class='tags']//a[@class!='platform-label']");
                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            var pro = product.InnerText;
                            sample.SampleProducts.Add(new SampleProduct() { ProductId = SampleCodeDll.FindProductIDbyName(pro) });
                        }
                    }
                    var platforms = node.SelectNodes(".//div[@class='tags']//a[@class='platform-label']");
                    if (platforms != null)
                    {
                        foreach (var platform in platforms)
                        {
                            var pla = platform.InnerText;
                            sample.SamplePlatforms.Add(new SamplePlatform() { PlatformId = SampleCodeDll.FindPlatformIDbyName(pla) });
                        }
                    }

                    sample.Author = node.SelectSingleNode(".//div[@class='meta']//span//a").InnerText;
                    var tempUpdate = node.SelectSingleNode(".//div[@class='meta']//span").InnerText;
                    var dataString = tempUpdate.Substring(tempUpdate.IndexOf(":") + 1);
                    var tt = dataString.Substring(0, dataString.Length - 1);
                    sample.LastUpdate = Convert.ToDateTime(dataString);
                    samples.Add(sample);

                }
            }
            return samples;
        }
        private static string sampleCodeUrl = "https://azure.microsoft.com/en-us/documentation/samples/samplesapi/?term=&sortType=0&service=&platform=&pageNumber={0}";
        public static string GetOnePageHtml(int num)
        {
            string body = string.Empty;
            string url = string.Format(sampleCodeUrl, num);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            body = reader.ReadToEnd();

            response.Close();
            reader.Close();
            return body;
        }

    }
}