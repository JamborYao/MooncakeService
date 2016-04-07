using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MooncakeTool.Controllers
{
    public class ImportExcelController : ApiController
    {
        public void Get()
        { }

        //do the following in excel
        // 1) copy url in new column
        //  Method:
        //       Sub ExtractHL()
        //          Dim HL As Hyperlink
        //          For Each HL In ActiveSheet.Hyperlinks
        //          HL.Range.Offset(0, 1).Value = HL.Address
        //          Next
        //      End Sub
        // 2) replace all ',' as ';'
        // Post api/<controller>
        public void Post()
        {
            string filePath = @"D:\MooncakeData.csv";
            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(filePath, Encoding.GetEncoding("gb2312"));

            if (lines.Count() < 0) { throw new Exception("csv file line count <0"); }
            string[] headers = lines[0].Split(',');

            List<MooncakeTool.Models.ForumThreads> threads = new List<Models.ForumThreads>();
            for (int i = 1; i < lines.Count(); i++)
            {
               
                string[] item = lines[i].Split(',');
                threads.Add(MooncakeTool.Common.ExcelHelper.ConvertCSVToForumThreads(headers, item, new MooncakeTool.Models.ForumThreads()));
            }
        }




        // GET api/<controller>/5


        // POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}