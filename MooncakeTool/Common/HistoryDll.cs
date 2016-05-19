using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Common
{
    public class HistoryDll
    {
        public static void InsertHistory(History history)
        {
            try
            {
                AzureReportEntities dbContext = new AzureReportEntities();
                dbContext.Histories.Add(history);
                dbContext.SaveChanges();
            }
            catch
            {
                throw new Exception("insert failed");
            }
        }

        public static void FindAllIsNewEntity()
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            //dbContext.
        }

        public static int NewCommitNumber(int? id)
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var result = dbContext.Histories.Where(c => c.GitCodeId == id);

            return result.Count();

        }

        public enum HistoryType
        {
            pull,
            issue,
            commit
        }
    }
}