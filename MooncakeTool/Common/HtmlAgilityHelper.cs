using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace MooncakeTool.Common
{
    public class HtmlAgilityHelper
    {
        public static List<SampleCode> HtmlToEntity(string html,ref List<SampleCode> samples)
        {
            HtmlDocument hdoc = new HtmlDocument();
            hdoc.LoadHtml(html);

            HtmlNodeCollection nodes = hdoc.DocumentNode.SelectNodes("//li[@class='card']");
            foreach (var node in nodes)
            {
                SampleCode sample = new SampleCode();
                sample.Title = node.SelectSingleNode(".//span[@class='sheet']//a").InnerText;
                sample.Description = node.SelectSingleNode(".//span[@class='sheet']//span").InnerText;

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
            return samples;
        }

        public static void GetTitleDescription(string liContent)
        {
            HtmlDocument hdoc = new HtmlDocument();
            hdoc.LoadHtml(liContent);
        }
    }
}