using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Newtonsoft;

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

    }
}