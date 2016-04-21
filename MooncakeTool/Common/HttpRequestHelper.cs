using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MooncakeTool.Common
{
    public class HttpRequestHelper
    {
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

        public static string GetGitHubURL(string url)
        {
            string body = string.Empty, gitUrl = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create($"https://azure.microsoft.com{url}");
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                body = reader.ReadToEnd();
                response.Close();
                reader.Close();
                HtmlDocument hdoc = new HtmlDocument();
                hdoc.LoadHtml(body);
                var link = hdoc.DocumentNode.SelectSingleNode("//a[@id='samples-browse-code']");
                gitUrl = link.Attributes["href"].Value;
                return gitUrl;
            }
            catch (Exception e)
            {
                //throw;
                return null;
            }
        }
    }
}