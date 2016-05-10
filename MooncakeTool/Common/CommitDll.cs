using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MooncakeTool.Common
{
    public class CommitDll
    {
        public static void FindAllIsNewEntity()
        {
            AzureReportEntities dbContext = new AzureReportEntities();
            var newCommits = dbContext.GitHubCommits.Where(c => c.IsNew == true);
            if (newCommits != null)
            {
                newCommits.ToList<GitHubCommit>().ForEach((p) =>
                {
                    History history = new History();
                    history.HistoryType = Enum.GetName(typeof(HistoryDll.HistoryType), HistoryDll.HistoryType.commit);
                    history.ForeignId = p.Id;
                    history.GitCodeId = p.GitCodeId;
                    history.IsHistory = true;
                    HistoryDll.InsertHistory(history);
                });
            }

        }
    }
}