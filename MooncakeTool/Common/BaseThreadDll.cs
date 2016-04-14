using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Newtonsoft;
using System.Data.Objects;
using System.Net.Http;
using System.Text;

namespace MooncakeTool.Common
{

    public class BaseThreadDll
    {
        static AzureReportEntities dbContext = new AzureReportEntities();

        /// <summary>
        /// batch insert entities
        /// </summary>
        /// <param name="entities"></param>
        public static void InsertBatchBaseThread(List<BaseThread> entities)
        {
            int count = 0;
            dbContext.Configuration.AutoDetectChangesEnabled = false;
            foreach (var entity in entities)
            {
                dbContext.BaseThreads.Add(entity);
                if (count == 3) break;
            }
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public static List<MSDNVolumn_Result> GetVolumnbyMonth(DateTime? startDate,DateTime? endDate)
        {
            List<MSDNVolumn_Result> volumnLists = new List<MSDNVolumn_Result>();
            try
            {
                volumnLists = dbContext.MSDNVolumn(startDate, endDate).ToList<MSDNVolumn_Result>();             
            }
            catch (Exception e)
            {
                throw new Exception("get msdn volumn error");
            }
            return volumnLists;

        }

        public static List<MSDNPageView_Result> GetPageViewbyMonth(DateTime? startDate, DateTime? endDate)
        {
            List<MSDNPageView_Result> volumnLists = new List<MSDNPageView_Result>();
            try
            {
                volumnLists = dbContext.MSDNPageView(startDate, endDate).ToList<MSDNPageView_Result>();
            }
            catch (Exception e)
            {
                throw new Exception("get msdn volumn error");
            }
            return volumnLists;
        }

        public static HttpResponseMessage GetNumbyMonth(string startDate, string endDate,Func<DateTime?,DateTime?,object> CallStored)
        {
            HttpResponseMessage result; DateTime? start, end;
            try
            {
                start = Convert.ToDateTime(startDate);
                end = Convert.ToDateTime(endDate);
            }
            catch
            {
                throw new Exception("datetime is incorrect!");
            }           
            string volumnJson = Newtonsoft.Json.JsonConvert.SerializeObject(CallStored(start, end));
            result = new HttpResponseMessage { Content = new StringContent(volumnJson, Encoding.GetEncoding("gb2312"), "application/json") };
            return result;
        }
    }
}